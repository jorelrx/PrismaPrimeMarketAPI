using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeMarket.Application.DTOs.Auth;
using PrismaPrimeMarket.Application.UseCases.Auth.Commands.ConfirmPasswordReset;
using PrismaPrimeMarket.Application.UseCases.Auth.Commands.Login;
using PrismaPrimeMarket.Application.UseCases.Auth.Commands.RefreshToken;
using PrismaPrimeMarket.Application.UseCases.Auth.Commands.RequestPasswordReset;

namespace PrismaPrimeMarket.API.Controllers.V1;

/// <summary>
/// Controller para endpoints de autenticação JWT
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class AuthController : BaseController
{
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Autentica um usuário e retorna tokens JWT
    /// </summary>
    /// <param name="command">Credenciais de login</param>
    /// <returns>Dados do usuário e tokens de autenticação</returns>
    /// <response code="200">Login realizado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="401">Credenciais inválidas</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Renova o access token usando um refresh token válido
    /// </summary>
    /// <param name="command">Refresh token</param>
    /// <returns>Novos tokens de autenticação</returns>
    /// <response code="200">Token renovado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="401">Refresh token inválido ou expirado</response>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthTokensDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Solicita reset de senha enviando email com token
    /// </summary>
    /// <param name="command">Email do usuário</param>
    /// <returns>Confirmação de solicitação</returns>
    /// <response code="200">Solicitação processada (sempre retorna 200 por segurança)</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost("password/reset-request")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetCommand command)
    {
        await Mediator.Send(command);
        return Ok(new { message = "Se o email existe, você receberá instruções para resetar sua senha" });
    }

    /// <summary>
    /// Confirma reset de senha usando token
    /// </summary>
    /// <param name="command">Token e nova senha</param>
    /// <returns>Confirmação de reset</returns>
    /// <response code="200">Senha resetada com sucesso</response>
    /// <response code="400">Token inválido ou expirado</response>
    [HttpPost("password/reset-confirm")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmPasswordReset([FromBody] ConfirmPasswordResetCommand command)
    {
        await Mediator.Send(command);
        return Ok(new { message = "Senha resetada com sucesso" });
    }
}
