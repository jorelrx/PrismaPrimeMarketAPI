using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Moq;
using PrismaPrimeMarket.Application.UseCases.Auth.Commands.ConfirmPasswordReset;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Exceptions;
using PrismaPrimeMarket.Domain.Interfaces;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;

namespace PrismaPrimeMarket.UnitTests.Application.UseCases.Auth.Commands;

public class ConfirmPasswordResetCommandHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IPasswordResetRepository> _passwordResetRepositoryMock;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ConfirmPasswordResetCommandHandler _handler;

    public ConfirmPasswordResetCommandHandlerTests()
    {
        // Setup UserManager mock
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        _passwordResetRepositoryMock = new Mock<IPasswordResetRepository>();
        _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new ConfirmPasswordResetCommandHandler(
            _userManagerMock.Object,
            _passwordResetRepositoryMock.Object,
            _refreshTokenRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidTokenAndPassword_ShouldResetPasswordSuccessfully()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        var resetToken = "valid_reset_token";
        var passwordReset = PasswordReset.Create(resetToken, user.Id, TimeSpan.FromHours(1));
        var command = new ConfirmPasswordResetCommand(resetToken, "NewSecurePass123!");

        _passwordResetRepositoryMock.Setup(x => x.GetByTokenAsync(resetToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(passwordReset);

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, resetToken, command.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);

        _userManagerMock.Verify(
            x => x.ResetPasswordAsync(user, resetToken, command.NewPassword),
            Times.Once);

        _passwordResetRepositoryMock.Verify(
            x => x.InvalidateAllUserResetsAsync(user.Id, It.IsAny<CancellationToken>()),
            Times.Once);

        _refreshTokenRepositoryMock.Verify(
            x => x.RevokeAllUserTokensAsync(user.Id, It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidToken_ShouldThrowInvalidTokenException()
    {
        // Arrange
        var command = new ConfirmPasswordResetCommand("invalid_token", "NewSecurePass123!");

        _passwordResetRepositoryMock.Setup(x => x.GetByTokenAsync(command.Token, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PasswordReset?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidTokenException>()
            .WithMessage("*inválido ou expirado*");

        _userManagerMock.Verify(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithExpiredToken_ShouldThrowInvalidTokenException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expiredToken = "expired_token";
        // Cria um token que vai expirar em 1 segundo
        var passwordReset = PasswordReset.Create(expiredToken, userId, TimeSpan.FromSeconds(1));

        // Aguarda para o token expirar
        await Task.Delay(1100);

        var command = new ConfirmPasswordResetCommand(expiredToken, "NewSecurePass123!");

        _passwordResetRepositoryMock.Setup(x => x.GetByTokenAsync(expiredToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(passwordReset);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidTokenException>()
            .WithMessage("*inválido ou expirado*");

        _userManagerMock.Verify(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithAlreadyUsedToken_ShouldThrowInvalidTokenException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var usedToken = "used_token";
        var passwordReset = PasswordReset.Create(usedToken, userId, TimeSpan.FromHours(1));
        passwordReset.MarkAsUsed(); // Marca o token como usado

        var command = new ConfirmPasswordResetCommand(usedToken, "NewSecurePass123!");

        _passwordResetRepositoryMock.Setup(x => x.GetByTokenAsync(usedToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(passwordReset);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidTokenException>()
            .WithMessage("*inválido ou expirado*");

        _userManagerMock.Verify(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithNonExistentUser_ShouldThrowUserNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var resetToken = "valid_token";
        var passwordReset = PasswordReset.Create(resetToken, userId, TimeSpan.FromHours(1));
        var command = new ConfirmPasswordResetCommand(resetToken, "NewSecurePass123!");

        _passwordResetRepositoryMock.Setup(x => x.GetByTokenAsync(resetToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(passwordReset);

        _userManagerMock.Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync((User)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UserNotFoundException>();

        _userManagerMock.Verify(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithIdentityFailure_ShouldThrowValidationException()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        var resetToken = "valid_token";
        var passwordReset = PasswordReset.Create(resetToken, user.Id, TimeSpan.FromHours(1));
        var command = new ConfirmPasswordResetCommand(resetToken, "weak");

        var identityErrors = new[]
        {
            new IdentityError { Code = "PasswordTooShort", Description = "Senha muito curta" },
            new IdentityError { Code = "PasswordRequiresNonAlphanumeric", Description = "Senha requer caractere especial" }
        };

        _passwordResetRepositoryMock.Setup(x => x.GetByTokenAsync(resetToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(passwordReset);

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, resetToken, command.NewPassword))
            .ReturnsAsync(IdentityResult.Failed(identityErrors));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var exception = await act.Should().ThrowAsync<ValidationException>();
        exception.Which.Errors.Should().ContainKey("PasswordTooShort");

        _passwordResetRepositoryMock.Verify(
            x => x.InvalidateAllUserResetsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);

        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithValidToken_ShouldMarkTokenAsUsed()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        var resetToken = "valid_token";
        var passwordReset = PasswordReset.Create(resetToken, user.Id, TimeSpan.FromHours(1));
        var command = new ConfirmPasswordResetCommand(resetToken, "NewSecurePass123!");

        _passwordResetRepositoryMock.Setup(x => x.GetByTokenAsync(resetToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(passwordReset);

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, resetToken, command.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        passwordReset.IsValid.Should().BeFalse();
        passwordReset.UsedAt.Should().NotBeNull();
        passwordReset.UsedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_WithValidToken_ShouldInvalidateAllUserPasswordResets()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        var resetToken = "valid_token";
        var passwordReset = PasswordReset.Create(resetToken, user.Id, TimeSpan.FromHours(1));
        var command = new ConfirmPasswordResetCommand(resetToken, "NewSecurePass123!");

        _passwordResetRepositoryMock.Setup(x => x.GetByTokenAsync(resetToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(passwordReset);

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, resetToken, command.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _passwordResetRepositoryMock.Verify(
            x => x.InvalidateAllUserResetsAsync(user.Id, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidToken_ShouldRevokeAllUserRefreshTokens()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        var resetToken = "valid_token";
        var passwordReset = PasswordReset.Create(resetToken, user.Id, TimeSpan.FromHours(1));
        var command = new ConfirmPasswordResetCommand(resetToken, "NewSecurePass123!");

        _passwordResetRepositoryMock.Setup(x => x.GetByTokenAsync(resetToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(passwordReset);

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, resetToken, command.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _refreshTokenRepositoryMock.Verify(
            x => x.RevokeAllUserTokensAsync(user.Id, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithSuccessfulReset_ShouldCommitAllChanges()
    {
        // Arrange
        var user = User.Create("testuser", "Test", "test@example.com");
        var resetToken = "valid_token";
        var passwordReset = PasswordReset.Create(resetToken, user.Id, TimeSpan.FromHours(1));
        var command = new ConfirmPasswordResetCommand(resetToken, "NewSecurePass123!");

        _passwordResetRepositoryMock.Setup(x => x.GetByTokenAsync(resetToken, It.IsAny<CancellationToken>()))
            .ReturnsAsync(passwordReset);

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, resetToken, command.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
