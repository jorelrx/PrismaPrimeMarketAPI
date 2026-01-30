# 🚀 Quick Reference - CI Workflows

Referência rápida para trabalhar com o pipeline de CI do projeto.

---

## 📋 Comandos Úteis

### Setup Inicial (Validação Local - Uma vez por desenvolvedor)

```bash
# Instalar Husky e commitlint
npm install

# Configurar Git hooks
npm run prepare

# Verificar instalação
npx husky --version
npx commitlint --version
```

### Validação Local (antes do push)

```bash
# Verificar formatação
dotnet format --verify-no-changes

# Corrigir formatação
dotnet format

# Build
dotnet build

# Testes
dotnet test

# Testes com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Verificar vulnerabilidades
dotnet list package --vulnerable --include-transitive

# Verificar deprecados
dotnet list package --deprecated --include-transitive
```

---

## 🔄 Fluxo de Trabalho

### 1. Criar Feature Branch

```bash
git checkout main
git pull origin main
git checkout -b feat/nome-da-feature
```

### 2. Fazer Commits

**IMPORTANTE:** Siga as convenções de commit definidas em [CONTRIBUTING.md](CONTRIBUTING.md#conventional-commits)

```bash
# Fazer commit (será validado automaticamente)
git add .
git commit -m "feat: Adicionar endpoint de produtos"

# Consulte CONTRIBUTING.md para:
# - Formato correto de commits (Conventional Commits)
# - Exemplos válidos e inválidos
# - Tipos de commits aceitos
# - Regras de validação
```

### 3. Push e Pull Request

```bash
# Push da branch (validações automáticas executam)
git push origin feat/nome-da-feature

# Abrir PR no GitHub
# Consulte CONTRIBUTING.md para regras de PR e títulos
```

**Validações automáticas no push:**
- Build do projeto
- Execução de todos os testes
- Bloqueio se falhar

**Detalhes completos:** [CONTRIBUTING.md](CONTRIBUTING.md#push)

### 4. Após Aprovação

```bash
# Merge no GitHub (Squash and Merge recomendado)
# Deletar branch após merge
git checkout main
git pull origin main
git branch -d feat/nome-da-feature
```

---

## ✅ Workflows Executados

### Em Pull Request

1. **PR Checks** (`pr-checks.yml`)
   - Valida título do PR
   - Valida commits
   - Adiciona label de tamanho

2. **CI Pipeline** (`ci.yml`)
   - Etapa 1: Testes em Docker
   - Etapa 2: Build & Testes com Cobertura
   - Etapa 3: Qualidade de Código
   - Etapa 4: Análise Estática
   - Etapa 5: Verificação Final

3. **Code Quality** (`code-quality.yml`)
   - CodeQL Analysis
   - Format Check
   - Code Metrics

### Após Merge em main/develop

4. **Docker Build & Push** (`docker-build.yml`)
   - Build da imagem Docker
   - Push para Docker Hub

---

## 🚫 Regras de Branch Protection

### Branch `main`

- ❌ **Push direto bloqueado**
- ✅ **Requer Pull Request**
- ✅ **Requer 1+ aprovações**
- ✅ **Requer todos os status checks**
- ✅ **Branch deve estar atualizada**
- ✅ **Conversas devem estar resolvidas**

### Status Checks Obrigatórios

- Validar Título do PR
- Validar Commits
- Etapa 1: Testes em Docker
- Etapa 2: Build & Testes com Cobertura
- Etapa 3: Qualidade de Código
- Etapa 4: Análise Estática
- Etapa 5: Verificação Final
- CodeQL Analysis

---

## 🐳 Docker

### Usar Imagem do Docker Hub

```bash
# Latest (main branch)
docker pull <seu-usuario>/prismaprime-market-api:latest

# Develop branch
docker pull <seu-usuario>/prismaprime-market-api:develop

# Rodar container
docker run -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ConnectionStrings__DefaultConnection="sua-connection-string" \
  <seu-usuario>/prismaprime-market-api:latest
```

### Build Local

```bash
# Build
docker build -t prismaprime-market-api:local .

# Run
docker run -p 8080:8080 prismaprime-market-api:local

# Docker Compose (desenvolvimento)
docker-compose up -d

# Docker Compose (testes)
docker-compose -f docker-compose.test.yml up --build --abort-on-container-exit
```

---

## 🔍 Troubleshooting

### Workflow Falhou

**Build failed:**
```bash
# Testar localmente
dotnet restore
dotnet build
```

**Tests failed:**
```bash
# Rodar testes localmente
dotnet test --verbosity detailed
```

**Format check failed:**
```bash
# Corrigir formatação
dotnet format
git add .
git commit -m "style: Corrigir formatação"
git push
```

### Validação Local (Husky/Commitlint)

**"husky command not found":**
```bash
# Instalar Node.js 18+ de https://nodejs.org/
# Depois executar:
npm install
npm run prepare
```

**Commit bloqueado:**
```bash
# ✅ Consulte CONTRIBUTING.md para formato correto
# Exemplo: feat: Adicionar nova feature

# 🚨 Bypass (somente emergência)
git commit -m "mensagem qualquer" --no-verify
```

**Regras completas de commits:** [CONTRIBUTING.md](CONTRIBUTING.md#conventional-commits)

**Push bloqueado por testes:**
```bash
# Ver qual teste falhou
dotnet test --verbosity detailed

# Corrigir teste e tentar novamente
# Ou bypass (emergência):
git push --no-verify
```

**Hooks demoram muito:**
```bash
# Editar .husky/pre-push
# Rodar só testes rápidos:
dotnet test --filter "Category!=Integration" --no-build
```

### PR Bloqueado

Se status checks não aparecerem:
1. Certifique-se de que os workflows existem em `.github/workflows/`
2. Verifique se a branch protection está configurada corretamente
3. Force um novo push: `git commit --amend --no-edit && git push --force-with-lease`

---

## 📊 Monitoramento

### GitHub Actions

```bash
# Via CLI (gh cli)
gh workflow list
gh workflow view ci.yml
gh run list --workflow=ci.yml
gh run view
gh run watch
```

### Badges para README

```markdown
[![CI Pipeline](https://github.com/seu-usuario/PrismaPrimeMarketAPI/actions/workflows/ci.yml/badge.svg)](https://github.com/seu-usuario/PrismaPrimeMarketAPI/actions/workflows/ci.yml)

[![PR Checks](https://github.com/seu-usuario/PrismaPrimeMarketAPI/actions/workflows/pr-checks.yml/badge.svg)](https://github.com/seu-usuario/PrismaPrimeMarketAPI/actions/workflows/pr-checks.yml)

[![Code Quality](https://github.com/seu-usuario/PrismaPrimeMarketAPI/actions/workflows/code-quality.yml/badge.svg)](https://github.com/seu-usuario/PrismaPrimeMarketAPI/actions/workflows/code-quality.yml)

[![Docker](https://img.shields.io/docker/v/seu-usuario/prismaprime-market-api?label=docker%20hub)](https://hub.docker.com/r/seu-usuario/prismaprime-market-api)
```

---

## 🔐 Secrets Necessários

Configure em **Settings → Secrets and variables → Actions**:

| Secret | Descrição | Como obter |
|--------|-----------|------------|
| `DOCKER_USERNAME` | Usuário Docker Hub | Seu username |
| `DOCKER_TOKEN` | Token Docker Hub | Account Settings → Security → Access Tokens |
| `CODECOV_TOKEN` | Token Codecov (opcional) | https://codecov.io/ |

---

## 📚 Documentação Relacionada

### Guias Completos

- **[CONTRIBUTING.md](CONTRIBUTING.md)** - Guia completo de contribuição
  - Conventional Commits (formato, exemplos, regras)
  - Tipos de contribuições
  - Processo passo a passo
  - Boas práticas de commits e PRs
  - Troubleshooting detalhado

- **[CI_WORKFLOW_GUIDE.md](CI_WORKFLOW_GUIDE.md)** - Detalhes dos workflows
  - Descrição detalhada de cada workflow
  - Configuração e customização
  - Entendimento técnico dos pipelines

- **[CI_SETUP_CHECKLIST.md](CI_SETUP_CHECKLIST.md)** - Setup do CI/CD
  - Configuração inicial do repositório
  - Branch protection rules
  - Secrets e configurações

---

**Última atualização:** Janeiro 2026
