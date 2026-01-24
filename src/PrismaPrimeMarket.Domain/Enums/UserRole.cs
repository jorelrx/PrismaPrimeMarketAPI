namespace PrismaPrimeMarket.Domain.Enums;

/// <summary>
/// Define os tipos de papéis (roles) que um usuário pode ter no sistema
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Cliente - Pode comprar produtos
    /// </summary>
    Customer = 1,

    /// <summary>
    /// Vendedor - Pode vender produtos
    /// </summary>
    Seller = 2,

    /// <summary>
    /// Administrador - Acesso total ao sistema
    /// </summary>
    Admin = 3
}
