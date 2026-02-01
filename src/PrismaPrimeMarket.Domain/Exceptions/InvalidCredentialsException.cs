namespace PrismaPrimeMarket.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando as credenciais de login são inválidas
/// </summary>
public class InvalidCredentialsException : DomainException
{
    public InvalidCredentialsException()
        : base("Email ou senha inválidos")
    {
    }

    public InvalidCredentialsException(string message)
        : base(message)
    {
    }
}
