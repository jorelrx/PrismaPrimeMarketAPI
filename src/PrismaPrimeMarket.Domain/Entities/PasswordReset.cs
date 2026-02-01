using PrismaPrimeMarket.Domain.Common;

namespace PrismaPrimeMarket.Domain.Entities;

/// <summary>
/// Entidade que representa uma solicitação de reset de senha
/// </summary>
public class PasswordReset : BaseEntity, IAggregateRoot
{
    public string Token { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }
    public DateTime? UsedAt { get; private set; }

    // Construtor privado para EF Core
    private PasswordReset()
    {
        Token = string.Empty;
        User = null!;
    }

    private PasswordReset(string token, Guid userId, DateTime expiresAt)
    {
        Id = Guid.NewGuid();
        Token = token;
        UserId = userId;
        ExpiresAt = expiresAt;
        IsUsed = false;
    }

    /// <summary>
    /// Factory method para criar uma nova solicitação de reset de senha
    /// </summary>
    public static PasswordReset Create(string token, Guid userId, TimeSpan validityPeriod)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("O token não pode ser vazio", nameof(token));

        if (userId == Guid.Empty)
            throw new ArgumentException("O userId não pode ser vazio", nameof(userId));

        if (validityPeriod <= TimeSpan.Zero)
            throw new ArgumentException("O período de validade deve ser maior que zero", nameof(validityPeriod));

        var expiresAt = DateTime.UtcNow.Add(validityPeriod);
        return new PasswordReset(token, userId, expiresAt);
    }

    /// <summary>
    /// Marca o token como usado
    /// </summary>
    public void MarkAsUsed()
    {
        if (IsUsed)
            throw new InvalidOperationException("Este token já foi usado");

        if (IsExpired)
            throw new InvalidOperationException("Este token expirou");

        IsUsed = true;
        UsedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Verifica se o token está válido (não usado e não expirado)
    /// </summary>
    public bool IsValid => !IsUsed && !IsExpired;

    /// <summary>
    /// Verifica se o token expirou
    /// </summary>
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
}
