using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using PrismaPrimeMarket.Application.DTOs.Auth;
using PrismaPrimeMarket.Application.DTOs.User;
using PrismaPrimeMarket.Application.Mappings;
using PrismaPrimeMarket.Application.UseCases.Auth.Commands.Login;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Exceptions;
using PrismaPrimeMarket.Domain.Interfaces;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;

namespace PrismaPrimeMarket.UnitTests.Application.UseCases.Auth.Commands;

public class LoginCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IMapper _mapper;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        // Setup UserManager mock
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        // Setup AutoMapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UserProfile>();
        });
        _mapper = config.CreateMapper();

        _handler = new LoginCommandHandler(
            _userManagerMock.Object,
            _jwtTokenServiceMock.Object,
            _refreshTokenRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapper
        );
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ShouldReturnAuthResponse()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "SecurePass123!");
        var user = User.Create("testuser", "Test", "test@example.com");
        var roles = new List<string> { "User" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, command.Password))
            .ReturnsAsync(true);

        _userManagerMock.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        _jwtTokenServiceMock.Setup(x => x.GenerateAccessToken(user.Id, user.Email!, roles))
            .Returns("access_token");

        _jwtTokenServiceMock.Setup(x => x.GenerateRefreshToken())
            .Returns("refresh_token");

        _jwtTokenServiceMock.Setup(x => x.ParseExpirationConfig("Jwt:AccessExpiration"))
            .Returns(TimeSpan.FromMinutes(15));

        _jwtTokenServiceMock.Setup(x => x.ParseExpirationConfig("Jwt:RefreshExpiration"))
            .Returns(TimeSpan.FromDays(7));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.User.Should().NotBeNull();
        result.User.Email.Should().Be(user.Email);
        result.Tokens.Should().NotBeNull();
        result.Tokens.AccessToken.Should().Be("access_token");
        result.Tokens.RefreshToken.Should().Be("refresh_token");

        _refreshTokenRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Exactly(2)); // Login registration + refresh token save
    }

    [Fact]
    public async Task Handle_WithNonExistentEmail_ShouldThrowInvalidCredentialsException()
    {
        // Arrange
        var command = new LoginCommand("nonexistent@example.com", "SecurePass123!");

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync((User?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidCredentialsException>();

        _userManagerMock.Verify(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        _refreshTokenRepositoryMock.Verify(x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidPassword_ShouldThrowInvalidCredentialsException()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "WrongPassword123!");
        var user = User.Create("testuser", "Test", "test@example.com");

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, command.Password))
            .ReturnsAsync(false);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidCredentialsException>();

        _jwtTokenServiceMock.Verify(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IList<string>>()), Times.Never);
        _refreshTokenRepositoryMock.Verify(x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInactiveUser_ShouldThrowInvalidCredentialsException()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "SecurePass123!");
        var user = User.Create("testuser", "Test", "test@example.com");
        user.Deactivate(); // Desativa o usuário

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, command.Password))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidCredentialsException>()
            .WithMessage("*inativo*");

        _jwtTokenServiceMock.Verify(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IList<string>>()), Times.Never);
        _refreshTokenRepositoryMock.Verify(x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ShouldRegisterLoginAndGenerateTokens()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "SecurePass123!");
        var user = User.Create("testuser", "Test", "test@example.com");
        var roles = new List<string> { "User" };
        var accessExpiration = TimeSpan.FromMinutes(15);
        var refreshExpiration = TimeSpan.FromDays(7);

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, command.Password))
            .ReturnsAsync(true);

        _userManagerMock.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        _jwtTokenServiceMock.Setup(x => x.GenerateAccessToken(user.Id, user.Email!, roles))
            .Returns("access_token");

        _jwtTokenServiceMock.Setup(x => x.GenerateRefreshToken())
            .Returns("refresh_token");

        _jwtTokenServiceMock.Setup(x => x.ParseExpirationConfig("Jwt:AccessExpiration"))
            .Returns(accessExpiration);

        _jwtTokenServiceMock.Setup(x => x.ParseExpirationConfig("Jwt:RefreshExpiration"))
            .Returns(refreshExpiration);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Tokens.AccessTokenExpiresAt.Should().BeCloseTo(DateTime.UtcNow.Add(accessExpiration), TimeSpan.FromSeconds(5));
        result.Tokens.RefreshTokenExpiresAt.Should().BeCloseTo(DateTime.UtcNow.Add(refreshExpiration), TimeSpan.FromSeconds(5));

        _refreshTokenRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<RefreshToken>(rt => rt.Token == "refresh_token" && rt.UserId == user.Id),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ShouldIncludeUserRolesInResponse()
    {
        // Arrange
        var command = new LoginCommand("admin@example.com", "SecurePass123!");
        var user = User.Create("adminuser", "Admin", "admin@example.com");
        var roles = new List<string> { "Admin", "User" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, command.Password))
            .ReturnsAsync(true);

        _userManagerMock.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        _jwtTokenServiceMock.Setup(x => x.GenerateAccessToken(user.Id, user.Email!, roles))
            .Returns("access_token");

        _jwtTokenServiceMock.Setup(x => x.GenerateRefreshToken())
            .Returns("refresh_token");

        _jwtTokenServiceMock.Setup(x => x.ParseExpirationConfig(It.IsAny<string>()))
            .Returns(TimeSpan.FromMinutes(15));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.User.Roles.Should().BeEquivalentTo(roles);

        _jwtTokenServiceMock.Verify(
            x => x.GenerateAccessToken(user.Id, user.Email!, roles),
            Times.Once);
    }
}
