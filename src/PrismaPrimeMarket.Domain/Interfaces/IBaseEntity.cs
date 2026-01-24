using PrismaPrimeMarket.Domain.Common;

namespace PrismaPrimeMarket.Domain.Interfaces;

/// <summary>
/// Interface que define o contrato para entidades base do domínio
/// </summary>
public interface IBaseEntity
{
    Guid Id { get; }
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    bool IsDeleted { get; }
    IReadOnlyCollection<DomainEvent> DomainEvents { get; }

    void UpdateTimestamp();
    void Delete();
    void AddDomainEvent(DomainEvent domainEvent);
    void RemoveDomainEvent(DomainEvent domainEvent);
    void ClearDomainEvents();
}
