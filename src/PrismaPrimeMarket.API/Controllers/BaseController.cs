using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PrismaPrimeMarket.API.Controllers;

/// <summary>
/// Controller base com suporte a CQRS via MediatR
/// Pode ser herdado para implementar controllers específicos
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator Mediator = mediator;
}
