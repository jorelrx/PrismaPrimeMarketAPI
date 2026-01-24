namespace PrismaPrimeMarket.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando já existe um usuário com o mesmo e-mail ou username
/// </summary>
public class UserAlreadyExistsException : DomainException
{
    public UserAlreadyExistsException(string identifier, string identifierType = "identificador")
        : base($"Já existe um usuário cadastrado com o {identifierType} '{identifier}'")
    {
    }
}
