using FluentValidation;

namespace PrismaPrimeMarket.Application.UseCases.Auth.Commands.Login;

/// <summary>
/// Validador para o comando de login
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty().WithMessage("O email ou nome de usuário é obrigatório")
            .MinimumLength(3).WithMessage("O email ou nome de usuário deve ter no mínimo 3 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória")
            .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres")
            .Matches(@"[A-Z]").WithMessage("A senha deve conter pelo menos uma letra maiúscula")
            .Matches(@"[a-z]").WithMessage("A senha deve conter pelo menos uma letra minúscula")
            .Matches(@"[0-9]").WithMessage("A senha deve conter pelo menos um número")
            .Matches(@"[\W_]").WithMessage("A senha deve conter pelo menos um caractere especial");
    }
}
