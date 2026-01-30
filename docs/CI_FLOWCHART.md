# 📊 Fluxogramas do CI

Visualização dos processos de CI do projeto usando Mermaid.

---

## 📋 Índice

- [Fluxo Completo de Contribuição](#fluxo-completo-de-contribuição)
- [Fluxo de Pull Request](#fluxo-de-pull-request)
- [Workflow: PR Checks](#workflow-pr-checks)
- [Workflow: CI Pipeline](#workflow-ci-pipeline)
- [Workflow: Code Quality](#workflow-code-quality)
- [Workflow: Docker Build](#workflow-docker-build)
- [Validação de Commits](#validação-de-commits)

---

## 🔄 Fluxo Completo de Contribuição

```mermaid
flowchart TD
    A[Início] --> B[Atualizar branch main]
    B --> C[Criar branch feat/xxx]
    C --> D[Desenvolver alterações]
    D --> E[git add + git commit]
    E --> F{Hook pre-commit<br/>Valida formato}
    F -->|❌ Inválido| G[Corrigir mensagem]
    G --> E
    F -->|✅ Válido| H[git push]
    H --> I{Hook pre-push<br/>Build & Testes}
    I -->|❌ Falhou| J[Corrigir código]
    J --> E
    I -->|✅ Passou| K[Push para GitHub]
    K --> L[Criar Pull Request]
    L --> M[GitHub Actions<br/>3 Workflows]
    M --> N[PR Checks]
    M --> O[CI Pipeline]
    M --> P[Code Quality]
    N --> Q{Todos passaram?}
    O --> Q
    P --> Q
    Q -->|❌ Não| R[Corrigir e push]
    R --> M
    Q -->|✅ Sim| S[Code Review]
    S --> T{Aprovado?}
    T -->|❌ Não| U[Fazer mudanças]
    U --> R
    T -->|✅ Sim| V[Squash & Merge]
    V --> W[Merge na main]
    W --> X[Docker Build & Push]
    X --> Y[Fim]
```

---

## 🔀 Fluxo de Pull Request

```mermaid
flowchart TD
    A[Pull Request Criado] --> B[Trigger Workflows]
    B --> C[PR Checks<br/>Título, Commits, Label]
    B --> D[CI Pipeline<br/>Docker, Build, Tests]
    B --> E[Code Quality<br/>CodeQL, Format]
    C --> F{Todos passaram?}
    D --> F
    E --> F
    F -->|❌ Não| G[Fix Required]
    G --> B
    F -->|✅ Sim| H[Ready for Review]
    H --> I[Code Review]
    I --> J{Aprovado?}
    J -->|❌ Não| K[Changes Requested]
    K --> G
    J -->|✅ Sim| L[Merge Enabled]
    L --> M[Squash and Merge]
    M --> N[Branch Deleted]
    N --> O[PR Completo]
```

---

## ⚙️ Workflow: PR Checks

**Trigger:** Pull Request (opened/updated/reopened)

```mermaid
flowchart TD
    A[PR Trigger] --> B[Checkout código]
    B --> C{Validar Título<br/>Conventional?}
    C -->|❌ Não| D[Workflow Failed]
    C -->|✅ Sim| E{Validar Commits<br/>Conventional?}
    E -->|❌ Não| F[Workflow Failed]
    E -->|✅ Sim| G[Calcular tamanho PR]
    G --> H[Adicionar label<br/>xs/s/m/l/xl]
    H --> I[Success]
```

---

## 🔧 Workflow: CI Pipeline

**Trigger:** Pull Request ou Push para develop

```mermaid
flowchart TD
    A[CI Trigger] --> B[Etapa 1: Docker Tests]
    B --> C{Passou?}
    C -->|❌ Não| D[Failed - Para tudo]
    C -->|✅ Sim| E[Etapa 2: Build & Coverage]
    E --> F{Passou?}
    F -->|❌ Não| G[Failed - Para tudo]
    F -->|✅ Sim| H[Etapa 3: Quality]
    H --> I{Passou?}
    I -->|❌ Não| J[Failed - Para tudo]
    I -->|✅ Sim| K[Etapa 4: Static Analysis]
    K --> L{Passou?}
    L -->|❌ Não| M[Failed - Para tudo]
    L -->|✅ Sim| N[Etapa 5: Final Check]
    N --> O{Todas passaram?}
    O -->|❌ Não| P[Failed]
    O -->|✅ Sim| Q[Success]
```

---

## 🔍 Workflow: Code Quality

**Trigger:** Pull Request, Push, Schedule (weekly)

```mermaid
flowchart TD
    A[Code Quality Trigger] --> B[Checkout código]
    B --> C[Setup .NET 8.0]
    C --> D[Initialize CodeQL]
    D --> E[dotnet restore<br/>dotnet build]
    E --> F[CodeQL Analysis<br/>Security, Vulnerabilities]
    F --> G[Upload results<br/>GitHub Security]
    G --> H[Success]
```

---

## 🐳 Workflow: Docker Build

**Trigger:** Push para main ou develop (após CI passar)

```mermaid
flowchart TD
    A[Docker Build Trigger] --> B[Checkout código]
    B --> C[Login Docker Hub]
    C --> D[Setup Docker Buildx]
    D --> E[Build & Push Image<br/>Cache + Multi-stage]
    E --> F[Tag: latest<br/>ou branch-sha]
    F --> G[Success]
```

---

## 📝 Validação de Commits

**Formato:** `tipo(escopo): Descrição com primeira maiúscula`

### Pre-commit hook: bloqueio de commit na main
```mermaid
flowchart TD
    A[git commit] --> B[Hook pre-commit]
    B --> C{Branch main?}
    C -->|✅ Sim| D[❌ Bloqueado<br/>Não pode commitar na main]
    C -->|❌ Não| E[Abre editor de mensagem]
    D --> F[Tentar novamente]
    F --> A
```

### Commit-msg hook: validação do formato da mensagem
```mermaid
flowchart TD
    A[Salva mensagem de commit] --> B[Hook commit-msg]
    B --> C{Formato válido?<br/>tipo: Descrição}
    C -->|❌ Não| D[❌ Bloqueado<br/>Formato incorreto]
    C -->|✅ Sim| E{Primeira letra<br/>maiúscula?}
    E -->|❌ Não| D
    E -->|✅ Sim| F{Sem ponto final?}
    F -->|❌ Não| D
    F -->|✅ Sim| G{Menos 100 chars?}
    G -->|❌ Não| D
    G -->|✅ Sim| H[✅ Commit aceito]
    D --> I[Tentar novamente]
    I --> A
```

---

## 🎯 Tipos de Commit Válidos

| Tipo | Descrição | Exemplo |
|------|-----------|---------|
| `feat` | Nova funcionalidade | `feat: Adicionar login social` |
| `fix` | Correção de bug | `fix: Resolver erro de timeout` |
| `docs` | Documentação | `docs: Atualizar README` |
| `style` | Formatação | `style: Corrigir indentação` |
| `refactor` | Refatoração | `refactor: Extrair método` |
| `perf` | Performance | `perf: Otimizar query SQL` |
| `test` | Testes | `test: Adicionar teste unitário` |
| `build` | Build system | `build: Atualizar dependências` |
| `ci` | CI/CD | `ci: Atualizar workflow` |
| `chore` | Manutenção | `chore: Limpar código morto` |
| `revert` | Reverter commit | `revert: Desfazer mudança X` |

---

## 🔍 Troubleshooting

### Commit Bloqueado

```mermaid
flowchart TD
    A[❌ Erro: Commit inválido] --> B{Verificar tipo}
    B -->|OK| C{Verificar descrição}
    B -->|❌| D[Tipo minúsculo e válido]
    C -->|OK| E[Corrigir e tentar]
    C -->|❌| F[Primeira maiúscula<br/>Sem ponto<br/>Menos 100 chars]
    D --> E
    F --> E
    E --> G[git commit -m 'feat: Mensagem correta']
```

### Workflow Falhou

```mermaid
flowchart TD
    A[❌ GitHub Actions Failed] --> B[Acessar Actions tab]
    B --> C[Clicar workflow falho]
    C --> D[Ver logs detalhados]
    D --> E{Tipo de erro?}
    E -->|Build failed| F[dotnet build local]
    E -->|Tests failed| G[dotnet test local]
    E -->|Format failed| H[dotnet format<br/>git add<br/>git commit]
    F --> I[Corrigir código]
    G --> I
    H --> J[git push]
    I --> K[git add + commit]
    K --> J
    J --> L[Workflows re-executam<br/>automaticamente]
    L --> M{Passou?}
    M -->|❌ Não| D
    M -->|✅ Sim| N[Success]
```

---

## 📚 Referências

Para mais detalhes, consulte:

- **[CONTRIBUTING.md](CONTRIBUTING.md)** - Guia completo de contribuição
- **[CI_README.md](CI_README.md)** - Overview do CI
- **[CI_QUICK_REFERENCE.md](CI_QUICK_REFERENCE.md)** - Comandos rápidos
- **[CI_WORKFLOW_GUIDE.md](CI_WORKFLOW_GUIDE.md)** - Detalhes dos workflows

---

**Última atualização:** Janeiro 2026  
**Versão:** 2.0
