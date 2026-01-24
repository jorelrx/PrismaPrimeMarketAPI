namespace PrismaPrimeMarket.Application.Common.Models;

/// <summary>
/// Mensagens padrão para cada tipo de resposta
/// </summary>
public static class ResponseMessages
{
    // Success messages
    public const string Success = "Operação realizada com sucesso";
    public const string Created = "Recurso criado com sucesso";
    public const string Updated = "Recurso atualizado com sucesso";
    public const string Deleted = "Recurso excluído com sucesso";
    public const string Retrieved = "Recurso recuperado com sucesso";
    public const string ListRetrieved = "Lista recuperada com sucesso";
    
    // Error messages
    public const string NotFound = "Recurso não encontrado";
    public const string ValidationError = "Erro de validação nos dados fornecidos";
    public const string BadRequest = "Requisição inválida";
    public const string Unauthorized = "Não autorizado para acessar este recurso";
    public const string Forbidden = "Acesso negado a este recurso";
    public const string Conflict = "Conflito detectado ao processar a requisição";
    public const string InternalServerError = "Erro interno no servidor";
    
    /// <summary>
    /// Obtém a mensagem padrão para um tipo de resposta
    /// </summary>
    public static string GetMessage(ResponseType type)
    {
        return type switch
        {
            ResponseType.Success => Success,
            ResponseType.Created => Created,
            ResponseType.Updated => Updated,
            ResponseType.Deleted => Deleted,
            ResponseType.Retrieved => Retrieved,
            ResponseType.NotFound => NotFound,
            ResponseType.ValidationError => ValidationError,
            ResponseType.BadRequest => BadRequest,
            ResponseType.Unauthorized => Unauthorized,
            ResponseType.Forbidden => Forbidden,
            ResponseType.Conflict => Conflict,
            ResponseType.InternalServerError => InternalServerError,
            _ => Success
        };
    }
}
