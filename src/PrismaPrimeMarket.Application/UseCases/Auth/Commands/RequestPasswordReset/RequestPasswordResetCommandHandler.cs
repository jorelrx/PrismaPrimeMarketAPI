using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Interfaces;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;

namespace PrismaPrimeMarket.Application.UseCases.Auth.Commands.RequestPasswordReset;

/// <summary>
/// Handler para solicitar reset de senha
/// </summary>
public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, Unit>
{
    private readonly UserManager<User> _userManager;
    private readonly IPasswordResetRepository _passwordResetRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RequestPasswordResetCommandHandler> _logger;

    public RequestPasswordResetCommandHandler(
        UserManager<User> userManager,
        IPasswordResetRepository passwordResetRepository,
        IUnitOfWork unitOfWork,
        ILogger<RequestPasswordResetCommandHandler> logger)
    {
        _userManager = userManager;
        _passwordResetRepository = passwordResetRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Unit> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        // Por segurança, sempre retorna sucesso mesmo que o usuário não exista
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            _logger.LogWarning("Tentativa de reset de senha para email não cadastrado.");
            return Unit.Value;
        }

        // Gera o token de reset
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        // Cria a entidade de reset de senha
        var passwordReset = PasswordReset.Create(resetToken, user.Id, TimeSpan.FromHours(1));
        await _passwordResetRepository.AddAsync(passwordReset, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        // TODO: Enviar email com o token de reset
        // Por enquanto, apenas loga o evento (em produção, isso deve ser enviado por email)
        _logger.LogInformation(
            "Token de reset de senha gerado para usuário {UserId}",
            user.Id);

        return Unit.Value;
    }
}
