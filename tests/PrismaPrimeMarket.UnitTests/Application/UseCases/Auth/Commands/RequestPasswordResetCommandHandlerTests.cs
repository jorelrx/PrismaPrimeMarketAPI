using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using PrismaPrimeMarket.Application.UseCases.Auth.Commands.RequestPasswordReset;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Interfaces;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;

namespace PrismaPrimeMarket.UnitTests.Application.UseCases.Auth.Commands;

public class RequestPasswordResetCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IPasswordResetRepository> _passwordResetRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<RequestPasswordResetCommandHandler>> _loggerMock;
    private readonly RequestPasswordResetCommandHandler _handler;

    public RequestPasswordResetCommandHandlerTests()
    {
        // Setup UserManager mock
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        _passwordResetRepositoryMock = new Mock<IPasswordResetRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<RequestPasswordResetCommandHandler>>();

        _handler = new RequestPasswordResetCommandHandler(
            _userManagerMock.Object,
            _passwordResetRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidEmail_ShouldGenerateResetTokenAndSaveToDatabase()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        var command = new RequestPasswordResetCommand("test@example.com");
        var resetToken = "reset_token_123";

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user))
            .ReturnsAsync(resetToken);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);

        _passwordResetRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<PasswordReset>(pr =>
                    pr.Token == resetToken &&
                    pr.UserId == user.Id &&
                    pr.IsValid),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(user.Id.ToString())),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentEmail_ShouldReturnSuccessWithoutGeneratingToken()
    {
        // Arrange
        var command = new RequestPasswordResetCommand("nonexistent@example.com");

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert - Por segurança, retorna sucesso mesmo que o usuário não exista
        result.Should().Be(Unit.Value);

        _userManagerMock.Verify(x => x.GeneratePasswordResetTokenAsync(It.IsAny<User>()), Times.Never);
        _passwordResetRepositoryMock.Verify(x => x.AddAsync(It.IsAny<PasswordReset>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("não cadastrado")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidEmail_ShouldGenerateTokenWithCorrectExpiration()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        var command = new RequestPasswordResetCommand("test@example.com");
        var resetToken = "reset_token_123";
        var expectedExpiration = TimeSpan.FromHours(1);

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user))
            .ReturnsAsync(resetToken);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _passwordResetRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<PasswordReset>(pr =>
                    pr.ExpiresAt > DateTime.UtcNow &&
                    pr.ExpiresAt <= DateTime.UtcNow.Add(expectedExpiration).AddMinutes(1)),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithMultipleRequests_ShouldGenerateNewTokenForEachRequest()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        var command = new RequestPasswordResetCommand("test@example.com");
        var firstToken = "reset_token_1";
        var secondToken = "reset_token_2";

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _userManagerMock.SetupSequence(x => x.GeneratePasswordResetTokenAsync(user))
            .ReturnsAsync(firstToken)
            .ReturnsAsync(secondToken);

        // Act
        await _handler.Handle(command, CancellationToken.None);
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _passwordResetRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<PasswordReset>(), It.IsAny<CancellationToken>()),
            Times.Exactly(2));

        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Handle_WithValidEmail_ShouldCreatePasswordResetWithIsValidTrue()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        var command = new RequestPasswordResetCommand("test@example.com");
        var resetToken = "reset_token_123";

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user))
            .ReturnsAsync(resetToken);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _passwordResetRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<PasswordReset>(pr => pr.IsValid),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserManagerGeneratesToken_ShouldUseIdentityToken()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        var command = new RequestPasswordResetCommand("test@example.com");
        var identityGeneratedToken = "identity_generated_token_xyz";

        _userManagerMock.Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user))
            .ReturnsAsync(identityGeneratedToken);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _passwordResetRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<PasswordReset>(pr => pr.Token == identityGeneratedToken),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
