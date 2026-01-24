using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeMarket.Application.DTOs.User;
using PrismaPrimeMarket.Application.UseCases.Users.Commands.RegisterUser;
using PrismaPrimeMarket.Application.UseCases.Users.Commands.UpdateUserProfile;
using PrismaPrimeMarket.Application.UseCases.Users.Queries.GetUserById;
using PrismaPrimeMarket.Application.UseCases.Users.Queries.GetUsers;

namespace PrismaPrimeMarket.API.Controllers.V1;

/// <summary>
/// Controller para gerenciamento de usuários
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : BaseController
{
    private readonly IMapper _mapper;

    public UsersController(IMediator mediator, IMapper mapper) : base(mediator)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Registra um novo usuário
    /// </summary>
    /// <param name="dto">Dados do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Usuário criado</returns>
    /// <response code="201">Usuário criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="409">Usuário já existe</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] CreateUserDto dto,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterUserCommand>(dto);

        var result = await Mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result
        );
    }

    /// <summary>
    /// Obtém um usuário por ID
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do usuário</returns>
    /// <response code="200">Usuário encontrado</response>
    /// <response code="404">Usuário não encontrado</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Obtém lista de usuários ativos
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de usuários</returns>
    /// <response code="200">Lista de usuários</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery();
        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Atualiza o perfil de um usuário
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <param name="dto">Dados para atualização</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Usuário atualizado</returns>
    /// <response code="200">Perfil atualizado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="404">Usuário não encontrado</response>
    [HttpPut("{id:guid}/profile")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfile(
        Guid id,
        [FromBody] UpdateUserProfileDto dto,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateUserProfileCommand>(dto);
        command = command with { UserId = id }; // Define o UserId da rota

        var result = await Mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}
