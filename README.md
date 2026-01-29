# Prisma Prime Market API

[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-blue)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![CI Pipeline](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/ci.yml/badge.svg)](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/ci.yml)
[![Docker Build](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/docker-build.yml/badge.svg)](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/docker-build.yml)
[![Code Quality](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/code-quality.yml/badge.svg)](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/code-quality.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## 📋 Sobre o Projeto

Prisma Prime Market API é um marketplace backend robusto e escalável desenvolvido como projeto de portfólio, demonstrando implementações de alto nível seguindo as melhores práticas de arquitetura de software, Clean Code e princípios SOLID.

Este projeto é focado exclusivamente no backend, fornecendo uma API RESTful completa para gerenciamento de produtos, usuários, pedidos, pagamentos e todo o ecossistema de um marketplace moderno.

## 🎯 Objetivos do Projeto

- Demonstrar arquitetura em camadas bem definida
- Implementar princípios SOLID e Clean Code
- Aplicar padrões de design (Design Patterns)
- Arquitetura orientada a microservices
- Testes automatizados (unitários, integração e E2E)
- Documentação completa e profissional
- CI/CD com boas práticas DevOps

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas, separando responsabilidades e garantindo manutenibilidade:

```
PrismaPrimeMarketAPI/
├── src/
│   ├── PrismaPrimeMarket.API/              # Camada de Apresentação (Controllers, Middlewares)
│   ├── PrismaPrimeMarket.Application/       # Camada de Aplicação (Use Cases, DTOs, Interfaces)
│   ├── PrismaPrimeMarket.Domain/            # Camada de Domínio (Entidades, Value Objects, Regras de Negócio)
│   ├── PrismaPrimeMarket.Infrastructure/    # Camada de Infraestrutura (Persistência, External Services)
│   └── PrismaPrimeMarket.CrossCutting/      # Recursos Transversais (IoC, Logging, Security)
├── tests/
│   ├── PrismaPrimeMarket.UnitTests/
│   ├── PrismaPrimeMarket.IntegrationTests/
│   └── PrismaPrimeMarket.E2ETests/
└── docs/
    ├── ARCHITECTURE.md
    ├── API.md
    └── CONTRIBUTING.md
```

### Principais Camadas

- **API**: Endpoints REST, validações de entrada, autenticação/autorização
- **Application**: Casos de uso, orquestração de domínio, DTOs, mapeamentos
- **Domain**: Coração da aplicação - entidades, agregados, regras de negócio
- **Infrastructure**: Implementações de persistência, serviços externos, messaging
- **CrossCutting**: Injeção de dependência, configurações, aspectos transversais

## 🚀 Tecnologias Utilizadas

### Core
- **.NET 8.0** - Framework principal
- **C# 12** - Linguagem de programação
- **ASP.NET Core** - Web API

### Persistência
- **Entity Framework Core** - ORM
- **PostgreSQL** - Banco de dados relacional
- **Redis** - Cache distribuído

### Messaging & Background Jobs
- **RabbitMQ** / **Azure Service Bus** - Message Broker
- **Azure Functions** - Background jobs e tarefas agendadas

### Autenticação & Segurança
- **JWT (JSON Web Tokens)** - Autenticação stateless
- **Identity** - Gerenciamento de usuários
- **OAuth 2.0** - Autorização de terceiros

### Documentação & Testes
- **Swagger/OpenAPI** - Documentação interativa da API
- **xUnit** - Framework de testes
- **Moq** - Mocking para testes
- **FluentAssertions** - Assertions legíveis

### Ferramentas & Qualidade
- **FluentValidation** - Validações fluentes
- **AutoMapper** - Mapeamento objeto-objeto
- **Serilog** - Logging estruturado
- **Polly** - Resilience e fault-handling
- **MediatR** - CQRS e mediator pattern

## 📦 Funcionalidades Principais

### Módulo de Usuários
- [ ] Registro e autenticação de usuários
- [ ] Perfis de usuário (Comprador, Vendedor, Admin)
- [ ] Gerenciamento de endereços
- [ ] Verificação de email/telefone

### Módulo de Produtos
- [ ] CRUD de produtos
- [ ] Categorização e tags
- [ ] Busca e filtros avançados
- [ ] Gestão de estoque
- [ ] Imagens e variações de produtos

### Módulo de Pedidos
- [ ] Carrinho de compras
- [ ] Processamento de pedidos
- [ ] Rastreamento de status
- [ ] Histórico de pedidos

### Módulo de Pagamentos
- [ ] Integração com gateways de pagamento
- [ ] Múltiplos métodos de pagamento
- [ ] Gestão de reembolsos
- [ ] Webhooks de confirmação

### Módulo de Avaliações
- [ ] Sistema de reviews e ratings
- [ ] Comentários e respostas
- [ ] Moderação de conteúdo

### Módulo de Notificações
- [ ] Email notifications
- [ ] Push notifications
- [ ] SMS notifications (opcional)

## 🔧 Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/)

**OU** (para desenvolvimento sem Docker):
- [PostgreSQL 17+](https://www.postgresql.org/download/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

## 🚀 Quick Start

### Opção 1: Docker Compose (Recomendado) 🐳

```bash
# Clone o repositório
git clone https://github.com/jorelrx/PrismaPrimeMarketAPI.git
cd PrismaPrimeMarketAPI

# Inicie todos os serviços
docker-compose up -d

# Acesse a API
# API: http://localhost:8080
# Swagger: http://localhost:8080/swagger
# PgAdmin: http://localhost:5050
```

**Pronto!** A API está rodando com banco de dados PostgreSQL e PgAdmin.

---

### Opção 2: Desenvolvimento Local

#### 1. Clone o repositório
```bash
git clone https://github.com/jorelrx/PrismaPrimeMarketAPI.git
cd PrismaPrimeMarketAPI
```

#### 2. Instalar validação local (Husky + Commitlint)

```bash
# Instalar dependências Node.js (commitlint, husky)
npm install

# Configurar Git hooks
npm run prepare
```

**Isso ativa:**
- ✅ Bloqueio de commits fora da convenção (feat, fix, etc.)
- ✅ Bloqueio de push se testes falharem

#### 3. Configure o banco de dados

**Com Docker:**
```bash
docker run -d \
  --name prismaprime-postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=PrismaPrimeMarketDB \
  -p 5432:5432 \
  postgres:16-alpine
```

**Ou instale PostgreSQL localmente** e crie o banco `PrismaPrimeMarketDB`

#### 3. Configure connection string

Edite `src/PrismaPrimeMarket.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=PrismaPrimeMarketDB;Username=postgres;Password=postgres"
  }
}
#### 4. Execute migrations e inicie a API

```bash
# Restaurar pacotes .NET
dotnet restore

# Build
dotnet build

# Aplicar migrations
dotnet ef database update --project src/PrismaPrimeMarket.Infrastructure --startup-project src/PrismaPrimeMarket.API

# Rodar testes (garantir que tudo está OK)
dotnet test

# Executar a API
dotnet run --project src/PrismaPrimeMarket.API

# Acesse: http://localhost:5000
# Swagger: http://localhost:5000/swagger
```

**Pronto!** Agora seus commits e pushes serão validados automaticamente. 🎉

---

## 🧪 Testes

### Executar todos os testes

```bash
# Testes locais
dotnet test

# Testes com Docker (ambiente isolado)
.\scripts\test-docker.bat           # Windows
docker-compose -f docker-compose.test.yml up --build --abort-on-container-exit  # Linux/Mac

# Validação completa (restore + build + test + format)
.\scripts\validate.bat              # Windows
./scripts/validate.sh               # Linux/Mac
```

### Git Hooks (Pre-Push)

Configurar hook para rodar testes antes de cada push:

```bash
git config core.hooksPath .githooks
```

Agora os testes rodarão automaticamente antes de cada `git push` e bloquearão o push se falharem! 🛡️

---

## 🐳 Docker

### Comandos Úteis

```bash
# Desenvolvimento local com live reload
docker-compose up -d

# Rebuild após mudanças
docker-compose up -d --build

# Ver logs
docker-compose logs -f api

# Parar todos os serviços
docker-compose down

# Limpar volumes (reset completo)
docker-compose down -v

# Rodar apenas testes
docker-compose -f docker-compose.test.yml up --build --abort-on-container-exit
```

### Ambientes

- **Development**: `docker-compose.yml` - Desenvolvimento local
- **Test**: `docker-compose.test.yml` - Testes automatizados

---

## 🔄 CI/CD

Pipeline completo com GitHub Actions:

- ✅ **CI**: Testes automatizados em Docker + análise de código
- ✅ **CD**: Deploy automático em Staging e Production
- ✅ **Security**: Scan de vulnerabilidades com Trivy
- ✅ **Quality**: Análise de código e formatação

Ver documentação completa: [CI/CD Docker Guide](docs/CI_CD_DOCKER.md)

### Status dos Pipelines

[![CI Pipeline](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/ci.yml/badge.svg)](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/ci.yml)
[![CD Pipeline](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/cd.yml/badge.svg)](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/cd.yml)
[![Docker Build](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/docker-build.yml/badge.svg)](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/docker-build.yml)

---

## 📚 Documentação

- [Arquitetura do Projeto](docs/ARCHITECTURE.md)
- [Estrutura do Projeto](docs/PROJECT_STRUCTURE.md)
- [Guia de API](docs/API.md)
- [CQRS Guide](docs/CQRS_GUIDE.md)
- [CI/CD com Docker](docs/CI_CD_DOCKER.md)
- [Testes Automatizados](docs/TESTING_AUTOMATION.md)
- [Referência Rápida](docs/QUICK_REFERENCE.md)
dotnet ef database update
```

### 4. Execute a aplicação
```bash
dotnet run
```

A API estará disponível em `https://localhost:5001` e a documentação Swagger em `https://localhost:5001/swagger`

## 🧪 Testes

### Executar todos os testes
```bash
dotnet test
```

### Executar testes com cobertura
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Executar testes por categoria
```bash
# Testes unitários
dotnet test --filter Category=Unit

# Testes de integração
dotnet test --filter Category=Integration

# Testes E2E
dotnet test --filter Category=E2E
```

## 📚 Documentação Adicional

### Geral
- [Arquitetura Detalhada](docs/ARCHITECTURE.md)
- [Documentação da API](docs/API.md)
- [Guia de Contribuição](docs/CONTRIBUTING.md)
- [Integração com IA (PostgreSQL + pgvector)](docs/AI_INTEGRATION.md)
- [Swagger UI](https://localhost:5001/swagger) (quando a aplicação estiver rodando)

### CI/CD e DevOps
- [Implementação CI/CD](docs/CI_CD_IMPLEMENTATION.md) - Visão geral do pipeline
- [Guia de Configuração CI/CD](docs/CI_CD_SETUP.md) - Setup passo a passo
- [Quick Reference](docs/QUICK_REFERENCE.md) - Comandos úteis
- [Workflows README](.github/workflows/README.md) - Documentação dos workflows

## 🎯 Princípios e Padrões Aplicados

### SOLID
- **S**ingle Responsibility Principle
- **O**pen/Closed Principle
- **L**iskov Substitution Principle
- **I**nterface Segregation Principle
- **D**ependency Inversion Principle

### Design Patterns
- Repository Pattern
- Unit of Work
- CQRS (Command Query Responsibility Segregation)
- Mediator Pattern
- Factory Pattern
- Strategy Pattern
- Specification Pattern

### Clean Code
- Nomes significativos e descritivos
- Funções pequenas e focadas
- Comentários apenas quando necessário
- Tratamento de erros consistente
- Testes como documentação

## 🔐 Segurança

- Autenticação baseada em JWT
- Validação de entrada rigorosa
- Proteção contra CORS
- Rate limiting
- Criptografia de dados sensíveis
- Auditoria e logging de ações críticas

## 🚀 Roadmap

### Fase 1 - Fundação (Atual)
- [x] Estruturação do projeto
- [x] Documentação inicial
- [ ] Configuração de CI/CD
- [ ] Setup de infraestrutura base

### Fase 2 - Core Features
- [ ] Autenticação e autorização
- [ ] CRUD de produtos
- [ ] Sistema de pedidos
- [ ] Integração de pagamentos

### Fase 3 - Features Avançadas
- [ ] Sistema de notificações
- [ ] Busca avançada com Elasticsearch
- [ ] Cache distribuído
- [ ] Message queue

### Fase 4 - Otimização
- [ ] Performance tuning
- [ ] Monitoramento e observability
- [ ] Escalabilidade horizontal
- [ ] Documentação completa

## 👨‍💻 Autor

**Jorel**
- GitHub: [@jorelrx](https://github.com/jorelrx)
- LinkedIn: [Joel Victor](https://www.linkedin.com/in/joel-victor/)
- Email: joelv.9j@gmail.com

## 🙏 Agradecimentos

Este projeto foi desenvolvido como parte do meu portfólio profissional, demonstrando habilidades em:
- Arquitetura de software
- Desenvolvimento backend com .NET
- Boas práticas de programação
- DevOps e CI/CD
- Documentação técnica

---

⭐ Se este projeto foi útil para você, considere dar uma estrela no repositório!
