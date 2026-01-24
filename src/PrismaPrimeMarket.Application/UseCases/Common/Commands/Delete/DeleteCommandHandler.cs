using PrismaPrimeMarket.Application.Common.Messaging;
using PrismaPrimeMarket.Application.Common.Models;
using PrismaPrimeMarket.Domain.Common;
using PrismaPrimeMarket.Domain.Interfaces;

namespace PrismaPrimeMarket.Application.UseCases.Common.Commands.Delete;

/// <summary>
/// Handler genérico para excluir entidade
/// </summary>
public class DeleteCommandHandler<TEntity>(
    IBaseRepository<TEntity> repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteCommand, Response<object>>
    where TEntity : BaseEntity, IAggregateRoot
{
    private readonly IBaseRepository<TEntity> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Response<object>> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsAsync(request.Id, cancellationToken);

        if (!exists)
            return Response<object>.NotFound($"Recurso com ID {request.Id} não encontrado");

        await _repository.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Response<object>.Deleted();
    }
}
