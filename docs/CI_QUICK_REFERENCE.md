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

### 2. Fazer Commits (Conventional Commits)

**IMPORTANTE:** Commits fora da convenção serão **BLOQUEADOS AUTOMATICAMENTE** pelo commitlint local!

```bash
# ✅ Features (aceito)
git commit -m "feat: adicionar endpoint de produtos"
git commit -m "feat(auth): implementar autenticação JWT"

# ✅ Bug Fixes (aceito)
git commit -m "fix: corrigir validação de email"
git commit -m "fix(orders): resolver cálculo de frete"

# ✅ Docs (aceito)
git commit -m "docs: atualizar README"
git commit -m "docs(api): adicionar documentação de endpoints"

# ✅ Refactor (aceito)
git commit -m "refactor: reorganizar estrutura de pastas"

# ✅ Tests (aceito)
git commit -m "test: adicionar testes de integração"

# ✅ CI (aceito)
git commit -m "ci: atualizar workflow de build"

# ✅ Chore (aceito)
git commit -m "chore: atualizar dependências"

# ❌ Inválido (BLOQUEADO)
git commit -m "adicionando nova feature"
# Erro: subject may not be empty [subject-empty]

# 🚨 Bypass (somente emergência!)
git commit -m "qualquer mensagem" --no-verify
```

### 3. Push e Pull Request

**IMPORTANTE:** Push será **BLOQUEADO** se build ou testes falharem localmente!

```bash
# Push da branch (roda build + testes automaticamente)
git push origin feat/nome-da-feature

# Saída esperada:
# 🧪 Executando testes locais antes do push...
# ⏳ Buildando o projeto...
# ✅ Build concluído com sucesso!
# ⏳ Executando testes...
# ✅ Todos os testes passaram!
# 🚀 Push liberado!

# 🚨 Bypass (somente emergência!)
git push origin feat/nome-da-feature --no-verify

# Abrir PR no GitHub
# Título do PR também deve seguir Conventional Commits!
# Exemplo: "feat: adicionar funcionalidade X"
```

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
git commit -m "style: corrigir formatação"
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
# ✅ Usar formato correto
git commit -m "feat: adiciona nova feature"

# Tipos válidos: feat, fix, docs, style, refactor, perf, test, build, ci, chore

# 🚨 Bypass (somente emergência)
git commit -m "mensagem qualquer" --no-verify
```

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

## 🎯 Conventional Commits - Tipos

| Tipo | Descrição | Exemplo |
|------|-----------|---------|
| `feat` | Nova funcionalidade | `feat: adicionar login social` |
| `fix` | Correção de bug | `fix: resolver erro de timeout` |
| `docs` | Documentação | `docs: atualizar README` |
| `style` | Formatação | `style: corrigir indentação` |
| `refactor` | Refatoração | `refactor: extrair método comum` |
| `perf` | Performance | `perf: otimizar query SQL` |
| `test` | Testes | `test: adicionar teste unitário` |
| `build` | Build system | `build: atualizar dependências` |
| `ci` | CI/Workflows | `ci: adicionar workflow de deploy` |
| `chore` | Manutenção | `chore: limpar código morto` |
| `revert` | Reverter commit | `revert: desfazer mudança X` |

---

**Última atualização:** Janeiro 2026
