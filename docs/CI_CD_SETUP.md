# 🚀 Guia de Configuração CI/CD

Este guia contém instruções passo a passo para configurar completamente o pipeline de CI/CD do Prisma Prime Market API.

## 📋 Pré-requisitos

- Repositório GitHub criado
- Acesso de administrador ao repositório
- Conta no Codecov (para cobertura de código)

## 🔧 Configuração Inicial

### 1. Habilitar GitHub Actions

1. Vá para **Settings** → **Actions** → **General**
2. Em **Actions permissions**, selecione:
   - ✅ Allow all actions and reusable workflows
3. Em **Workflow permissions**, selecione:
   - ✅ Read and write permissions
   - ✅ Allow GitHub Actions to create and approve pull requests
4. Clique em **Save**

### 2. Configurar Branch Protection

#### Branch: `main`

1. Vá para **Settings** → **Branches** → **Add branch protection rule**
2. Branch name pattern: `main`
3. Configure as seguintes opções:

**Require a pull request before merging:**
- ✅ Enabled
- Require approvals: 1
- ✅ Dismiss stale pull request approvals when new commits are pushed
- ✅ Require review from Code Owners (opcional)

**Require status checks to pass before merging:**
- ✅ Enabled
- ✅ Require branches to be up to date before merging
- Status checks required:
  - `build / Build & Test`
  - `code-quality / Code Quality Checks`
  - `analyze / Static Code Analysis`

**Require conversation resolution before merging:**
- ✅ Enabled

**Do not allow bypassing the above settings:**
- ✅ Enabled (exceto para admins se necessário)

4. Clique em **Create**

#### Branch: `develop`

Repita o processo acima para a branch `develop` (opcional, com regras menos rígidas se preferir).

### 3. Configurar Codecov

#### Criar Conta e Obter Token

1. Acesse https://codecov.io/
2. Faça login com GitHub
3. Adicione o repositório PrismaPrimeMarketAPI
4. Copie o token de upload

#### Adicionar Token ao GitHub

1. No repositório, vá para **Settings** → **Secrets and variables** → **Actions**
2. Clique em **New repository secret**
3. Nome: `CODECOV_TOKEN`
4. Value: Cole o token copiado
5. Clique em **Add secret**

#### Configurar codecov.yml (Opcional)

Crie um arquivo `codecov.yml` na raiz do projeto:

```yaml
coverage:
  precision: 2
  round: down
  range: 70...100
  status:
    project:
      default:
        target: 80%
        threshold: 2%
    patch:
      default:
        target: 80%

comment:
  layout: "reach, diff, flags, files"
  behavior: default
  require_changes: false

ignore:
  - "tests/**"
  - "**/*.Designer.cs"
  - "**/Program.cs"
```

### 4. Configurar CodeQL

CodeQL está habilitado automaticamente. Para configuração avançada:

1. Vá para **Settings** → **Code security and analysis**
2. Em **Code scanning**, clique em **Set up** → **Advanced**
3. O workflow padrão será criado (já temos um customizado em `.github/workflows/code-quality.yml`)

### 5. Configurar Docker Hub (Opcional)

Se quiser publicar imagens Docker automaticamente:

#### Criar Token no Docker Hub

1. Acesse https://hub.docker.com/
2. Vá para **Account Settings** → **Security**
3. Clique em **New Access Token**
4. Dê um nome (ex: "GitHub Actions")
5. Copie o token

#### Adicionar Secrets

1. No repositório GitHub: **Settings** → **Secrets and variables** → **Actions**
2. Adicione dois secrets:

**DOCKER_USERNAME:**
- Nome: `DOCKER_USERNAME`
- Value: Seu username do Docker Hub

**DOCKER_PASSWORD:**
- Nome: `DOCKER_PASSWORD`
- Value: Token copiado

### 6. Configurar Labels do Repositório

Para o workflow de PR size labeling, crie as labels:

1. Vá para **Issues** → **Labels**
2. Crie as seguintes labels:

| Label | Cor | Descrição |
|-------|-----|-----------|
| `size/xs` | #3CBF00 | Extra Small PR (< 10 linhas) |
| `size/s` | #5D9801 | Small PR (< 100 linhas) |
| `size/m` | #7F7203 | Medium PR (< 500 linhas) |
| `size/l` | #A14C05 | Large PR (< 1000 linhas) |
| `size/xl` | #C32607 | Extra Large PR (> 1000 linhas) |

Outras labels úteis:

| Label | Cor | Descrição |
|-------|-----|-----------|
| `feat` | #0E8A16 | Nova funcionalidade |
| `fix` | #D73A4A | Correção de bug |
| `docs` | #0075CA | Documentação |
| `refactor` | #FBCA04 | Refatoração |
| `test` | #BFD4F2 | Testes |
| `ci` | #000000 | CI/CD |

## 🧪 Testando os Workflows

### Testar Localmente com Act

Instale o [Act](https://github.com/nektos/act):

```bash
# Windows (Chocolatey)
choco install act-cli

# macOS
brew install act

# Linux
curl https://raw.githubusercontent.com/nektos/act/master/install.sh | sudo bash
```

Execute workflows localmente:

```bash
# Testar workflow de CI
act -j build

# Testar workflow de PR
act pull_request

# Testar com secrets
act -s CODECOV_TOKEN=fake-token
```

### Testar no GitHub

#### Opção 1: Push para Feature Branch

```bash
git checkout -b test/ci-setup
git commit --allow-empty -m "test: trigger CI"
git push origin test/ci-setup
```

#### Opção 2: Executar Manualmente

1. Vá para **Actions**
2. Selecione o workflow "CI Pipeline"
3. Clique em **Run workflow**
4. Escolha a branch
5. Clique em **Run workflow**

## 📊 Monitoramento

### Ver Status dos Workflows

1. Vá para a aba **Actions** no GitHub
2. Veja histórico de execuções
3. Clique em uma execução para ver detalhes
4. Expanda jobs para ver logs

### Adicionar Badges ao README

Atualize o README.md principal:

```markdown
# Prisma Prime Market API

[![CI Pipeline](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/ci.yml/badge.svg)](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/ci.yml)
[![Code Quality](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/code-quality.yml/badge.svg)](https://github.com/jorelrx/PrismaPrimeMarketAPI/actions/workflows/code-quality.yml)
[![codecov](https://codecov.io/gh/USERNAME/PrismaPrimeMarketAPI/branch/main/graph/badge.svg)](https://codecov.io/gh/USERNAME/PrismaPrimeMarketAPI)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
```

Substitua `USERNAME` pelo seu usuário do GitHub.

## 🔍 Troubleshooting

### Workflows Falhando por Projetos Não Existentes

**Problema:** Build falha porque os projetos ainda não foram criados.

**Solução:**
1. Os workflows estão preparados para quando os projetos existirem
2. Alguns steps têm `continue-on-error: true` temporariamente
3. Quando criar os projetos, os workflows funcionarão normalmente

### CodeQL Timeout

**Problema:** Análise CodeQL demora muito ou dá timeout.

**Solução:**
1. CodeQL só roda em push para main/develop e PRs
2. Para acelerar, limite a análise:
   ```yaml
   - name: Initialize CodeQL
     uses: github/codeql-action/init@v3
     with:
       queries: +security-extended  # Em vez de security-and-quality
   ```

### Codecov Upload Falha

**Problema:** Upload de cobertura retorna erro 401.

**Solução:**
1. Verifique se o secret `CODECOV_TOKEN` está correto
2. Regenere o token no Codecov se necessário
3. Verifique se o repositório está ativo no Codecov

### PR Validation Falha em Conventional Commits

**Problema:** Commits não seguem o padrão.

**Solução:**
1. Use commits no formato: `tipo(escopo): mensagem`
2. Tipos aceitos: feat, fix, docs, style, refactor, perf, test, build, ci, chore, revert
3. Exemplo: `feat(products): adicionar endpoint de listagem`

## 📚 Recursos

- [GitHub Actions Documentation](https://docs.github.com/actions)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Codecov Documentation](https://docs.codecov.com/)
- [CodeQL Documentation](https://codeql.github.com/docs/)
- [Act - Local GitHub Actions](https://github.com/nektos/act)

## ✅ Checklist de Configuração

Use este checklist para garantir que tudo está configurado:

- [ ] GitHub Actions habilitado
- [ ] Branch protection configurado (main)
- [ ] Codecov token adicionado
- [ ] CodeQL habilitado
- [ ] Labels criadas
- [ ] Workflows testados
- [ ] Badges adicionados ao README
- [ ] Docker Hub configurado (opcional)
- [ ] Documentação revisada
- [ ] Time treinado sobre conventional commits

## 🎯 Próximos Passos

Após a configuração:

1. **Criar estrutura do projeto**
   - Criar projetos .NET (API, Application, Domain, Infrastructure)
   - Configurar Entity Framework
   - Implementar base entities

2. **Configurar Docker**
   - Criar Dockerfile
   - Criar docker-compose.yml
   - Testar build local

3. **Implementar testes**
   - Configurar projetos de teste
   - Adicionar testes unitários
   - Adicionar testes de integração

4. **Configurar deploy**
   - Configurar Azure/AWS
   - Adicionar workflow de deploy
   - Configurar ambientes (staging, production)

---

**Última atualização:** 2026-01-19
