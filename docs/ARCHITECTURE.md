# Arquitetura do Prisma Prime Market API

## 📐 Visão Geral

Este documento descreve a arquitetura do Prisma Prime Market API, um sistema backend construído seguindo os princípios de Clean Architecture, Domain-Driven Design (DDD) e arquitetura orientada a microservices.

## 🎯 Princípios Arquiteturais

### 1. Separação de Responsabilidades
Cada camada tem uma responsabilidade clara e bem definida, facilitando manutenção e evolução.

### 2. Independência de Frameworks
O core da aplicação (Domain) não depende de frameworks externos, permitindo flexibilidade tecnológica.

### 3. Testabilidade
Arquitetura projetada para facilitar testes em todos os níveis.

### 4. Independência de UI
A lógica de negócio não conhece a camada de apresentação.

### 5. Independência de Banco de Dados
O domínio não depende de tecnologias de persistência específicas.

## 🏛️ Estrutura em Camadas

```
┌─────────────────────────────────────────────────────────────┐
│                         API Layer                            │
│  Controllers | Middlewares | Filters | Validators           │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                    Application Layer                         │
│  Use Cases | DTOs | Mappers | Interfaces | Services         │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│                      Domain Layer                            │
│  Entities | Value Objects | Aggregates | Domain Services    │
│  Events | Specifications | Interfaces                       │
└─────────────────────────────────────────────────────────────┘
                            ↑
┌─────────────────────────────────────────────────────────────┐
│                  Infrastructure Layer                        │
│  Repositories | Data Context | External Services            │
│  Message Bus | Email Service | Storage                      │
└─────────────────────────────────────────────────────────────┘
                            ↑
┌─────────────────────────────────────────────────────────────┐
│                   CrossCutting Layer                         │
│  IoC Container | Logging | Security | Caching               │
└─────────────────────────────────────────────────────────────┘
```

## 📦 Detalhamento das Camadas

### 1️⃣ API Layer (PrismaPrimeMarket.API)

**Responsabilidade**: Interface de comunicação com o mundo externo (HTTP/REST).

#### Estrutura:
```
PrismaPrimeMarket.API/
├── Controllers/
│   ├── V1/
│   │   ├── ProductsController.cs
│   │   ├── OrdersController.cs
│   │   ├── UsersController.cs
│   │   └── PaymentsController.cs
│   └── V2/
├── Middlewares/
│   ├── ExceptionHandlingMiddleware.cs
│   ├── RequestLoggingMiddleware.cs
│   └── RateLimitingMiddleware.cs
├── Filters/
│   ├── ValidationFilter.cs
│   └── AuthorizationFilter.cs
├── Extensions/
│   └── ServiceCollectionExtensions.cs
├── Models/
│   ├── Requests/
│   └── Responses/
└── Program.cs
```

#### Responsabilidades:
- Receber requisições HTTP
- Validar entrada de dados (validações básicas)
- Rotear para Application Layer
- Formatar respostas
- Gerenciar autenticação/autorização
- Documentação via Swagger/OpenAPI
- Versionamento de API
- CORS e políticas de segurança

#### Tecnologias:
- ASP.NET Core Web API
- Swagger/Swashbuckle
- FluentValidation
- JWT Bearer Authentication

---

### 2️⃣ Application Layer (PrismaPrimeMarket.Application)

**Responsabilidade**: Orquestração da lógica de negócio, casos de uso da aplicação.

#### Estrutura:
```
PrismaPrimeMarket.Application/
├── UseCases/
│   ├── Products/
│   │   ├── Commands/
│   │   │   ├── CreateProduct/
│   │   │   │   ├── CreateProductCommand.cs
│   │   │   │   ├── CreateProductCommandHandler.cs
│   │   │   │   └── CreateProductCommandValidator.cs
│   │   │   ├── UpdateProduct/
│   │   │   └── DeleteProduct/
│   │   └── Queries/
│   │       ├── GetProduct/
│   │       ├── GetProductList/
│   │       └── SearchProducts/
│   ├── Orders/
│   ├── Users/
│   └── Payments/
├── DTOs/
│   ├── Product/
│   │   ├── ProductDto.cs
│   │   ├── CreateProductDto.cs
│   │   └── UpdateProductDto.cs
│   └── Order/
├── Mappings/
│   ├── ProductProfile.cs
│   ├── OrderProfile.cs
│   └── UserProfile.cs
├── Validators/
│   ├── ProductValidators/
│   └── OrderValidators/
└── Common/
    ├── Behaviors/
    │   ├── ValidationBehavior.cs
    │   └── LoggingBehavior.cs
    ├── Exceptions/
    │   ├── ValidationException.cs
    │   └── NotFoundException.cs
    ├── Messaging/
    │   ├── ICommand.cs
    │   ├── IQuery.cs
    │   ├── ICommandHandler.cs
    │   └── IQueryHandler.cs
    └── Models/
        ├── Response.cs
        ├── PagedResponse.cs
        ├── ResponseType.cs
        ├── ResponseMessages.cs
        └── PaginationFilter.cs
```

#### Responsabilidades:
- Implementar casos de uso
- Coordenar fluxo entre Domain e Infrastructure
- Validações de regras de aplicação
- Mapeamento entre DTOs e Entidades
- Tratamento de exceções de negócio
- CQRS (separação de Commands e Queries)
- Pipeline de comportamentos (logging, validação, etc.)
- Padronização de respostas da API

#### Padrões Aplicados:
- **CQRS**: Separação de comandos e consultas com MediatR
- **Mediator Pattern**: MediatR para desacoplar handlers
- **DTO Pattern**: Transferência de dados sem expor entidades
- **Validation Pipeline**: FluentValidation com behaviors
- **Response Pattern**: Respostas padronizadas com `Response<T>` e `PagedResponse<T>`

#### Response Pattern

Todas as operações retornam respostas padronizadas:

```csharp
// Response simples
public class Response<T>
{
    public T? Data { get; set; }
    public bool Succeeded { get; set; }
    public string[]? Errors { get; set; }
    public string Message { get; set; }
    public ResponseType Type { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Path { get; set; }
}

// Response paginada
public class PagedResponse<T> : Response<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; }
    public bool HasNextPage { get; }
}

// Factory methods
Response<ProductDto>.Created(data);
Response<ProductDto>.Updated(data);
Response<ProductDto>.NotFound("Mensagem");
PagedResponse<IEnumerable<T>>.Create(data, page, size, total);
```

**Benefícios:**
- Consistência em todas as respostas da API
- Facilita tratamento de erros no frontend
- Metadados úteis (timestamp, path, type)
- Suporte nativo a paginação
- Factory methods para simplificar criação

#### Tecnologias:
- MediatR
- AutoMapper
- FluentValidation

---

### 3️⃣ Domain Layer (PrismaPrimeMarket.Domain)

**Responsabilidade**: Coração da aplicação - regras de negócio puras, agnósticas de tecnologia.

#### Estrutura:
```
PrismaPrimeMarket.Domain/
├── Entities/
│   ├── Product.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   ├── User.cs
│   ├── Payment.cs
│   ├── Review.cs
│   └── Category.cs
├── ValueObjects/
│   ├── Money.cs
│   ├── Address.cs
│   ├── Email.cs
│   ├── CPF.cs
│   └── PhoneNumber.cs
├── Aggregates/
│   ├── OrderAggregate/
│   │   ├── Order.cs
│   │   ├── OrderItem.cs
│   │   └── OrderStatus.cs
│   └── ProductAggregate/
├── Enums/
│   ├── OrderStatus.cs
│   ├── PaymentMethod.cs
│   ├── PaymentStatus.cs
│   └── UserRole.cs
├── Events/
│   ├── DomainEvent.cs
│   ├── OrderCreatedEvent.cs
│   ├── OrderPaidEvent.cs
│   ├── ProductCreatedEvent.cs
│   └── ProductStockUpdatedEvent.cs
├── Interfaces/
│   ├── Repositories/
│   │   ├── IProductRepository.cs
│   │   ├── IOrderRepository.cs
│   │   ├── IUserRepository.cs
│   │   └── IUnitOfWork.cs
├── Specifications/
│   ├── ProductSpecifications/
│   │   ├── ProductByIdSpecification.cs
│   │   ├── ActiveProductsSpecification.cs
│   │   └── ProductsByCategorySpecification.cs
│   └── OrderSpecifications/
└── Common/
    ├── BaseEntity.cs
    ├── IAggregateRoot.cs
    └── IEntity.cs
```

#### Responsabilidades:
- Definir entidades e objetos de valor
- Implementar regras de negócio
- Manter invariantes do domínio
- Definir contratos (interfaces) para infraestrutura
- Emitir eventos de domínio
- Implementar especificações para queries complexas
- Serviços de domínio para operações que envolvem múltiplas entidades

#### Princípios DDD:
- **Entities**: Objetos com identidade única
- **Value Objects**: Objetos imutáveis sem identidade
- **Aggregates**: Cluster de entidades tratadas como unidade
- **Domain Events**: Eventos significativos no domínio
- **Repositories**: Abstração para persistência
- **Domain Services**: Lógica que não pertence a uma entidade específica
- **Specifications**: Encapsular lógica de query reutilizável

#### Características:
- **Sem dependências externas** (frameworks, ORM, etc.)
- **Testável isoladamente**
- **Rica em comportamento** (não apenas dados)
- **Imutabilidade** em Value Objects
- **Validações** em nível de entidade

---

### 4️⃣ Infrastructure Layer (PrismaPrimeMarket.Infrastructure)

**Responsabilidade**: Implementações concretas de persistência, serviços externos e integrações.

#### Estrutura:
```
PrismaPrimeMarket.Infrastructure/
├── Data/
│   ├── Context/
│   │   └── ApplicationDbContext.cs
│   ├── Configurations/
│   │   ├── ProductConfiguration.cs
│   │   ├── OrderConfiguration.cs
│   │   └── UserConfiguration.cs
│   ├── Migrations/
│   └── Seeds/
│       └── DatabaseSeeder.cs
├── Repositories/
│   ├── ProductRepository.cs
│   ├── OrderRepository.cs
│   ├── UserRepository.cs
│   └── UnitOfWork.cs
├── Identity/
│   ├── ApplicationUser.cs
│   ├── IdentityService.cs
│   └── JwtTokenGenerator.cs
├── ExternalServices/
│   ├── Payment/
│   │   ├── StripePaymentGateway.cs
│   │   └── PayPalPaymentGateway.cs
│   ├── Email/
│   │   └── SendGridEmailService.cs
│   ├── Storage/
│   │   ├── AzureBlobStorageService.cs
│   │   └── LocalStorageService.cs
│   └── Notification/
│       └── FirebaseNotificationService.cs
├── MessageBus/
│   ├── RabbitMQMessageBus.cs
│   └── Events/
│       └── OrderCreatedEventHandler.cs
├── Caching/
│   ├── RedisCacheService.cs
│   └── MemoryCacheService.cs
└── BackgroundJobs/
    ├── HangfireConfiguration.cs
    └── Jobs/
        ├── ProcessPendingOrdersJob.cs
        └── SendReminderEmailsJob.cs
```

#### Responsabilidades:
- Implementar repositórios
- Gerenciar contexto de banco de dados (EF Core)
- Configurar mapeamento ORM
- Implementar serviços externos (email, storage, payment)
- Message bus e event handlers
- Cache distribuído
- Background jobs
- Migrações de banco de dados

#### Tecnologias:
- Entity Framework Core
- PostgreSQL (com extensões para IA: pgvector, pg_trgm)
- Redis
- RabbitMQ / Azure Service Bus
- Hangfire
- Stripe/PayPal SDK
- SendGrid
- Azure Blob Storage

---

### 5️⃣ CrossCutting Layer (PrismaPrimeMarket.CrossCutting)

**Responsabilidade**: Aspectos transversais que permeiam todas as camadas.

#### Estrutura:
```
PrismaPrimeMarket.CrossCutting/
├── IoC/
│   ├── DependencyInjection.cs
│   └── NativeInjectorBootstrapper.cs
├── Logging/
│   ├── LoggingConfiguration.cs
│   └── SerilogExtensions.cs
├── Security/
│   ├── Encryption/
│   │   ├── AesEncryption.cs
│   │   └── IEncryptionService.cs
│   ├── Hashing/
│   │   └── PasswordHasher.cs
│   └── Claims/
│       └── ClaimsPrincipalExtensions.cs
├── Caching/
│   └── CacheConfiguration.cs
├── Configuration/
│   ├── AppSettings.cs
│   ├── JwtSettings.cs
│   └── DatabaseSettings.cs
└── Extensions/
    ├── StringExtensions.cs
    ├── DateTimeExtensions.cs
    └── EnumExtensions.cs
```

#### Responsabilidades:
- Injeção de dependência
- Configuração de logging
- Segurança e criptografia
- Configurações globais
- Extensions methods
- Recursos compartilhados

---

## 🔄 Fluxo de Dados

### Exemplo: Criar um Produto

```
1. HTTP POST /api/v1/products
   ↓
2. ProductsController.CreateProduct()
   ↓
3. Valida Request Model (FluentValidation)
   ↓
4. Envia CreateProductCommand via MediatR
   ↓
5. CreateProductCommandHandler.Handle()
   - Mapeia DTO → Entity
   - Valida regras de negócio
   - Chama ProductRepository.AddAsync()
   ↓
6. ProductRepository (Infrastructure)
   - Persiste no banco via EF Core
   - Dispara evento ProductCreatedEvent
   ↓
7. Event Handler processa evento
   - Envia notificação
   - Atualiza cache
   - Publica mensagem no bus
   ↓
8. Retorna ProductDto ao Controller
   ↓
9. HTTP 201 Created com ProductResponse
```

---

## 🎯 Padrões de Design Aplicados

### Repository Pattern
**Objetivo**: Abstrair a camada de acesso a dados.

```csharp
// Domain Layer - Interface
public interface IProductRepository
{
    Task<Product> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Guid id);
}

// Infrastructure Layer - Implementação
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    
    public async Task<Product> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    // ... outras implementações
}
```

### Unit of Work Pattern
**Objetivo**: Gerenciar transações e garantir consistência.

```csharp
public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    Task<int> CommitAsync();
    Task RollbackAsync();
}
```

### CQRS Pattern
**Objetivo**: Separar operações de leitura e escrita.

```csharp
// Command (Write)
public record CreateProductCommand(string Name, decimal Price) : IRequest<ProductDto>;

// Query (Read)
public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;
```

### Mediator Pattern
**Objetivo**: Desacoplar solicitações de seus handlers.

```csharp
// Controller
public async Task<IActionResult> Create(CreateProductRequest request)
{
    var command = new CreateProductCommand(request.Name, request.Price);
    var result = await _mediator.Send(command);
    return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
}
```

### Specification Pattern
**Objetivo**: Encapsular lógica de query reutilizável.

```csharp
public class ActiveProductsSpecification : Specification<Product>
{
    public override Expression<Func<Product, bool>> ToExpression()
    {
        return product => product.IsActive && product.Stock > 0;
    }
}
```

### Strategy Pattern
**Objetivo**: Diferentes estratégias de pagamento.

```csharp
public interface IPaymentStrategy
{
    Task<PaymentResult> ProcessPayment(decimal amount);
}

public class CreditCardPaymentStrategy : IPaymentStrategy { }
public class PixPaymentStrategy : IPaymentStrategy { }
public class BoletoPaymentStrategy : IPaymentStrategy { }
```

### Factory Pattern
**Objetivo**: Criar objetos complexos.

```csharp
public interface IPaymentGatewayFactory
{
    IPaymentGateway Create(PaymentMethod method);
}
```

---

## 🧪 Estratégia de Testes

### Pirâmide de Testes

```
       /\
      /E2E\          ← Poucos testes (10%)
     /------\
    /  Int   \       ← Testes médios (30%)
   /----------\
  /   Unit     \     ← Muitos testes (60%)
 /--------------\
```

### Testes Unitários
- **Foco**: Domain e Application layers
- **Frameworks**: xUnit, Moq, FluentAssertions
- **Cobertura alvo**: > 80%

### Testes de Integração
- **Foco**: Infrastructure e API integration
- **Ferramentas**: WebApplicationFactory, TestContainers
- **Escopo**: Banco de dados, repositórios, APIs

### Testes E2E
- **Foco**: Fluxos completos da aplicação
- **Ferramentas**: HTTP Client, postman collections
- **Cenários**: Jornadas de usuário completas

---

## 🔐 Segurança

### Camadas de Segurança

1. **Autenticação**: JWT Bearer Tokens
2. **Autorização**: Role-based e Policy-based
3. **Validação**: Input validation em todos os níveis
4. **Criptografia**: Dados sensíveis em repouso e em trânsito
5. **Rate Limiting**: Proteção contra abuso
6. **CORS**: Política restritiva
7. **HTTPS**: Obrigatório em produção
8. **SQL Injection**: Prevenido via EF Core
9. **XSS**: Sanitização de inputs

---

## 📊 Observabilidade

### Logging
- **Framework**: Serilog
- **Níveis**: Verbose, Debug, Information, Warning, Error, Fatal
- **Sinks**: Console, File, Elasticsearch, Application Insights

### Métricas
- Response time
- Request count
- Error rate
- Database query performance
- Cache hit/miss ratio

### Tracing
- Distributed tracing com OpenTelemetry
- Correlation IDs para rastreamento de requisições

---

## 🚀 Escalabilidade

### Horizontal Scaling
- Stateless API
- Load balancer ready
- Shared cache (Redis)
- Message queue para processamento assíncrono

### Vertical Scaling
- Database connection pooling
- Async/await throughout
- Efficient queries (projections, includes)

### Caching Strategy
- **L1 Cache**: Memory cache (local)
- **L2 Cache**: Redis (distribuído)
- **Cache-aside pattern**
- **TTL strategy**

---

## 🔄 Microservices Ready

Embora seja um monolito modular, a arquitetura permite fácil transição para microservices:

### Módulos Independentes
- Products Service
- Orders Service
- Payments Service
- Users Service
- Notifications Service

### Event-Driven Communication
- Domain events
- Message bus (RabbitMQ)
- Event sourcing ready

### Bounded Contexts
- Contextos bem definidos
- Comunicação via eventos
- Contratos claros (DTOs)

---

## 📈 Performance

### Database Optimization
- Índices estratégicos
- Query optimization
- Pagination
- Eager/Lazy loading consciente
- Connection pooling

### API Optimization
- Response compression
- Async operations
- Streaming para grandes volumes
- Partial responses (select fields)

### Caching
- Output caching
- Distributed caching
- Cache invalidation strategies

---

## 🛠️ DevOps & CI/CD

### Pipeline
1. Build
2. Unit Tests
3. Integration Tests
4. Code Quality Analysis (SonarQube)
5. Security Scan
6. Docker Build
7. Deploy to Staging
8. E2E Tests
9. Deploy to Production

### Infrastructure as Code
- Docker Compose para desenvolvimento
- Kubernetes para produção
- Terraform para cloud resources

---

## 📚 Referências

- **Clean Architecture** - Robert C. Martin
- **Domain-Driven Design** - Eric Evans
- **Implementing Domain-Driven Design** - Vaughn Vernon
- **Microsoft .NET Architecture Guides**
- **Enterprise Integration Patterns** - Gregor Hohpe

---

## 🎓 Decisões Arquiteturais (ADR)

Decisões importantes de arquitetura são documentadas em:
- `docs/adr/001-architecture-style.md`
- `docs/adr/002-database-choice.md`
- `docs/adr/003-cqrs-implementation.md`

---

**Última atualização**: Janeiro 2026  
**Versão**: 1.0
