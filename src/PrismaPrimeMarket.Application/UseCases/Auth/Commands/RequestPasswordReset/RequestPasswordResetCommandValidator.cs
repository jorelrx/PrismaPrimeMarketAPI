using FluentValidation;

namespace PrismaPrimeMarket.Application.UseCases.Auth.Commands.RequestPasswordReset;

/// <summary>
/// Validador para o comando de solicitação de reset de senha
/// </summary>
public class RequestPasswordResetCommandValidator : AbstractValidator<RequestPasswordResetCommand>
{
    public RequestPasswordResetCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório")
            .EmailAddress().WithMessage("O email deve ser válido");
    }
}
