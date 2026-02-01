using FluentAssertions;
using PrismaPrimeMarket.Application.UseCases.Auth.Commands.Login;

namespace PrismaPrimeMarket.UnitTests.Application.UseCases.Auth.Commands;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator;

    public LoginCommandValidatorTests()
    {
        _validator = new LoginCommandValidator();
    }

    [Fact]
    public void Validate_WithValidEmail_ShouldPass()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "SecureP@ss123");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithValidUsername_ShouldPass()
    {
        // Arrange
        var command = new LoginCommand("testuser", "SecureP@ss123");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyUsernameOrEmail_ShouldFail()
    {
        // Arrange
        var command = new LoginCommand("", "SecureP@ss123");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "UsernameOrEmail");
    }

    [Fact]
    public void Validate_WithTooShortUsernameOrEmail_ShouldFail()
    {
        // Arrange
        var command = new LoginCommand("ab", "SecureP@ss123");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "UsernameOrEmail");
    }

    [Fact]
    public void Validate_WithEmptyPassword_ShouldFail()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }

    [Fact]
    public void Validate_WithTooShortPassword_ShouldFail()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "Short1!");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }

    [Fact]
    public void Validate_WithPasswordMissingUppercase_ShouldFail()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "securep@ss123");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password" && e.ErrorMessage.Contains("maiúscula"));
    }

    [Fact]
    public void Validate_WithPasswordMissingLowercase_ShouldFail()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "SECUREP@SS123");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password" && e.ErrorMessage.Contains("minúscula"));
    }

    [Fact]
    public void Validate_WithPasswordMissingNumber_ShouldFail()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "SecureP@ssword");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password" && e.ErrorMessage.Contains("número"));
    }

    [Fact]
    public void Validate_WithPasswordMissingSpecialCharacter_ShouldFail()
    {
        // Arrange
        var command = new LoginCommand("test@example.com", "SecurePass123");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password" && e.ErrorMessage.Contains("caractere especial"));
    }
}
