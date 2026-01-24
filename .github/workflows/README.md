# GitHub Actions Workflows

Este diretório contém os workflows do GitHub Actions para CI/CD do Prisma Prime Market API.

## 📋 Workflows Disponíveis

### 1. CI Pipeline (`ci.yml`)

**Trigger:** Push e Pull Requests para `main` e `develop`

Executa o pipeline de integração contínua completo:

#### Jobs:
- **build**: Compila o projeto e executa todos os testes
  - Restaura dependências
  - Build da solução
  - Executa testes unitários
  - Executa testes de integração
  - Coleta cobertura de código
  - Upload dos resultados para Codecov

- **code-quality**: Verifica qualidade do código
  - Verifica formatação com `dotnet format`
  - Escaneia pacotes vulneráveis
  - Lista pacotes deprecados

- **analyze**: Análise estática de código
  - Executa Roslyn Analyzers
  - Verifica warnings e erros de compilação

- **build-status**: Status consolidado de todos os jobs

**Badges:**
```markdown
![CI Pipeline](https://github.com/USERNAME/PrismaPrimeMarketAPI/actions/workflows/ci.yml/badge.svg)
```

### 2. PR Validation (`pr-checks.yml`)

**Trigger:** Pull Requests (opened, synchronized, reopened)

Valida Pull Requests antes do merge:

#### Jobs:
- **pr-validation**: Validação básica
  - Build da solução
  - Execução de testes
  - Comentário automático com resultados

- **size-label**: Adiciona labels de tamanho do PR
  - xs: até 10 linhas
  - s: até 100 linhas
  - m: até 500 linhas
  - l: até 1000 linhas
  - xl: mais de 1000 linhas

- **conventional-commits**: Valida formato de commits
  - Verifica Conventional Commits
  - Valida título do PR

**Tipos de commit aceitos:**
- `feat`: Nova funcionalidade
- `fix`: Correção de bug
- `docs`: Documentação
- `style`: Formatação
- `refactor`: Refatoração
- `perf`: Melhoria de performance
- `test`: Testes
- `build`: Build
- `ci`: CI/CD
- `chore`: Manutenção
- `revert`: Reversão

### 3. Code Quality Analysis (`code-quality.yml`)

**Trigger:** 
- Push para `main` e `develop`
- Pull Requests
- Agendado semanalmente (domingos)
- Manual

Análise profunda de qualidade e segurança:

#### Jobs:
- **analyze-csharp**: Análise de código C#
  - Microsoft.CodeAnalysis.NetAnalyzers
  - SecurityCodeScan
  - Nível de análise: latest

- **dependency-review**: Revisão de dependências (PRs)
  - Verifica vulnerabilidades em dependências
  - Fail em severidade alta ou superior

- **codeql-analysis**: Análise de segurança CodeQL
  - Queries de segurança e qualidade
  - Detecção de vulnerabilidades
  - Upload para GitHub Security

- **dotnet-format**: Verificação de formatação
  - Valida se o código está formatado corretamente
  - Instruções para correção

- **metrics**: Métricas de código
  - Cálculo de métricas de qualidade
  - Relatórios armazenados como artefatos

### 4. Release Build (`release.yml`)

**Trigger:**
- Publicação de Release no GitHub
- Manual (com input de versão)

Build e empacotamento para releases:

#### Jobs:
- **build-release**: Build de produção
  - Build com versão específica
  - Execução de testes
  - Publicação da API
  - Criação de pacote de deployment (.tar.gz)
  - Upload para a release do GitHub
  - Geração de release notes automáticas

- **docker-build**: Build da imagem Docker (releases)
  - Build da imagem
  - Tag com versão e latest
  - Push para Docker Hub (se configurado)

## 🔧 Configuração

### Secrets Necessários

Configure os seguintes secrets no repositório (Settings → Secrets and variables → Actions):

#### Obrigatórios para CI/CD completo:
- `CODECOV_TOKEN`: Token do Codecov para upload de cobertura
  - Obtenha em: https://codecov.io/

#### Opcionais (para releases Docker):
- `DOCKER_USERNAME`: Usuário do Docker Hub
- `DOCKER_PASSWORD`: Senha/token do Docker Hub

### Permissões do GitHub Token

Para CodeQL e análise de segurança, garanta que o repositório tenha:
- Settings → Actions → General → Workflow permissions:
  - ✅ Read and write permissions
  - ✅ Allow GitHub Actions to create and approve pull requests

## 📊 Status Badges

Adicione badges ao README principal:

```markdown
<!-- CI/CD Status -->
[![CI Pipeline](https://github.com/USERNAME/PrismaPrimeMarketAPI/actions/workflows/ci.yml/badge.svg)](https://github.com/USERNAME/PrismaPrimeMarketAPI/actions/workflows/ci.yml)
[![Code Quality](https://github.com/USERNAME/PrismaPrimeMarketAPI/actions/workflows/code-quality.yml/badge.svg)](https://github.com/USERNAME/PrismaPrimeMarketAPI/actions/workflows/code-quality.yml)
[![codecov](https://codecov.io/gh/USERNAME/PrismaPrimeMarketAPI/branch/main/graph/badge.svg)](https://codecov.io/gh/USERNAME/PrismaPrimeMarketAPI)
```

## 🚀 Uso

### Executar Build Localmente

Para replicar o processo de CI localmente:

```bash
# Restore
dotnet restore

# Build
dotnet build --configuration Release --no-restore

# Testes
dotnet test --configuration Release --no-build --verbosity normal

# Format check
dotnet format --verify-no-changes

# Security scan
dotnet list package --vulnerable --include-transitive
```

### Disparar Workflow Manualmente

1. Vá para Actions no GitHub
2. Selecione o workflow desejado
3. Clique em "Run workflow"
4. Escolha a branch e preencha inputs (se aplicável)
5. Clique em "Run workflow"

### Criar uma Release

1. Garanta que `main` está estável
2. Crie uma tag de versão:
   ```bash
   git tag -a v1.0.0 -m "Release 1.0.0"
   git push origin v1.0.0
   ```
3. No GitHub, vá para Releases → Draft a new release
4. Escolha a tag criada
5. Preencha título e descrição
6. Publique a release
7. O workflow `release.yml` será executado automaticamente

## 📝 Manutenção

### Atualizar Versão do .NET

Edite a variável de ambiente em todos os workflows:

```yaml
env:
  DOTNET_VERSION: '8.0.x'  # Atualize aqui
```

### Adicionar Novo Job

1. Edite o arquivo `.yml` apropriado
2. Adicione o novo job seguindo o padrão existente
3. Teste localmente com `act` (opcional):
   ```bash
   act -j job-name
   ```
4. Commit e push

### Modificar Estratégia de Testes

Edite os filtros de categoria em `ci.yml`:

```yaml
# Testes unitários
--filter "Category=Unit"

# Testes de integração
--filter "Category=Integration"

# Todos os testes
# (remova o --filter)
```

## 🔍 Troubleshooting

### Build Falha por Projeto Não Encontrado

**Problema:** Workflows falham porque projetos ainda não foram criados.

**Solução:** Os workflows são preparados para quando os projetos forem criados. Use `continue-on-error: true` temporariamente ou aguarde a criação da estrutura.

### Testes Não Encontrados

**Problema:** `dotnet test` não encontra testes.

**Solução:** Garanta que:
- Projetos de teste existem em `tests/`
- Testes têm `[Fact]` ou `[Theory]` (xUnit)
- Categorias estão corretas: `[Trait("Category", "Unit")]`

### CodeQL Demora Muito

**Problema:** Análise CodeQL timeout ou demora muito.

**Solução:**
- CodeQL é executado apenas em push para main/develop e PRs
- Para branches de feature, o job será ignorado
- Considere executar apenas semanalmente

### Codecov Token Inválido

**Problema:** Upload de cobertura falha.

**Solução:**
1. Obtenha token em https://codecov.io/
2. Adicione como secret: `CODECOV_TOKEN`
3. Ou configure `fail_ci_if_error: false` (já configurado)

## 📚 Recursos

- [GitHub Actions Documentation](https://docs.github.com/actions)
- [.NET CLI Reference](https://docs.microsoft.com/dotnet/core/tools/)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Codecov](https://docs.codecov.com/)
- [CodeQL](https://codeql.github.com/docs/)

## ✅ Checklist de Implementação

- [x] CI Pipeline básico
- [x] Testes automatizados
- [x] Code quality checks
- [x] PR validation
- [x] Security scanning
- [x] Release automation
- [ ] Docker Hub push (necessita secrets)
- [ ] Deploy automático (próxima fase)
- [ ] Testes E2E (quando implementados)

---

**Última atualização:** 2026-01-19
