# Guia de Implementação CQRS

## 📋 Visão Geral

O padrão CQRS (Command Query Responsibility Segregation) foi implementado usando MediatR, separando responsabilidades entre:
- **Commands**: Modificam estado (Create, Update, Delete)
- **Queries**: Apenas leem dados (GetById, GetList)

## 🏗️ Estrutura Implementada

```
Application/
├── Common/
│   ├── Messaging/              # Interfaces base CQRS
│   │   ├── ICommand.cs
│   │   ├── ICommandHandler.cs
│   │   ├── IQuery.cs
│   │   └── IQueryHandler.cs
│   └── Behaviors/              # Pipeline behaviors
│       ├── ValidationBehavior.cs
│       └── LoggingBehavior.cs
└── UseCases/
    └── Common/                 # Commands/Queries genéricos reutilizáveis
        ├── Commands/
        │   ├── Create/
        │   │   ├── CreateCommand.cs
        │   │   └── CreateCommandHandler.cs
        │   ├── Update/
        │   │   ├── UpdateCommand.cs
        │   │   └── UpdateCommandHandler.cs
        │   └── Delete/
        │       ├── DeleteCommand.cs
        │       └── DeleteCommandHandler.cs
        └── Queries/
            ├── GetById/
            │   ├── GetByIdQuery.cs
            │   └── GetByIdQueryHandler.cs
            └── GetList/
                ├── GetListQuery.cs
                └── GetListQueryHandler.cs
```

## 🚀 Como Usar

### No Controller

```csharp
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PrismaPrimeMarket.Application.UseCases.Common.Commands.Create;
using PrismaPrimeMarket.Application.UseCases.Common.Queries.GetById;

namespace PrismaPrimeMarket.API.Controllers.V1;

[ApiVersion("1.0")]
public class ProductsController(IMediator mediator) : BaseController(mediator)
{
    /// <summary>
    /// Obtém produto por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetByIdQuery<ProductDto>(id);
        var result = await Mediator.Send(query, cancellationToken);
        
        result.Path = HttpContext.Request.Path;
        
        return result.Succeeded 
            ? Ok(result) 
            : NotFound(result);
    }

    /// <summary>
    /// Cria novo produto
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateCommand<ProductDto>(dto);
        var result = await Mediator.Send(command, cancellationToken);
        
        result.Path = HttpContext.Request.Path;
        
        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
    }

    /// <summary>
    /// Atualiza produto
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateCommand<ProductDto>(id, dto);
        var result = await Mediator.Send(command, cancellationToken);
        
        result.Path = HttpContext.Request.Path;
        
        return result.Succeeded 
            ? Ok(result) 
            : NotFound(result);
    }

    /// <summary>
    /// Exclui produto
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCommand(id);
        var result = await Mediator.Send(command, cancellationToken);
        
        result.Path = HttpContext.Request.Path;
        
        return result.Succeeded 
            ? Ok(result) 
            : NotFound(result);
    }

    /// <summary>
    /// Lista produtos com paginação
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter, CancellationToken cancellationToken)
    {
        var query = new GetListQuery<ProductDto>(filter);
        var result = await Mediator.Send(query, cancellationToken);
        
        result.Path = HttpContext.Request.Path;
        
        return Ok(result);
    }
}
```

## 📝 Criando Commands/Queries Específicos

### Quando Usar Genéricos vs Específicos

**Use Commands/Queries Genéricos quando:**
- Operação CRUD simples
- Não há lógica de negócio complexa
- Validações básicas são suficientes

**Crie Commands/Queries Específicos quando:**
- Há regras de negócio específicas
- Validações complexas são necessárias
- Há lógica adicional além do CRUD básico

### Exemplo: Command Específico

```csharp
// Command específico para criar produto
namespace PrismaPrimeMarket.Application.UseCases.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    Guid CategoryId
) : ICommand<Response<ProductDto>>;

// Handler específico
public class CreateProductCommandHandler(
    IBaseRepository<Product> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : ICommandHandler<CreateProductCommand, Response<ProductDto>>
{
    public async Task<Response<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // 1. Criar entidade com factory method (Rich Domain Model)
        var product = Product.Create(
            request.Name,
            request.Description,
            Money.FromDecimal(request.Price),
            request.CategoryId
        );

        // 2. Validar regras de domínio
        // (Já validadas no método Create da entidade)

        // 3. Persistir
        await repository.AddAsync(product, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        // 4. Mapear e retornar
        var dto = mapper.Map<ProductDto>(product);
        return Response<ProductDto>.Created(dto);
    }
}

// Validator específico
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(200).WithMessage("Nome não pode ter mais de 200 caracteres");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Preço deve ser maior que zero");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Categoria é obrigatória");
    }
}
```

## 🔄 Pipeline Behaviors

### ValidationBehavior

Valida automaticamente todos os commands/queries usando FluentValidation antes da execução.

```csharp
// Validator será executado automaticamente
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
```

### LoggingBehavior

Loga automaticamente execução de commands/queries.

```
[Information] Handling CreateProductCommand
[Information] Handled CreateProductCommand successfully
```

## 📦 Registrando Handlers Específicos

Handlers genéricos são registrados automaticamente pelo MediatR. Para handlers específicos:

```csharp
// Não é necessário registrar manualmente!
// MediatR encontra automaticamente todos os handlers no assembly
```

## 🎯 Benefícios da Implementação

1. **Separação de Responsabilidades**: Commands modificam, Queries leem
2. **Validação Automática**: FluentValidation integrado via behavior
3. **Logging Automático**: Todas operações são logadas
4. **Testabilidade**: Handlers podem ser testados isoladamente
5. **Escalabilidade**: Fácil adicionar novos commands/queries
6. **Flexibilidade**: Use genéricos para CRUD simples, específicos para lógica complexa

## 🔀 Migrando de BaseService para CQRS

### Antes (BaseService)

```csharp
public class ProductsController(IProductService service) : BaseController<Product, ProductDto>(service)
{
    // Usa métodos herdados do BaseController
}
```

### Depois (CQRS)

```csharp
public class ProductsController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetByIdQuery<ProductDto>(id);
        var result = await Mediator.Send(query);
        return result.Succeeded ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var command = new CreateCommand<ProductDto>(dto);
        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
    }
}
```

## 🧪 Testando

### Testando Handler

```csharp
public class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidData_ShouldCreateProduct()
    {
        // Arrange
        var repository = new Mock<IBaseRepository<Product>>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var mapper = new Mock<IMapper>();
        
        var handler = new CreateProductCommandHandler(
            repository.Object,
            unitOfWork.Object,
            mapper.Object
        );

        var command = new CreateProductCommand("Test", "Desc", 100, Guid.NewGuid());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        repository.Verify(x => x.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
```

## 📚 Próximos Passos

1. **Criar Entidades de Domínio**: Product, Order, User, etc.
2. **Criar DTOs**: ProductDto, CreateProductDto, UpdateProductDto
3. **Criar Mappings**: ProductProfile com AutoMapper
4. **Criar Commands/Queries Específicos**: Para lógica de negócio complexa
5. **Criar Validators**: FluentValidation para cada command
6. **Adicionar Testes**: Unit tests para handlers

---

**Nota**: BaseService ainda existe no projeto para referência, mas **NÃO** deve ser usado em novos desenvolvimentos. Use sempre CQRS com MediatR.
