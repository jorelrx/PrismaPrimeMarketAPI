using FluentAssertions;
using PrismaPrimeMarket.Domain.Exceptions;
using PrismaPrimeMarket.Domain.ValueObjects;

namespace PrismaPrimeMarket.UnitTests.Domain.ValueObjects;

public class PhoneNumberTests
{
    [Theory]
    [InlineData("11987654321")]
    [InlineData("(11) 98765-4321")]
    [InlineData("11 98765-4321")]
    public void Create_WithValidPhone_ShouldCreatePhoneNumber(string phone)
    {
        // Act
        var result = PhoneNumber.Create(phone);

        // Assert
        result.Should().NotBeNull();
        result.Number.Should().HaveLength(11);
        result.CountryCode.Should().Be("+55");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("123")]
    [InlineData("123456789012")] // Too long
    public void Create_WithInvalidPhone_ShouldThrowDomainException(string invalidPhone)
    {
        // Act
        Action act = () => PhoneNumber.Create(invalidPhone);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*telefone*");
    }

    [Fact]
    public void GetFormatted_ShouldReturnFormattedPhone()
    {
        // Arrange
        var phone = PhoneNumber.Create("11987654321");

        // Act
        var formatted = phone.GetFormatted();

        // Assert
        formatted.Should().Be("(11) 98765-4321");
    }

    [Fact]
    public void TwoPhones_WithSameNumber_ShouldBeEqual()
    {
        // Arrange
        var phone1 = PhoneNumber.Create("11987654321");
        var phone2 = PhoneNumber.Create("(11) 98765-4321");

        // Act & Assert
        phone1.Should().Be(phone2);
        (phone1 == phone2).Should().BeTrue();
    }
}
