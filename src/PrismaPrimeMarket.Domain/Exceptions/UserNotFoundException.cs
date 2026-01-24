namespace PrismaPrimeMarket.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando um usuário não é encontrado
/// </summary>
public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(Guid userId) 
        : base($"Usuário com ID '{userId}' não foi encontrado")
    {
    }

    public UserNotFoundException(string identifier, string identifierType = "identificador") 
        : base($"Usuário com {identifierType} '{identifier}' não foi encontrado")
    {
    }
}
