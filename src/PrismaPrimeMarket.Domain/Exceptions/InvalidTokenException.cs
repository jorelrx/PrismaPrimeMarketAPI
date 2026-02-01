namespace PrismaPrimeMarket.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando um token é inválido ou expirado
/// </summary>
public class InvalidTokenException : DomainException
{
    public InvalidTokenException()
        : base("Token inválido ou expirado")
    {
    }

    public InvalidTokenException(string message)
        : base(message)
    {
    }
}
