using FluentAssertions;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Exceptions;
using PrismaPrimeMarket.Domain.ValueObjects;

namespace PrismaPrimeMarket.UnitTests.Domain.Entities;

public class UserTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var userName = "johndoe";
        var firstName = "John";

        // Act
        var user = User.Create(userName, firstName);

        // Assert
        user.Should().NotBeNull();
        user.UserName.Should().Be(userName);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().BeNullOrEmpty();
        user.Email.Should().BeNull();
        user.IsActive.Should().BeTrue();
        user.IsDeleted.Should().BeFalse();
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void Create_WithAllOptionalFields_ShouldCreateUser()
    {
        // Arrange
        var userName = "johndoe";
        var firstName = "John";
        var email = "john@example.com";
        var lastName = "Doe";
        var cpf = CPF.Create("12345678909");
        var phone = PhoneNumber.Create("11987654321");
        var birthDate = DateTime.UtcNow.AddYears(-20);

        // Act
        var user = User.Create(userName, firstName, email, lastName, cpf, phone, birthDate);

        // Assert
        user.Should().NotBeNull();
        user.UserName.Should().Be(userName);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Email.Should().Be(email);
        user.CPF.Should().Be(cpf);
        user.Phone.Should().Be(phone);
        user.BirthDate.Should().Be(birthDate);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidUserName_ShouldThrowDomainException(string? invalidUserName)
    {
        // Arrange
        var firstName = "John";

        // Act
        Action act = () => User.Create(invalidUserName!, firstName);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*nome de usuário*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidFirstName_ShouldThrowDomainException(string? invalidFirstName)
    {
        // Arrange
        var userName = "johndoe";

        // Act
        Action act = () => User.Create(userName, invalidFirstName!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*não pode ser vazio*");
    }

    [Fact]
    public void UpdateProfile_WithValidData_ShouldUpdateUser()
    {
        // Arrange
        var user = User.Create("johndoe", "John");
        var newFirstName = "Jane";
        var newLastName = "Doe";

        // Act
        user.UpdateProfile(newFirstName, newLastName);

        // Assert
        user.FirstName.Should().Be(newFirstName);
        user.LastName.Should().Be(newLastName);
        user.UpdatedAt.Should().NotBeNull();
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void UpdateProfile_WithOptionalLastName_ShouldUpdateOnlyFirstName()
    {
        // Arrange
        var user = User.Create("johndoe", "John");
        var newFirstName = "Jane";

        // Act
        user.UpdateProfile(newFirstName, null);

        // Assert
        user.FirstName.Should().Be(newFirstName);
        user.LastName.Should().BeNull();
    }

    [Fact]
    public void Activate_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var user = User.Create("johndoe", "John");
        user.Deactivate();

        // Act
        user.Activate();

        // Assert
        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var user = User.Create("johndoe", "John");

        // Act
        user.Deactivate();

        // Assert
        user.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Delete_ShouldMarkAsDeleted()
    {
        // Arrange
        var user = User.Create("johndoe", "John");

        // Act
        user.Delete();

        // Assert
        user.IsDeleted.Should().BeTrue();
        user.DeletedAt.Should().NotBeNull();
        user.DeletedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }
}
