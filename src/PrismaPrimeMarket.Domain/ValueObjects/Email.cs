using System.Text.RegularExpressions;
using PrismaPrimeMarket.Domain.Common;
using PrismaPrimeMarket.Domain.Exceptions;

namespace PrismaPrimeMarket.Domain.ValueObjects;

/// <summary>
/// Value Object que representa um endereço de e-mail válido
/// </summary>
public class Email : ValueObject
{
    public string Address { get; private set; } = string.Empty;

    private Email() { } // EF Core

    private Email(string address)
    {
        Address = address;
    }

    /// <summary>
    /// Cria uma nova instância de Email
    /// </summary>
    /// <param name="address">Endereço de e-mail</param>
    /// <returns>Email válido</returns>
    /// <exception cref="DomainException">Lançada quando o e-mail é inválido</exception>
    public static Email Create(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new DomainException("O e-mail não pode ser vazio");

        address = address.Trim().ToLowerInvariant();

        if (!IsValidEmail(address))
            throw new DomainException("O e-mail fornecido não é válido");

        if (address.Length > 254)
            throw new DomainException("O e-mail não pode ter mais de 254 caracteres");

        return new Email(address);
    }

    private static bool IsValidEmail(string email)
    {
        // Regex básico para validação de e-mail
        const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, EmailPattern, RegexOptions.IgnoreCase);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Address;
    }

    public override string ToString() => Address;

    public static implicit operator string(Email email) => email.Address;
}
