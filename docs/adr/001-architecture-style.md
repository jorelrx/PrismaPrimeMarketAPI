# ADR 001: Escolha do Estilo de Arquitetura

**Status:** Aceito  
**Data:** 2026-01-06  
**Decisão:** Joel Victor  
**Contexto:** Definição da arquitetura base do projeto

---

## Contexto

Precisávamos definir um estilo arquitetural para o Prisma Prime Market API que atendesse aos seguintes requisitos:

- Alta manutenibilidade
- Testabilidade em todos os níveis
- Separação clara de responsabilidades
- Facilidade de evolução e adição de novas features
- Possibilidade de futura migração para microservices
- Independência de frameworks e tecnologias
- Aplicação de princípios SOLID e Clean Code

## Decisão

Decidimos adotar **Clean Architecture** combinada com **Domain-Driven Design (DDD)** como estilo arquitetural principal.

### Estrutura de Camadas

```
API (Presentation)
    ↓
Application (Use Cases)
    ↓
Domain (Business Rules) ← Infrastructure (External Concerns)
    ↓
CrossCutting (Shared Concerns)
```

### Principais Características

1. **Domain Layer como núcleo**: Contém as regras de negócio puras, sem dependências externas
2. **Dependency Inversion**: Camadas externas dependem de abstrações definidas no domínio
3. **CQRS Pattern**: Separação de comandos (write) e queries (read)
4. **Repository Pattern**: Abstração da camada de persistência
5. **Rich Domain Model**: Entidades com comportamento, não apenas dados

## Consequências

### Positivas ✅

- **Testabilidade**: Cada camada pode ser testada isoladamente
- **Manutenibilidade**: Mudanças em frameworks não afetam o domínio
- **Escalabilidade**: Preparado para crescer e evoluir
- **Clareza**: Responsabilidades bem definidas
- **Reusabilidade**: Lógica de negócio pode ser reutilizada
- **Independência**: Core da aplicação independente de tecnologia

### Negativas ❌

- **Complexidade Inicial**: Curva de aprendizado para novos desenvolvedores
- **Overhead**: Mais camadas e abstrações podem parecer excessivas para features simples
- **Boilerplate**: Mais código necessário comparado a arquiteturas simples
- **Tempo de desenvolvimento**: Pode demorar mais inicialmente

### Neutras ⚖️

- **Estrutura de pastas**: Mais organizada, mas também mais profunda
- **Número de projetos**: Múltiplos projetos na solution
- **Configuração**: Mais configuração de DI necessária

## Alternativas Consideradas

### 1. Arquitetura em N-Camadas Tradicional
**Prós:**
- Simplicidade
- Familiar para maioria dos desenvolvedores

**Contras:**
- Forte acoplamento entre camadas
- Difícil de testar
- Domínio dependente de infraestrutura

### 2. Arquitetura Hexagonal (Ports & Adapters)
**Prós:**
- Isolamento do domínio
- Flexibilidade

**Contras:**
- Menos adotada na comunidade .NET
- Menos exemplos e recursos

### 3. Vertical Slice Architecture
**Prós:**
- Organização por features
- Menos boilerplate

**Contras:**
- Pode levar a duplicação de código
- Menos estruturado

## Referências

- [Clean Architecture - Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design - Eric Evans](https://www.domainlanguage.com/ddd/)
- [Microsoft .NET Architecture Guides](https://docs.microsoft.com/dotnet/architecture/)
- [Implementing DDD - Vaughn Vernon](https://vaughnvernon.com/)

## Notas de Implementação

- Cada camada será um projeto separado na solution
- Domain Layer não terá referências externas (exceto primitivos)
- Infrastructure implementará interfaces definidas no Domain
- Application orquestrará casos de uso via MediatR
- API será responsável apenas por HTTP concerns

## Revisão Futura

Esta decisão deve ser revisada:
- Após 6 meses de desenvolvimento
- Quando o time atingir 5+ desenvolvedores
- Se houver necessidade de migração para microservices
- Se a complexidade se tornar um impedimento significativo

---

**Última atualização:** 2026-01-06
