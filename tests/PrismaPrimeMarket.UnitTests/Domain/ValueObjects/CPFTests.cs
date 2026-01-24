using FluentAssertions;
using PrismaPrimeMarket.Domain.Exceptions;
using PrismaPrimeMarket.Domain.ValueObjects;

namespace PrismaPrimeMarket.UnitTests.Domain.ValueObjects;

public class CPFTests
{
    [Theory]
    [InlineData("12345678909")]
    [InlineData("123.456.789-09")]
    [InlineData("111.444.777-35")]
    public void Create_WithValidCPF_ShouldCreateCPF(string cpf)
    {
        // Act
        var result = CPF.Create(cpf);

        // Assert
        result.Should().NotBeNull();
        result.Number.Should().HaveLength(11);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("123")]
    [InlineData("12345678900")] // Invalid CPF
    [InlineData("11111111111")] // All same digits
    public void Create_WithInvalidCPF_ShouldThrowDomainException(string invalidCpf)
    {
        // Act
        Action act = () => CPF.Create(invalidCpf);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*CPF*");
    }

    [Fact]
    public void GetFormatted_ShouldReturnFormattedCPF()
    {
        // Arrange
        var cpf = CPF.Create("12345678909");

        // Act
        var formatted = cpf.GetFormatted();

        // Assert
        formatted.Should().Be("123.456.789-09");
    }

    [Fact]
    public void TwoCPFs_WithSameNumber_ShouldBeEqual()
    {
        // Arrange
        var cpf1 = CPF.Create("12345678909");
        var cpf2 = CPF.Create("123.456.789-09");

        // Act & Assert
        cpf1.Should().Be(cpf2);
        (cpf1 == cpf2).Should().BeTrue();
    }
}
