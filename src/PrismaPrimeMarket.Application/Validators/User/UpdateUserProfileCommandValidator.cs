using FluentValidation;
using PrismaPrimeMarket.Application.UseCases.Users.Commands.UpdateUserProfile;

namespace PrismaPrimeMarket.Application.Validators.User;

/// <summary>
/// Validator para UpdateUserProfileCommand
/// </summary>
public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("O ID do usuário é obrigatório");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("O primeiro nome é obrigatório")
            .MinimumLength(2).WithMessage("O primeiro nome deve ter no mínimo 2 caracteres")
            .MaximumLength(100).WithMessage("O primeiro nome não pode ter mais de 100 caracteres");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("O sobrenome é obrigatório")
            .MinimumLength(2).WithMessage("O sobrenome deve ter no mínimo 2 caracteres")
            .MaximumLength(100).WithMessage("O sobrenome não pode ter mais de 100 caracteres");

        When(x => !string.IsNullOrEmpty(x.CPF), () =>
        {
            RuleFor(x => x.CPF)
                .Must(BeValidCpf!).WithMessage("O CPF informado não é válido");
        });

        When(x => !string.IsNullOrEmpty(x.PhoneNumber), () =>
        {
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\(?\d{2}\)?[\s-]?[\s9]?\d{4}-?\d{4}$")
                .WithMessage("O número de telefone não é válido");
        });

        When(x => x.BirthDate.HasValue, () =>
        {
            RuleFor(x => x.BirthDate)
                .LessThan(DateTime.UtcNow).WithMessage("A data de nascimento não pode ser futura")
                .Must(BeAtLeast13YearsOld!).WithMessage("O usuário deve ter no mínimo 13 anos");
        });
    }

    private bool BeValidCpf(string cpf)
    {
        try
        {
            var cleanedCpf = System.Text.RegularExpressions.Regex.Replace(cpf, @"[^\d]", "");

            if (cleanedCpf.Length != 11)
                return false;

            if (cleanedCpf.All(c => c == cleanedCpf[0]))
                return false;

            var sum = 0;
            for (int i = 0; i < 9; i++)
                sum += int.Parse(cleanedCpf[i].ToString()) * (10 - i);

            var remainder = sum % 11;
            var digit1 = remainder < 2 ? 0 : 11 - remainder;

            if (int.Parse(cleanedCpf[9].ToString()) != digit1)
                return false;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(cleanedCpf[i].ToString()) * (11 - i);

            remainder = sum % 11;
            var digit2 = remainder < 2 ? 0 : 11 - remainder;

            return int.Parse(cleanedCpf[10].ToString()) == digit2;
        }
        catch
        {
            return false;
        }
    }

    private bool BeAtLeast13YearsOld(DateTime? birthDate)
    {
        if (!birthDate.HasValue)
            return true;

        var age = DateTime.UtcNow.Year - birthDate.Value.Year;
        if (birthDate.Value > DateTime.UtcNow.AddYears(-age))
            age--;

        return age >= 13;
    }
}
