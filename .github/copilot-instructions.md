# GitHub Copilot Instructions - Prisma Prime Market API

## Objetivo
Fornecer orientações curtas e objetivas para o Copilot gerar código consistente com o projeto, sem exemplos inline. Para detalhes, sempre consulte os arquivos em `docs/`.

## Regras Essenciais
- Arquitetura: Clean Architecture + DDD + SOLID. Fluxo de dependências: `API → Application → Domain ← Infrastructure`, com `CrossCutting` transversal. O `Domain` não depende de nenhuma outra camada.
- Padrões: Use CQRS (commands/queries), validação com FluentValidation, eventos de domínio quando fizer sentido, e mapeamentos via camadas apropriadas.
- Assíncrono: Prefira métodos assíncronos, evite bloqueios síncronos.
- Exceções: Lançar exceções específicas (ex.: domínio, not found) e tratar onde devido.
- Logging: Logging estruturado; evite concatenação de strings.
- DI: Dependa de abstrações (interfaces). Registre serviços no container nas camadas corretas.
- Nomenclatura/Estilo: Siga convenções C# (PascalCase para tipos/métodos/propriedades públicas, camelCase para variáveis/parâmetros, campos privados com underscore). Evite magic numbers/strings.
- Persistência: Respeite limites de agregados; consultas somente leitura com `AsNoTracking` e paginação quando aplicável.

## Como o Copilot deve agir
- Gerar apenas o código necessário para a tarefa, mantendo as camadas e responsabilidades bem definidas.
- Nunca adicionar dependências do `Domain` para outras camadas.
- Quando precisar de detalhes (contratos, exemplos, comandos, decisões), consultar a documentação do projeto (seção abaixo) e seguir o que estiver definido lá.
- Evitar produzir exemplos extensos ou tutoriais no arquivo; remeter à documentação.

## Documentação a Consultar
- Arquitetura geral e decisões: `docs/ARCHITECTURE.md`, `docs/adr/001-architecture-style.md`, `docs/adr/002-database-choice.md`.
- Estrutura do projeto e camadas: `docs/PROJECT_STRUCTURE.md`.
- Referências rápidas (padrões, comandos, checklist): `docs/QUICK_REFERENCE.md`.
- Contratos e API pública: `docs/API.md`.
- Pipeline CI/CD (quando relevante): `docs/CI_CD_SUMMARY.md`.
- Visão geral e onboarding: `docs/README.md`.

## Checklist ao Gerar Código
- Camadas e dependências corretas e coesas.
- Convenções de nomenclatura e estilo aderentes ao guia do projeto.
- Aplicação de SOLID e do modelo de domínio rico (evitar anemic model).
- Validações no lugar correto (Domínio vs. Aplicação).
- Exceções e logs estruturados apropriados.
- Métodos assíncronos e consultas eficientes (seleção de colunas, paginação, `AsNoTracking`).
- Registro no DI e uso de interfaces.
- Testes necessários (unitários/integrados) conforme o impacto.

## Não Fazer
- Não incluir blocos de código de exemplo neste arquivo.
- Não quebrar as regras de dependência entre camadas.
- Não introduzir decisões que conflitem com os arquivos em `docs/`.

---

Última atualização: Janeiro 2026  
Versão: 2.0

# GitHub Copilot Instructions - Prisma Prime Market API

## 🎯 Visão Geral do Projeto

Este é o **Prisma Prime Market API**, um projeto de marketplace backend desenvolvido em **C# .NET 8.0** seguindo princípios de **Clean Architecture**, **Domain-Driven Design (DDD)** e **SOLID**. O projeto é estruturado em camadas bem definidas com foco em qualidade, testabilidade e manutenibilidade.

## 📐 Arquitetura

### Estrutura em Camadas

```
PrismaPrimeMarket.API           → Apresentação (Controllers, Middlewares)
PrismaPrimeMarket.Application   → Casos de uso (CQRS, Handlers, DTOs)
PrismaPrimeMarket.Domain        → Núcleo (Entities, Value Objects, Rules)
PrismaPrimeMarket.Infrastructure → Persistência (Repositories, External Services)
PrismaPrimeMarket.CrossCutting  → Transversal (IoC, Logging, Security)
```

### Fluxo de Dependências
```
API → Application → Domain ← Infrastructure
         ↓
   CrossCutting
```

**IMPORTANTE**: O Domain Layer NÃO deve depender de nenhuma outra camada.

---

## 🎨 Padrões e Convenções de Código

### Naming Conventions

#### Classes e Interfaces
```csharp
// ✅ CORRETO
public class ProductService { }
public interface IProductRepository { }
public record CreateProductCommand : IRequest<ProductDto>;
public class ProductDto { }
```

#### Métodos e Propriedades
```csharp
// ✅ CORRETO - PascalCase
public async Task<Product> GetProductByIdAsync(Guid id) { }
public string ProductName { get; set; }
public decimal Price { get; private set; }
```

#### Variáveis Locais e Parâmetros
```csharp
// ✅ CORRETO - camelCase
var productId = Guid.NewGuid();
var isActive = true;
public void ProcessOrder(Order order, User currentUser) { }
```

#### Campos Privados
```csharp
// ✅ CORRETO - camelCase com underscore
private readonly IProductRepository _productRepository;
private readonly ILogger<ProductService> _logger;
private string _cachedValue;
```

#### Constantes e Enums
```csharp
// ✅ CORRETO
public const int MaxPageSize = 100;
public const string DefaultCulture = "pt-BR";

public enum OrderStatus
{
    Pending,
    Processing,
    Completed,
    Cancelled
}
```

### Estrutura de Arquivos

#### Controllers
```csharp
// Arquivo: Controllers/V1/ProductsController.cs
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo produto
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var command = new CreateProductCommand(request.Name, request.Price);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
```

#### Commands (CQRS)
```csharp
// Arquivo: Application/UseCases/Products/Commands/CreateProduct/CreateProductCommand.cs
public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    Guid CategoryId
) : IRequest<ProductDto>;

// Handler
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // 1. Criar entidade de domínio
        var product = Product.Create(
            request.Name,
            request.Description,
            Money.FromDecimal(request.Price),
            request.CategoryId
        );

        // 2. Validar regras de domínio
        var validationResult = product.Validate();
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // 3. Persistir
        await _productRepository.AddAsync(product);
        await _unitOfWork.CommitAsync();

        // 4. Retornar DTO
        return _mapper.Map<ProductDto>(product);
    }
}

// Validator
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do produto é obrigatório")
            .MaximumLength(200).WithMessage("O nome não pode ter mais de 200 caracteres");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("O preço deve ser maior que zero");
    }
}
```

#### Entities (Domain)
```csharp
// Arquivo: Domain/Entities/Product.cs
public class Product : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }
    public int Stock { get; private set; }
    public bool IsActive { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }

    private Product() { } // EF Core

    private Product(string name, string description, Money price, Guid categoryId)
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
        Stock = 0;
        IsActive = true;
    }

    public static Product Create(string name, string description, Money price, Guid categoryId)
    {
        // Validações básicas
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("O nome do produto não pode ser vazio");

        if (price.Amount <= 0)
            throw new DomainException("O preço deve ser maior que zero");

        var product = new Product(name, description, price, categoryId);
        product.AddDomainEvent(new ProductCreatedEvent(product));
        
        return product;
    }

    public void UpdateStock(int quantity)
    {
        if (Stock + quantity < 0)
            throw new DomainException("Estoque insuficiente");

        Stock += quantity;
        AddDomainEvent(new ProductStockUpdatedEvent(Id, Stock));
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
```

#### Value Objects
```csharp
// Arquivo: Domain/ValueObjects/Money.cs
public class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    private Money() { } // EF Core

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money FromDecimal(decimal amount, string currency = "BRL")
    {
        if (amount < 0)
            throw new DomainException("O valor não pode ser negativo");

        return new Money(amount, currency);
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new DomainException("Não é possível somar valores de moedas diferentes");

        return new Money(Amount + other.Amount, Currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
```

#### Repositories (Interface no Domain)
```csharp
// Arquivo: Domain/Interfaces/Repositories/IProductRepository.cs
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PaginatedList<Product>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
```

#### Repositories (Implementação na Infrastructure)
```csharp
// Arquivo: Infrastructure/Repositories/ProductRepository.cs
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<PaginatedList<Product>> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt);

        return await PaginatedList<Product>.CreateAsync(query, pageNumber, pageSize);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
    }
}
```

---

## 🎯 Princípios SOLID

### Single Responsibility Principle (SRP)
```csharp
// ✅ CORRETO - Cada classe tem uma única responsabilidade
public class ProductService { } // Apenas lógica de negócio de produtos
public class ProductRepository { } // Apenas persistência de produtos
public class ProductValidator { } // Apenas validação de produtos
```

### Open/Closed Principle (OCP)
```csharp
// ✅ CORRETO - Aberto para extensão, fechado para modificação
public interface IPaymentStrategy
{
    Task<PaymentResult> ProcessAsync(decimal amount);
}

public class CreditCardPayment : IPaymentStrategy { }
public class PixPayment : IPaymentStrategy { }
public class BoletoPayment : IPaymentStrategy { }
```

### Liskov Substitution Principle (LSP)
```csharp
// ✅ CORRETO - Subtipos podem substituir tipos base
public abstract class PaymentMethod
{
    public abstract Task<bool> ProcessAsync(decimal amount);
}

public class CreditCard : PaymentMethod
{
    public override async Task<bool> ProcessAsync(decimal amount)
    {
        // Implementação específica
        return true;
    }
}
```

### Interface Segregation Principle (ISP)
```csharp
// ✅ CORRETO - Interfaces específicas
public interface IReadRepository<T>
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
}

public interface IWriteRepository<T>
{
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
```

### Dependency Inversion Principle (DIP)
```csharp
// ✅ CORRETO - Depende de abstrações
public class ProductService
{
    private readonly IProductRepository _repository; // Interface, não implementação
    
    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }
}
```

---

## 🧪 Testes

### Testes Unitários
```csharp
// Arquivo: Tests/UnitTests/Domain/Entities/ProductTests.cs
public class ProductTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateProduct()
    {
        // Arrange
        var name = "Notebook";
        var description = "Dell Inspiron";
        var price = Money.FromDecimal(2500);
        var categoryId = Guid.NewGuid();

        // Act
        var product = Product.Create(name, description, price, categoryId);

        // Assert
        product.Should().NotBeNull();
        product.Name.Should().Be(name);
        product.Price.Should().Be(price);
        product.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowDomainException()
    {
        // Arrange
        var name = "";
        var price = Money.FromDecimal(100);

        // Act
        Action act = () => Product.Create(name, "Description", price, Guid.NewGuid());

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O nome do produto não pode ser vazio");
    }

    [Fact]
    public void UpdateStock_WithValidQuantity_ShouldUpdateStock()
    {
        // Arrange
        var product = Product.Create("Product", "Desc", Money.FromDecimal(100), Guid.NewGuid());
        
        // Act
        product.UpdateStock(10);

        // Assert
        product.Stock.Should().Be(10);
    }
}
```

### Testes de Integração
```csharp
// Arquivo: Tests/IntegrationTests/Repositories/ProductRepositoryTests.cs
public class ProductRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly ApplicationDbContext _context;
    private readonly ProductRepository _repository;

    public ProductRepositoryTests(DatabaseFixture fixture)
    {
        _context = fixture.CreateContext();
        _repository = new ProductRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistProduct()
    {
        // Arrange
        var product = Product.Create("Test", "Desc", Money.FromDecimal(100), Guid.NewGuid());

        // Act
        await _repository.AddAsync(product);
        await _context.SaveChangesAsync();

        // Assert
        var saved = await _repository.GetByIdAsync(product.Id);
        saved.Should().NotBeNull();
        saved!.Name.Should().Be("Test");
    }
}
```

---

## 📝 Documentação de Código

### XML Documentation Comments
```csharp
/// <summary>
/// Cria um novo produto no sistema
/// </summary>
/// <param name="name">Nome do produto</param>
/// <param name="description">Descrição detalhada do produto</param>
/// <param name="price">Preço do produto em BRL</param>
/// <param name="categoryId">ID da categoria do produto</param>
/// <returns>Produto criado</returns>
/// <exception cref="DomainException">Lançada quando os dados são inválidos</exception>
public static Product Create(string name, string description, Money price, Guid categoryId)
{
    // Implementação
}
```

---

## 🚫 Anti-Patterns a Evitar

### ❌ Anemic Domain Model
```csharp
// ERRADO - Entidade sem comportamento
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// Service com toda a lógica
public class ProductService
{
    public void UpdatePrice(Product product, decimal newPrice)
    {
        if (newPrice <= 0)
            throw new Exception("Invalid price");
        product.Price = newPrice;
    }
}
```

### ✅ Rich Domain Model
```csharp
// CORRETO - Entidade com comportamento
public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Money Price { get; private set; }

    public void UpdatePrice(Money newPrice)
    {
        if (newPrice.Amount <= 0)
            throw new DomainException("O preço deve ser maior que zero");
        
        Price = newPrice;
        AddDomainEvent(new ProductPriceChangedEvent(Id, newPrice));
    }
}
```

### ❌ God Classes
```csharp
// ERRADO - Classe faz tudo
public class ProductManager
{
    public void CreateProduct() { }
    public void UpdateProduct() { }
    public void DeleteProduct() { }
    public void SendEmailNotification() { }
    public void GenerateReport() { }
    public void ProcessPayment() { }
}
```

### ❌ Magic Numbers/Strings
```csharp
// ERRADO
if (order.Status == 3) { }
if (user.Role == "admin") { }

// CORRETO
if (order.Status == OrderStatus.Completed) { }
if (user.Role == UserRole.Admin) { }
```

---

## 🔧 Configurações e Boas Práticas

### Async/Await
```csharp
// ✅ CORRETO
public async Task<Product> GetProductAsync(Guid id)
{
    return await _repository.GetByIdAsync(id);
}

// ❌ ERRADO - .Result bloqueia a thread
public Product GetProduct(Guid id)
{
    return _repository.GetByIdAsync(id).Result;
}
```

### Exception Handling
```csharp
// ✅ CORRETO - Exceções específicas
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

public class NotFoundException : Exception
{
    public NotFoundException(string entity, Guid id) 
        : base($"{entity} com ID {id} não encontrado") { }
}

// Uso
throw new NotFoundException(nameof(Product), productId);
```

### Logging
```csharp
// ✅ CORRETO - Structured logging
_logger.LogInformation("Produto criado. ProductId: {ProductId}, Name: {Name}", 
    product.Id, product.Name);

_logger.LogWarning("Tentativa de atualizar produto inexistente. ProductId: {ProductId}", 
    productId);

_logger.LogError(ex, "Erro ao processar pedido. OrderId: {OrderId}", orderId);

// ❌ ERRADO - String concatenation
_logger.LogInformation("Produto criado: " + product.Id + " - " + product.Name);
```

### Dependency Injection
```csharp
// Program.cs ou Startup.cs
services.AddScoped<IProductRepository, ProductRepository>();
services.AddScoped<IProductService, ProductService>();
services.AddTransient<IEmailService, SendGridEmailService>();
services.AddSingleton<ICacheService, RedisCacheService>();
```

---

## 📦 Quando Criar Novos Componentes

### Nova Entity
1. Crie no `Domain/Entities/`
2. Herde de `BaseEntity`
3. Implemente `IAggregateRoot` se for raiz de agregado
4. Use construtor privado + factory method `Create()`
5. Adicione validações no domínio
6. Emita domain events quando necessário

### Novo Use Case
1. Crie command/query em `Application/UseCases/{Feature}/Commands|Queries/`
2. Crie handler correspondente
3. Crie validator com FluentValidation
4. Adicione testes unitários

### Novo Repository
1. Defina interface em `Domain/Interfaces/Repositories/`
2. Implemente em `Infrastructure/Repositories/`
3. Registre no DI
4. Adicione testes de integração

### Novo Controller
1. Crie em `API/Controllers/V1/`
2. Use `[ApiController]` e versionamento
3. Injete `IMediator`
4. Documente com XML comments
5. Adicione exemplos de request/response

---

## 🎨 Code Style

### Organização de Usings
```csharp
// 1. System namespaces
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// 2. External libraries
using Microsoft.EntityFrameworkCore;
using AutoMapper;

// 3. Project namespaces
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Interfaces;
```

### Formatação
- **Indentação**: 4 espaços
- **Chaves**: Nova linha
- **Linhas em branco**: Uma entre métodos
- **Ordem de membros**: Campos → Propriedades → Construtores → Métodos

```csharp
public class ProductService
{
    // 1. Campos privados
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductService> _logger;

    // 2. Propriedades
    public bool IsInitialized { get; private set; }

    // 3. Construtor
    public ProductService(IProductRepository repository, ILogger<ProductService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    // 4. Métodos públicos
    public async Task<Product> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    // 5. Métodos privados
    private void ValidateProduct(Product product)
    {
        // Validação
    }
}
```

---

## 🔍 Code Review Checklist

Ao gerar código, sempre verifique:

- [ ] Nomenclatura segue convenções C#
- [ ] Princípios SOLID aplicados
- [ ] Camadas respeitam dependências corretas
- [ ] Exceções tratadas adequadamente
- [ ] Logging estruturado implementado
- [ ] Métodos assíncronos quando apropriado
- [ ] Validações no lugar correto (Domain vs Application)
- [ ] Testes criados/atualizados
- [ ] XML documentation comments adicionados
- [ ] Sem magic numbers/strings
- [ ] Injeção de dependência usada corretamente
- [ ] Entity Framework configurado corretamente (se aplicável)

---

## 🚀 Performance Guidelines

### Database Queries
```csharp
// ✅ CORRETO - AsNoTracking para readonly
var products = await _context.Products
    .AsNoTracking()
    .Where(p => p.IsActive)
    .ToListAsync();

// ✅ CORRETO - Select only needed columns
var productNames = await _context.Products
    .Select(p => new { p.Id, p.Name })
    .ToListAsync();

// ✅ CORRETO - Pagination
var products = await _context.Products
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();

// ❌ ERRADO - Loading everything
var products = await _context.Products.ToListAsync();
```

### Caching
```csharp
// ✅ CORRETO
public async Task<Product> GetProductAsync(Guid id)
{
    var cacheKey = $"product:{id}";
    var cached = await _cache.GetAsync<Product>(cacheKey);
    
    if (cached != null)
        return cached;

    var product = await _repository.GetByIdAsync(id);
    await _cache.SetAsync(cacheKey, product, TimeSpan.FromMinutes(10));
    
    return product;
}
```

---

## 📚 Recursos Úteis

- **Documentação .NET**: https://docs.microsoft.com/dotnet/
- **C# Coding Conventions**: https://docs.microsoft.com/dotnet/csharp/fundamentals/coding-style/
- **Entity Framework Core**: https://docs.microsoft.com/ef/core/
- **Clean Architecture**: https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html

---

## ⚡ Quick Commands

```bash
# Build
dotnet build

# Run
dotnet run --project src/PrismaPrimeMarket.API

# Test
dotnet test

# Create migration
dotnet ef migrations add MigrationName -p src/PrismaPrimeMarket.Infrastructure -s src/PrismaPrimeMarket.API

# Update database
dotnet ef database update -p src/PrismaPrimeMarket.Infrastructure -s src/PrismaPrimeMarket.API
```

---

**Última atualização**: Janeiro 2026  
**Versão**: 1.0

Ao gerar código para este projeto, sempre siga estas diretrizes para manter consistência e qualidade de alto nível.
