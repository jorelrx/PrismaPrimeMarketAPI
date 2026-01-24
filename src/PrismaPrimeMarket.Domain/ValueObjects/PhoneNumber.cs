using System.Text.RegularExpressions;
using PrismaPrimeMarket.Domain.Common;
using PrismaPrimeMarket.Domain.Exceptions;

namespace PrismaPrimeMarket.Domain.ValueObjects;

/// <summary>
/// Value Object que representa um número de telefone brasileiro válido
/// </summary>
public class PhoneNumber : ValueObject
{
    public string Number { get; private set; } = string.Empty;
    public string CountryCode { get; private set; } = string.Empty;

    private PhoneNumber() { } // EF Core

    private PhoneNumber(string number, string countryCode = "+55")
    {
        Number = number;
        CountryCode = countryCode;
    }

    /// <summary>
    /// Cria uma nova instância de PhoneNumber
    /// </summary>
    /// <param name="number">Número do telefone (pode conter parênteses, traços e espaços)</param>
    /// <param name="countryCode">Código do país (padrão: +55 para Brasil)</param>
    /// <returns>PhoneNumber válido</returns>
    /// <exception cref="DomainException">Lançada quando o número é inválido</exception>
    public static PhoneNumber Create(string number, string countryCode = "+55")
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new DomainException("O número de telefone não pode ser vazio");

        if (string.IsNullOrWhiteSpace(countryCode))
            throw new DomainException("O código do país não pode ser vazio");

        // Remove caracteres não numéricos
        var cleanedNumber = Regex.Replace(number, @"[^\d]", "");

        // Valida número brasileiro (com ou sem nono dígito)
        if (countryCode == "+55")
        {
            if (!IsValidBrazilianPhone(cleanedNumber))
                throw new DomainException("O número de telefone brasileiro deve ter 10 ou 11 dígitos");
        }
        else
        {
            // Validação genérica para outros países
            if (cleanedNumber.Length < 8 || cleanedNumber.Length > 15)
                throw new DomainException("O número de telefone deve ter entre 8 e 15 dígitos");
        }

        return new PhoneNumber(cleanedNumber, countryCode);
    }

    /// <summary>
    /// Valida número de telefone brasileiro
    /// </summary>
    private static bool IsValidBrazilianPhone(string number)
    {
        // Deve ter 10 (fixo) ou 11 (celular) dígitos
        if (number.Length != 10 && number.Length != 11)
            return false;

        // DDD válido (11 a 99)
        var ddd = int.Parse(number.Substring(0, 2));
        if (ddd < 11 || ddd > 99)
            return false;

        // Se tiver 11 dígitos, o terceiro deve ser 9 (celular)
        if (number.Length == 11 && number[2] != '9')
            return false;

        return true;
    }

    /// <summary>
    /// Retorna o número formatado no padrão brasileiro: (00) 00000-0000 ou (00) 0000-0000
    /// </summary>
    public string GetFormatted()
    {
        if (CountryCode == "+55")
        {
            if (Number.Length == 11)
                return $"({Number.Substring(0, 2)}) {Number.Substring(2, 5)}-{Number.Substring(7, 4)}";
            else if (Number.Length == 10)
                return $"({Number.Substring(0, 2)}) {Number.Substring(2, 4)}-{Number.Substring(6, 4)}";
        }

        return $"{CountryCode} {Number}";
    }

    /// <summary>
    /// Retorna o número completo com código do país
    /// </summary>
    public string GetFullNumber()
    {
        return $"{CountryCode}{Number}";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
        yield return CountryCode;
    }

    public override string ToString() => GetFormatted();

    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Number;
}
