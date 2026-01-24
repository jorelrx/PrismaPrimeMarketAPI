using System.Text.RegularExpressions;
using PrismaPrimeMarket.Domain.Common;
using PrismaPrimeMarket.Domain.Exceptions;

namespace PrismaPrimeMarket.Domain.ValueObjects;

/// <summary>
/// Value Object que representa um CPF (Cadastro de Pessoa Física) válido
/// </summary>
public class CPF : ValueObject
{
    public string Number { get; private set; } = string.Empty;

    private CPF() { } // EF Core

    private CPF(string number)
    {
        Number = number;
    }

    /// <summary>
    /// Cria uma nova instância de CPF
    /// </summary>
    /// <param name="number">Número do CPF (pode conter pontos e traços)</param>
    /// <returns>CPF válido</returns>
    /// <exception cref="DomainException">Lançada quando o CPF é inválido</exception>
    public static CPF Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new DomainException("O CPF não pode ser vazio");

        // Remove caracteres não numéricos
        var cleanedNumber = Regex.Replace(number, @"[^\d]", "");

        if (cleanedNumber.Length != 11)
            throw new DomainException("O CPF deve conter 11 dígitos");

        if (!IsValidCPF(cleanedNumber))
            throw new DomainException("O CPF fornecido não é válido");

        // Armazena sem formatação
        return new CPF(cleanedNumber);
    }

    /// <summary>
    /// Valida um CPF usando o algoritmo de validação oficial
    /// </summary>
    private static bool IsValidCPF(string cpf)
    {
        // CPFs com todos os dígitos iguais são inválidos
        if (cpf.All(c => c == cpf[0]))
            return false;

        // Validação do primeiro dígito verificador
        var sum = 0;
        for (int i = 0; i < 9; i++)
            sum += int.Parse(cpf[i].ToString()) * (10 - i);
        
        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;

        if (int.Parse(cpf[9].ToString()) != digit1)
            return false;

        // Validação do segundo dígito verificador
        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += int.Parse(cpf[i].ToString()) * (11 - i);
        
        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;

        return int.Parse(cpf[10].ToString()) == digit2;
    }

    /// <summary>
    /// Retorna o CPF formatado (000.000.000-00)
    /// </summary>
    public string GetFormatted()
    {
        return $"{Number.Substring(0, 3)}.{Number.Substring(3, 3)}.{Number.Substring(6, 3)}-{Number.Substring(9, 2)}";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }

    public override string ToString() => GetFormatted();

    public static implicit operator string(CPF cpf) => cpf.Number;
}
