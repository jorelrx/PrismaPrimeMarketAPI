# Estrutura do Projeto - Prisma Prime Market API

Este documento descreve a estrutura de pastas e arquivos do projeto.

```
PrismaPrimeMarketAPI/
│
├── .github/
│   ├── workflows/              # GitHub Actions (CI/CD)
│   │   ├── ci.yml
│   │   ├── cd.yml
│   │   └── codeql.yml
│   ├── ISSUE_TEMPLATE/         # Templates de issues
│   │   ├── bug_report.md
│   │   ├── feature_request.md
│   │   └── question.md
│   ├── PULL_REQUEST_TEMPLATE.md
│   └── copilot-instructions.md # Instruções para o GitHub Copilot
│
├── docs/                       # Documentação do projeto
│   ├── ARCHITECTURE.md         # Documentação de arquitetura
│   ├── API.md                  # Documentação da API
│   ├── adr/                    # Architectural Decision Records
│   │   ├── 001-architecture-style.md
│   │   ├── 002-database-choice.md
│   │   └── 003-cqrs-implementation.md
│   └── diagrams/               # Diagramas de arquitetura
│       ├── architecture.png
│       ├── database-schema.png
│       └── flow-diagrams/
│
├── src/                        # Código fonte
│   │
│   ├── PrismaPrimeMarket.API/  # Camada de Apresentação
│   │   ├── Controllers/
│   │   │   └── V1/
│   │   │       ├── ProductsController.cs
│   │   │       ├── OrdersController.cs
│   │   │       ├── UsersController.cs
│   │   │       ├── PaymentsController.cs
│   │   │       └── ReviewsController.cs
│   │   ├── Middlewares/
│   │   │   ├── ExceptionHandlingMiddleware.cs
│   │   │   ├── RequestLoggingMiddleware.cs
│   │   │   └── RateLimitingMiddleware.cs
│   │   ├── Filters/
│   │   │   ├── ValidationFilter.cs
│   │   │   └── AuthorizationFilter.cs
│   │   ├── Extensions/
│   │   │   └── ServiceCollectionExtensions.cs
│   │   ├── Models/
│   │   │   ├── Requests/
│   │   │   └── Responses/
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.Production.json
│   │   └── Program.cs
│   │
│   ├── PrismaPrimeMarket.Application/ # Camada de Aplicação
│   │   ├── UseCases/
│   │   │   ├── Products/
│   │   │   │   ├── Commands/
│   │   │   │   │   ├── CreateProduct/
│   │   │   │   │   │   ├── CreateProductCommand.cs
│   │   │   │   │   │   ├── CreateProductCommandHandler.cs
│   │   │   │   │   │   └── CreateProductCommandValidator.cs
│   │   │   │   │   ├── UpdateProduct/
│   │   │   │   │   ├── DeleteProduct/
│   │   │   │   │   └── UpdateStock/
│   │   │   │   └── Queries/
│   │   │   │       ├── GetProduct/
│   │   │   │       ├── GetProductList/
│   │   │   │       └── SearchProducts/
│   │   │   ├── Orders/
│   │   │   ├── Users/
│   │   │   └── Payments/
│   │   ├── DTOs/
│   │   │   ├── Product/
│   │   │   ├── Order/
│   │   │   ├── User/
│   │   │   └── Payment/
│   │   ├── Mappings/
│   │   │   ├── ProductProfile.cs
│   │   │   ├── OrderProfile.cs
│   │   │   └── UserProfile.cs
│   │   ├── Validators/
│   │   └── Common/
│   │       ├── Behaviors/
│   │       ├── Exceptions/
│   │       └── Models/
│   │
│   ├── PrismaPrimeMarket.Domain/    # Camada de Domínio
│   │   ├── Entities/
│   │   │   ├── Product.cs
│   │   │   ├── Order.cs
│   │   │   ├── OrderItem.cs
│   │   │   ├── User.cs
│   │   │   ├── Payment.cs
│   │   │   ├── Review.cs
│   │   │   └── Category.cs
│   │   ├── ValueObjects/
│   │   │   ├── Money.cs
│   │   │   ├── Address.cs
│   │   │   ├── Email.cs
│   │   │   ├── CPF.cs
│   │   │   └── PhoneNumber.cs
│   │   ├── Aggregates/
│   │   │   ├── OrderAggregate/
│   │   │   └── ProductAggregate/
│   │   ├── Enums/
│   │   │   ├── OrderStatus.cs
│   │   │   ├── PaymentMethod.cs
│   │   │   └── UserRole.cs
│   │   ├── Events/
│   │   │   ├── DomainEvent.cs
│   │   │   ├── OrderCreatedEvent.cs
│   │   │   └── ProductCreatedEvent.cs
│   │   ├── Interfaces/
│   │   │   ├── Repositories/
│   │   │   │   ├── IProductRepository.cs
│   │   │   │   ├── IOrderRepository.cs
│   │   │   │   └── IUnitOfWork.cs
│   │   │   └── Services/
│   │   ├── Specifications/
│   │   ├── Services/
│   │   └── Common/
│   │       ├── BaseEntity.cs
│   │       └── IAggregateRoot.cs
│   │
│   ├── PrismaPrimeMarket.Infrastructure/ # Camada de Infraestrutura
│   │   ├── Data/
│   │   │   ├── Context/
│   │   │   │   └── ApplicationDbContext.cs
│   │   │   ├── Configurations/
│   │   │   │   ├── ProductConfiguration.cs
│   │   │   │   └── OrderConfiguration.cs
│   │   │   ├── Migrations/
│   │   │   └── Seeds/
│   │   ├── Repositories/
│   │   │   ├── ProductRepository.cs
│   │   │   ├── OrderRepository.cs
│   │   │   └── UnitOfWork.cs
│   │   ├── Identity/
│   │   │   ├── ApplicationUser.cs
│   │   │   └── JwtTokenGenerator.cs
│   │   ├── ExternalServices/
│   │   │   ├── Payment/
│   │   │   ├── Email/
│   │   │   ├── Storage/
│   │   │   └── Notification/
│   │   ├── MessageBus/
│   │   ├── Caching/
│   │   └── BackgroundJobs/
│   │
│   └── PrismaPrimeMarket.CrossCutting/ # Camada Transversal
│       ├── IoC/
│       │   └── DependencyInjection.cs
│       ├── Logging/
│       │   └── LoggingConfiguration.cs
│       ├── Security/
│       │   ├── Encryption/
│       │   └── Hashing/
│       └── Extensions/
│
├── tests/                      # Testes
│   ├── PrismaPrimeMarket.UnitTests/
│   │   ├── Domain/
│   │   │   ├── Entities/
│   │   │   └── ValueObjects/
│   │   └── Application/
│   │       └── UseCases/
│   ├── PrismaPrimeMarket.IntegrationTests/
│   │   ├── API/
│   │   │   └── Controllers/
│   │   └── Infrastructure/
│   │       └── Repositories/
│   └── PrismaPrimeMarket.E2ETests/
│       └── Scenarios/
│
├── scripts/                    # Scripts utilitários
│   ├── setup-dev.sh
│   ├── run-tests.sh
│   ├── seed-database.sql
│   └── generate-migration.sh
│
├── docker/                     # Arquivos Docker
│   ├── Dockerfile
│   ├── docker-compose.yml
│   └── docker-compose.override.yml
│
├── .gitignore
├── .editorconfig
├── README.md
├── CHANGELOG.md
├── LICENSE
├── CONTRIBUTING.md
└── PrismaPrimeMarketAPI.sln   # Solution file
```

## Descrição das Camadas

### API Layer
Contém controllers, middlewares e tudo relacionado à interface HTTP.

### Application Layer
Implementa casos de uso usando CQRS (Commands e Queries), DTOs e mapeamentos.

### Domain Layer
O coração da aplicação. Contém entidades, value objects, regras de negócio e interfaces.

### Infrastructure Layer
Implementações concretas de persistência, serviços externos e infraestrutura técnica.

### CrossCutting Layer
Aspectos transversais como IoC, logging, segurança e extensões.

## Convenções de Nomenclatura

- **Pastas**: PascalCase
- **Arquivos C#**: PascalCase (igual ao nome da classe)
- **Arquivos config**: lowercase com hífen (docker-compose.yml)
- **Tests**: NomeDaClasse + Tests.cs

## Padrões de Organização

1. Cada feature deve ter sua própria pasta
2. Commands e Queries devem estar separados
3. Um arquivo por classe/interface
4. Testes devem espelhar a estrutura do código fonte
