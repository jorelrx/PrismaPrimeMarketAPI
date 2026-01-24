using PrismaPrimeMarket.Domain.Common;

namespace PrismaPrimeMarket.Domain.Events;

/// <summary>
/// Evento de domínio disparado quando um usuário ativa sua conta
/// (geralmente após confirmação de e-mail)
/// </summary>
public class UserActivatedEvent : DomainEvent
{
    public Guid UserId { get; private set; }
    public string Email { get; private set; }

    public UserActivatedEvent(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
    }
}
