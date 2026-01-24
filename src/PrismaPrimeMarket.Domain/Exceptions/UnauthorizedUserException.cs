namespace PrismaPrimeMarket.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando um usuário tenta realizar uma ação sem autorização adequada
/// </summary>
public class UnauthorizedUserException : DomainException
{
    public UnauthorizedUserException(string message = "Usuário não autorizado a realizar esta operação")
        : base(message)
    {
    }
}
