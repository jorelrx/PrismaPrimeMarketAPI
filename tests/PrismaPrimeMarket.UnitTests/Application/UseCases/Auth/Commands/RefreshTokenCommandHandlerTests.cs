using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using PrismaPrimeMarket.Application.UseCases.Auth.Commands.RefreshToken;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Exceptions;
using PrismaPrimeMarket.Domain.Interfaces;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;

namespace PrismaPrimeMarket.UnitTests.Application.UseCases.Auth.Commands;

public class RefreshTokenCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RefreshTokenCommandHandler _handler;

    public RefreshTokenCommandHandlerTests()
    {
        // Setup UserManager mock
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new RefreshTokenCommandHandler(
            _userManagerMock.Object,
            _jwtTokenServiceMock.Object,
            _refreshTokenRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRefreshToken_ShouldReturnNewTokens()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        var refreshTokenEntity = RefreshToken.Create(
            "valid_refresh_token",
            user.Id,
            DateTime.UtcNow.AddDays(7));

        var command = new RefreshTokenCommand("valid_refresh_token");
        var roles = new List<string> { "User" };

        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync(command.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(refreshTokenEntity);

        _userManagerMock.Setup(x => x.FindByIdAsync(refreshTokenEntity.UserId.ToString()))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        _jwtTokenServiceMock.Setup(x => x.GenerateAccessToken(user.Id, user.Email!, roles))
            .Returns("new_access_token");

        _jwtTokenServiceMock.Setup(x => x.GenerateRefreshToken())
            .Returns("new_refresh_token");

        _jwtTokenServiceMock.Setup(x => x.ParseExpirationConfig("Jwt:AccessExpiration"))
            .Returns(TimeSpan.FromMinutes(15));

        _jwtTokenServiceMock.Setup(x => x.ParseExpirationConfig("Jwt:RefreshExpiration"))
            .Returns(TimeSpan.FromDays(7));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("new_access_token");
        result.RefreshToken.Should().Be("new_refresh_token");
        result.AccessTokenExpiresAt.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(15), TimeSpan.FromSeconds(5));
        result.RefreshTokenExpiresAt.Should().BeCloseTo(DateTime.UtcNow.AddDays(7), TimeSpan.FromSeconds(5));

        _refreshTokenRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidRefreshToken_ShouldThrowInvalidTokenException()
    {
        // Arrange
        var command = new RefreshTokenCommand("invalid_refresh_token");

        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync(command.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync((RefreshToken?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidTokenException>()
            .WithMessage("*inválido*");

        _jwtTokenServiceMock.Verify(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IList<string>>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithExpiredRefreshToken_ShouldThrowInvalidTokenException()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        // Cria um token que vai expirar em 1 segundo
        var expiredToken = RefreshToken.Create(
            "expired_refresh_token",
            user.Id,
            DateTime.UtcNow.AddSeconds(1));

        // Aguarda para o token expirar
        await Task.Delay(1100);

        var command = new RefreshTokenCommand("expired_refresh_token");

        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync(command.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expiredToken);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidTokenException>()
            .WithMessage("*inválido ou expirado*");

        _jwtTokenServiceMock.Verify(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IList<string>>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithRevokedRefreshToken_ShouldThrowInvalidTokenException()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        var revokedToken = RefreshToken.Create(
            "revoked_refresh_token",
            user.Id,
            DateTime.UtcNow.AddDays(7));

        revokedToken.Revoke("new_token"); // Revoga o token

        var command = new RefreshTokenCommand("revoked_refresh_token");

        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync(command.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(revokedToken);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidTokenException>()
            .WithMessage("*inválido ou expirado*");

        _jwtTokenServiceMock.Verify(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IList<string>>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInactiveUser_ShouldThrowInvalidTokenException()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        user.Deactivate(); // Desativa o usuário

        var refreshTokenEntity = RefreshToken.Create(
            "valid_refresh_token",
            user.Id,
            DateTime.UtcNow.AddDays(7));

        var command = new RefreshTokenCommand("valid_refresh_token");

        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync(command.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(refreshTokenEntity);

        _userManagerMock.Setup(x => x.FindByIdAsync(refreshTokenEntity.UserId.ToString()))
            .ReturnsAsync(user);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidTokenException>()
            .WithMessage("*inválido ou expirado*");

        _jwtTokenServiceMock.Verify(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IList<string>>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithNonExistentUser_ShouldThrowInvalidTokenException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var refreshTokenEntity = RefreshToken.Create(
            "valid_refresh_token",
            userId,
            DateTime.UtcNow.AddDays(7));

        var command = new RefreshTokenCommand("valid_refresh_token");

        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync(command.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(refreshTokenEntity);

        _userManagerMock.Setup(x => x.FindByIdAsync(refreshTokenEntity.UserId.ToString()))
            .ReturnsAsync((User)null!);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidTokenException>()
            .WithMessage("*inválido ou expirado*");

        _jwtTokenServiceMock.Verify(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<IList<string>>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithValidRefreshToken_ShouldRevokeOldTokenAndCreateNew()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        var oldRefreshToken = RefreshToken.Create(
            "old_refresh_token",
            user.Id,
            DateTime.UtcNow.AddDays(7));

        var command = new RefreshTokenCommand("old_refresh_token");
        var roles = new List<string> { "User" };

        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync(command.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(oldRefreshToken);

        _userManagerMock.Setup(x => x.FindByIdAsync(oldRefreshToken.UserId.ToString()))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        _jwtTokenServiceMock.Setup(x => x.GenerateAccessToken(user.Id, user.Email!, roles))
            .Returns("new_access_token");

        _jwtTokenServiceMock.Setup(x => x.GenerateRefreshToken())
            .Returns("new_refresh_token");

        _jwtTokenServiceMock.Setup(x => x.ParseExpirationConfig(It.IsAny<string>()))
            .Returns(TimeSpan.FromMinutes(15));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        oldRefreshToken.IsActive.Should().BeFalse();
        oldRefreshToken.RevokedAt.Should().NotBeNull();
        oldRefreshToken.ReplacedByToken.Should().Be("new_refresh_token");

        _refreshTokenRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<RefreshToken>(rt =>
                    rt.Token == "new_refresh_token" &&
                    rt.UserId == user.Id &&
                    rt.IsActive),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidRefreshToken_ShouldGenerateTokensWithCorrectUserRoles()
    {
        // Arrange
        var user = User.Create("adminuser", "Admin", "admin@example.com");
        var refreshTokenEntity = RefreshToken.Create(
            "valid_refresh_token",
            user.Id,
            DateTime.UtcNow.AddDays(7));

        var command = new RefreshTokenCommand("valid_refresh_token");
        var roles = new List<string> { "Admin", "User" };

        _refreshTokenRepositoryMock.Setup(x => x.GetByTokenAsync(command.RefreshToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(refreshTokenEntity);

        _userManagerMock.Setup(x => x.FindByIdAsync(refreshTokenEntity.UserId.ToString()))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        _jwtTokenServiceMock.Setup(x => x.GenerateAccessToken(user.Id, user.Email!, roles))
            .Returns("new_access_token");

        _jwtTokenServiceMock.Setup(x => x.GenerateRefreshToken())
            .Returns("new_refresh_token");

        _jwtTokenServiceMock.Setup(x => x.ParseExpirationConfig(It.IsAny<string>()))
            .Returns(TimeSpan.FromMinutes(15));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _jwtTokenServiceMock.Verify(
            x => x.GenerateAccessToken(user.Id, user.Email!, roles),
            Times.Once);
    }
}
