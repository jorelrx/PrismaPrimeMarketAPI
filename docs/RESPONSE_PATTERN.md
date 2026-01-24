# Response Pattern - Guia de Uso

## Visão Geral

O sistema de Response foi refatorado para fornecer um padrão consistente em todas as respostas da API, incluindo mensagens automáticas, tipos de operação e informações da requisição.

## Estrutura das Classes

### ResponseType (Enum)

Define os tipos de resposta disponíveis:

**Success Types:**
- `Success` - Operação genérica bem-sucedida
- `Created` - Recurso criado
- `Updated` - Recurso atualizado
- `Deleted` - Recurso excluído
- `Retrieved` - Recurso recuperado

**Error Types:**
- `NotFound` - Recurso não encontrado
- `ValidationError` - Erro de validação
- `BadRequest` - Requisição inválida
- `Unauthorized` - Não autorizado
- `Forbidden` - Acesso negado
- `Conflict` - Conflito
- `InternalServerError` - Erro interno

### ResponseMessages (Classe Estática)

Contém mensagens padrão em português para cada tipo de resposta. Método `GetMessage(ResponseType)` retorna a mensagem apropriada.

### Response&lt;T&gt;

Classe base para todas as respostas da API.

**Propriedades:**
```csharp
public T? Data { get; set; }                // Dados da resposta
public bool Succeeded { get; set; }         // Indica se a operação foi bem-sucedida
public string[]? Errors { get; set; }       // Array de erros (se houver)
public string Message { get; set; }         // Mensagem descritiva
public ResponseType Type { get; set; }      // Tipo da resposta
public DateTime Timestamp { get; set; }     // Data/hora UTC da resposta
public string? Path { get; set; }           // Path da requisição
```

**Factory Methods (Success):**
```csharp
Response<T>.Success(data, customMessage?, path?)
Response<T>.Created(data, customMessage?, path?)
Response<T>.Updated(data, customMessage?, path?)
Response<T>.Deleted(customMessage?, path?)
Response<T>.Retrieved(data, customMessage?, path?)
```

**Factory Methods (Error):**
```csharp
Response<T>.NotFound(customMessage?, path?)
Response<T>.ValidationError(errors[], customMessage?, path?)
Response<T>.BadRequest(customMessage?, errors[]?, path?)
Response<T>.Unauthorized(customMessage?, path?)
Response<T>.Forbidden(customMessage?, path?)
Response<T>.Conflict(customMessage?, errors[]?, path?)
Response<T>.InternalServerError(customMessage?, errors[]?, path?)
```

### PagedResponse&lt;T&gt;

Estende `Response<T>` para incluir informações de paginação.

**Propriedades Adicionais:**
```csharp
public int PageNumber { get; set; }
public int PageSize { get; set; }
public int TotalRecords { get; set; }
public int TotalPages { get; set; }
public bool HasPreviousPage { get; }        // Computed
public bool HasNextPage { get; }            // Computed
```

**Factory Method:**
```csharp
PagedResponse<T>.Create(data, pageNumber, pageSize, totalRecords, customMessage?, path?)
```

## Exemplos de Uso

### No Controller

```csharp
// GET by ID - Success
[HttpGet("{id}")]
public async Task<ActionResult<Response<ProductDto>>> GetById(Guid id)
{
    var data = await _service.GetByIdAsync(id);
    
    if (data == null)
        return NotFound(Response<ProductDto>.NotFound(path: HttpContext.Request.Path));
    
    var result = Response<ProductDto>.Retrieved(data, path: HttpContext.Request.Path);
    return Ok(result);
}

// POST - Create
[HttpPost]
public async Task<ActionResult<Response<ProductDto>>> Create([FromBody] CreateProductDto dto)
{
    var data = await _service.AddAsync(dto);
    var result = Response<ProductDto>.Created(data, path: HttpContext.Request.Path);
    return CreatedAtAction(nameof(GetById), new { id = data.Id }, result);
}

// PUT - Update
[HttpPut("{id}")]
public async Task<ActionResult<Response<ProductDto>>> Update(Guid id, [FromBody] UpdateProductDto dto)
{
    await _service.UpdateAsync(id, dto);
    var data = await _service.GetByIdAsync(id);
    
    if (data == null)
        return NotFound(Response<ProductDto>.NotFound(path: HttpContext.Request.Path));
    
    var result = Response<ProductDto>.Updated(data, path: HttpContext.Request.Path);
    return Ok(result);
}

// DELETE
[HttpDelete("{id}")]
public async Task<ActionResult<Response<object>>> Delete(Guid id)
{
    await _service.DeleteAsync(id);
    var result = Response<object>.Deleted(path: HttpContext.Request.Path);
    return Ok(result);
}

// GET All - Paginated
[HttpGet]
public async Task<ActionResult<PagedResponse<IEnumerable<ProductDto>>>> GetAll([FromQuery] PaginationFilter filter)
{
    var result = await _service.GetAllAsync(filter);
    result.Path = HttpContext.Request.Path;
    return Ok(result);
}
```

### No Service

```csharp
public async Task<PagedResponse<IEnumerable<TDto>>> GetAllAsync(PaginationFilter filter)
{
    // ... lógica de busca e paginação ...
    
    return PagedResponse<IEnumerable<TDto>>.Create(
        dtos, 
        pageNumber, 
        pageSize, 
        totalCount
    );
}
```

### No Middleware de Exceções

```csharp
private async Task HandleExceptionAsync(HttpContext context, Exception exception)
{
    Response<string> responseModel;
    var path = context.Request.Path.Value;

    switch (exception)
    {
        case DomainException:
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            responseModel = Response<string>.BadRequest(exception.Message, path: path);
            break;
        case ValidationException ex:
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            var errors = ex.Errors.SelectMany(x => x.Value).ToArray();
            responseModel = Response<string>.ValidationError(errors, path: path);
            break;
        case NotFoundException:
            response.StatusCode = (int)HttpStatusCode.NotFound;
            responseModel = Response<string>.NotFound(exception.Message, path: path);
            break;
        default:
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            responseModel = Response<string>.InternalServerError(path: path);
            break;
    }
    
    await response.WriteAsync(JsonSerializer.Serialize(responseModel));
}
```

### No ValidationFilter

```csharp
public override void OnActionExecuting(ActionExecutingContext context)
{
    if (!context.ModelState.IsValid)
    {
        var errors = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(v => v.ErrorMessage)
            .ToArray();

        var response = Response<string>.ValidationError(
            errors, 
            path: context.HttpContext.Request.Path.Value
        );

        context.Result = new BadRequestObjectResult(response);
    }
}
```

## Exemplos de Resposta JSON

### Success - GET by ID
```json
{
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "name": "Notebook Dell",
    "price": 3500.00
  },
  "succeeded": true,
  "errors": null,
  "message": "Recurso recuperado com sucesso",
  "type": "Retrieved",
  "timestamp": "2026-01-24T12:00:00Z",
  "path": "/api/v1/products/123e4567-e89b-12d3-a456-426614174000"
}
```

### Success - POST Create
```json
{
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "name": "Notebook Dell",
    "price": 3500.00
  },
  "succeeded": true,
  "errors": null,
  "message": "Recurso criado com sucesso",
  "type": "Created",
  "timestamp": "2026-01-24T12:00:00Z",
  "path": "/api/v1/products"
}
```

### Success - DELETE
```json
{
  "data": null,
  "succeeded": true,
  "errors": null,
  "message": "Recurso excluído com sucesso",
  "type": "Deleted",
  "timestamp": "2026-01-24T12:00:00Z",
  "path": "/api/v1/products/123e4567-e89b-12d3-a456-426614174000"
}
```

### Success - GET All (Paginated)
```json
{
  "data": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "name": "Notebook Dell",
      "price": 3500.00
    }
  ],
  "pageNumber": 1,
  "pageSize": 20,
  "totalRecords": 95,
  "totalPages": 5,
  "hasPreviousPage": false,
  "hasNextPage": true,
  "succeeded": true,
  "errors": null,
  "message": "Lista recuperada com sucesso",
  "type": "Retrieved",
  "timestamp": "2026-01-24T12:00:00Z",
  "path": "/api/v1/products"
}
```

### Error - Not Found
```json
{
  "data": null,
  "succeeded": false,
  "errors": null,
  "message": "Recurso não encontrado",
  "type": "NotFound",
  "timestamp": "2026-01-24T12:00:00Z",
  "path": "/api/v1/products/123e4567-e89b-12d3-a456-426614174000"
}
```

### Error - Validation
```json
{
  "data": null,
  "succeeded": false,
  "errors": [
    "O campo Nome é obrigatório",
    "O preço deve ser maior que zero"
  ],
  "message": "Erro de validação nos dados fornecidos",
  "type": "ValidationError",
  "timestamp": "2026-01-24T12:00:00Z",
  "path": "/api/v1/products"
}
```

## Benefícios

1. **Consistência**: Todas as respostas seguem o mesmo padrão
2. **Mensagens Automáticas**: Mensagens padrão em português já definidas
3. **Rastreabilidade**: Timestamp e path em todas as respostas
4. **Tipagem Forte**: Factory methods evitam erros de construção
5. **Informativo**: Tipo de operação explícito (Created, Updated, etc.)
6. **Fácil Manutenção**: Mensagens centralizadas em um único lugar
7. **Suporte a Paginação**: PagedResponse com metadados completos

## Customização de Mensagens

Embora as mensagens padrão sejam fornecidas, você pode customizá-las quando necessário:

```csharp
var result = Response<ProductDto>.Created(
    product, 
    customMessage: "Produto criado e notificação enviada com sucesso",
    path: HttpContext.Request.Path
);
```

## Migração de Código Existente

Para migrar código existente:

1. **Controllers**: Substituir retornos diretos por factory methods
2. **Services**: Usar `PagedResponse.Create()` ao invés do construtor
3. **Middlewares**: Usar factory methods apropriados para cada tipo de erro
4. **Filters**: Usar `Response<T>.ValidationError()` para erros de validação

---

**Última atualização**: Janeiro 2026  
**Versão**: 1.0
