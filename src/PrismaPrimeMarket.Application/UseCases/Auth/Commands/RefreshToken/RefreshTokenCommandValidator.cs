using FluentValidation;

namespace PrismaPrimeMarket.Application.UseCases.Auth.Commands.RefreshToken;

/// <summary>
/// Validador para o comando de refresh token
/// </summary>
public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("O refresh token é obrigatório");
    }
}
