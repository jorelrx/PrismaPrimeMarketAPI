using Microsoft.AspNetCore.Mvc;
using PrismaPrimeMarket.Application.Interfaces;
using PrismaPrimeMarket.Domain.Common;
using PrismaPrimeMarket.Domain.Interfaces;

namespace PrismaPrimeMarket.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseController<TEntity, TDto>(IBaseService<TEntity, TDto> service) : ControllerBase
    where TEntity : BaseEntity, IAggregateRoot
    where TDto : class
{
    protected readonly IBaseService<TEntity, TDto> _service = service;

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    public virtual async Task<ActionResult<TDto>> Create([FromBody] TDto dto, CancellationToken cancellationToken)
    {
        var result = await _service.AddAsync(dto, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] TDto dto, CancellationToken cancellationToken)
    {
        await _service.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
