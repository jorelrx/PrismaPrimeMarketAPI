using PrismaPrimeMarket.Domain.Common;

namespace PrismaPrimeMarket.Domain.Events;

/// <summary>
/// Evento de domínio disparado quando um novo usuário é registrado no sistema
/// </summary>
public class UserRegisteredEvent : DomainEvent
{
    public Guid UserId { get; private set; }
    public string Email { get; private set; }
    public string UserName { get; private set; }

    public UserRegisteredEvent(Guid userId, string email, string userName)
    {
        UserId = userId;
        Email = email;
        UserName = userName;
    }
}
