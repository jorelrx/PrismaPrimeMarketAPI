using MediatR;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeMarket.Application.Common.Models;
using PrismaPrimeMarket.Application.UseCases.Common.Commands.Create;
using PrismaPrimeMarket.Application.UseCases.Common.Commands.Delete;
using PrismaPrimeMarket.Application.UseCases.Common.Commands.Update;
using PrismaPrimeMarket.Application.UseCases.Common.Queries.GetById;
using PrismaPrimeMarket.Application.UseCases.Common.Queries.GetList;
using PrismaPrimeMarket.Domain.Common;

namespace PrismaPrimeMarket.API.Controllers;

/// <summary>
/// Controller base com suporte a CQRS via MediatR
/// Fornece métodos CRUD genéricos usando Commands e Queries padrões
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator Mediator = mediator;

    /// <summary>
    /// Cria uma nova entidade usando CreateCommand genérico
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade de domínio</typeparam>
    /// <typeparam name="TDto">Tipo do DTO de retorno</typeparam>
    /// <param name="entity">Entidade a ser criada</param>
    /// <returns>Response com o DTO criado</returns>
    protected async Task<ActionResult<Response<TDto>>> CreateAsync<TEntity, TDto>(TEntity entity)
        where TEntity : BaseEntity
    {
        var command = new CreateCommand<TEntity>(entity);
        var result = await Mediator.Send(command);
        return CreatedAtAction(null, result);
    }

    /// <summary>
    /// Atualiza uma entidade existente usando UpdateCommand genérico
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade de domínio</typeparam>
    /// <typeparam name="TDto">Tipo do DTO de retorno</typeparam>
    /// <param name="id">ID da entidade</param>
    /// <param name="entity">Entidade com dados atualizados</param>
    /// <returns>Response com o DTO atualizado</returns>
    protected async Task<ActionResult<Response<TDto>>> UpdateAsync<TEntity, TDto>(Guid id, TEntity entity)
        where TEntity : BaseEntity
    {
        var command = new UpdateCommand<TEntity>(id, entity);
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Deleta uma entidade usando DeleteCommand genérico
    /// </summary>
    /// <param name="id">ID da entidade a ser deletada</param>
    /// <returns>Response vazio indicando sucesso</returns>
    protected async Task<ActionResult<Response<object>>> DeleteAsync(Guid id)
    {
        var command = new DeleteCommand(id);
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Obtém uma entidade por ID usando GetByIdQuery genérico
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade de domínio</typeparam>
    /// <typeparam name="TDto">Tipo do DTO de retorno</typeparam>
    /// <param name="id">ID da entidade</param>
    /// <returns>Response com o DTO encontrado</returns>
    protected async Task<ActionResult<Response<TDto>>> GetByIdAsync<TEntity, TDto>(Guid id)
        where TEntity : BaseEntity
    {
        var query = new GetByIdQuery<TEntity>(id);
        var result = await Mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Obtém uma lista paginada de entidades usando GetListQuery genérico
    /// </summary>
    /// <typeparam name="TDto">Tipo do DTO de retorno</typeparam>
    /// <param name="pageNumber">Número da página (padrão: 1)</param>
    /// <param name="pageSize">Tamanho da página (padrão: 10)</param>
    /// <returns>PagedResponse com lista de DTOs</returns>
    protected async Task<ActionResult<PagedResponse<IEnumerable<TDto>>>> GetListAsync<TDto>(
        int pageNumber = 1,
        int pageSize = 10)
        where TDto : class
    {
        var query = new GetListQuery<TDto>(new PaginationFilter { PageNumber = pageNumber, PageSize = pageSize });
        var result = await Mediator.Send(query);
        return Ok(result);
    }
}
