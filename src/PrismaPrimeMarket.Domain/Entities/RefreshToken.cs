using PrismaPrimeMarket.Domain.Common;

namespace PrismaPrimeMarket.Domain.Entities;

/// <summary>
/// Entidade que representa um refresh token para renovação de JWT
/// </summary>
public class RefreshToken : BaseEntity, IAggregateRoot
{
    public string Token { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string? ReplacedByToken { get; private set; }

    // Construtor privado para EF Core
    private RefreshToken()
    {
        Token = string.Empty;
        User = null!;
    }

    private RefreshToken(string token, Guid userId, DateTime expiresAt)
    {
        Id = Guid.NewGuid();
        Token = token;
        UserId = userId;
        ExpiresAt = expiresAt;
        IsRevoked = false;
    }

    /// <summary>
    /// Factory method para criar um novo refresh token
    /// </summary>
    public static RefreshToken Create(string token, Guid userId, DateTime expiresAt)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("O token não pode ser vazio", nameof(token));

        if (userId == Guid.Empty)
            throw new ArgumentException("O userId não pode ser vazio", nameof(userId));

        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException("A data de expiração deve ser futura", nameof(expiresAt));

        return new RefreshToken(token, userId, expiresAt);
    }

    /// <summary>
    /// Revoga o refresh token
    /// </summary>
    public void Revoke(string? replacedByToken = null)
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        ReplacedByToken = replacedByToken;
    }

    /// <summary>
    /// Verifica se o token está ativo (não expirado e não revogado)
    /// </summary>
    public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiresAt;

    /// <summary>
    /// Verifica se o token expirou
    /// </summary>
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
}
