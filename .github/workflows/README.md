# GitHub Actions Workflows

Este diretório contém os workflows de CI (Integração Contínua) do projeto.

---

## 📋 Workflows Disponíveis

### 1. `pr-checks.yml` - Validação de Pull Requests

**Quando executa:**
- Ao abrir um Pull Request
- Ao atualizar um Pull Request (novo push)
- Ao reabrir um Pull Request

**Jobs:**
- **validate-pr**: Valida título, commits e adiciona labels

**Status checks criados:**
- `Validar Título do PR`
- `Validar Commits`

---

### 2. `ci.yml` - Pipeline de CI

**Quando executa:**
- Em Pull Requests para `main` ou `develop`
- Em push para `develop`
- Manualmente

**Jobs (executam em sequência):**
1. **test-docker**: Testes em ambiente Docker
2. **build**: Build e testes com cobertura
3. **code-quality**: Verificação de qualidade
4. **analyze**: Análise estática
5. **build-status**: Verificação final

**Status checks criados:**
- `Etapa 1: Testes em Docker`
- `Etapa 2: Build & Testes com Cobertura`
- `Etapa 3: Qualidade de Código`
- `Etapa 4: Análise Estática`
- `Etapa 5: Verificação Final`

---

### 3. `code-quality.yml` - Qualidade e Segurança

**Quando executa:**
- Em Pull Requests para `main` ou `develop`
- Em push para `main` ou `develop`
- Semanalmente (segundas às 9h UTC)
- Manualmente

**Jobs:**
- **codeql**: Análise de segurança com CodeQL
- **format-check**: Verificação de formatação
- **code-metrics**: Métricas e scan de vulnerabilidades

**Status checks criados:**
- `CodeQL Analysis`
- `Format Check`
- `Code Metrics`

---

### 4. `docker-build.yml` - Build e Push Docker

**Quando executa:**
- Apenas em push para `main` ou `develop`
- Não executa em Pull Requests

**Jobs:**
- **docker**: Build da imagem e push para Docker Hub

**Tags geradas:**
- Push para `main`: `latest`, `main`, `main-<sha>`
- Push para `develop`: `develop`, `develop-<sha>`

---

## 🔄 Fluxo de Execução

### Em Pull Request

```
PR aberto/atualizado
    ↓
PR Checks (validações)
    ↓
CI Pipeline (5 etapas sequenciais)
    ↓
Code Quality (análise)
    ↓
Aguarda aprovação
    ↓
Merge aprovado
```

### Após Merge

```
Merge em main/develop
    ↓
Docker Build & Push
    ↓
Imagem disponível no Docker Hub
```

---

## ✅ Status Checks Obrigatórios

Configure em **Settings → Branches → Branch protection rules**:

Para branch `main`, adicione estes status checks:

- `validate-pr / Validar Título do PR`
- `validate-pr / Validar Commits`
- `build / Etapa 1: Testes em Docker`
- `build / Etapa 2: Build & Testes com Cobertura`
- `build / Etapa 3: Qualidade de Código`
- `build / Etapa 4: Análise Estática`
- `build / Etapa 5: Verificação Final`
- `codeql / CodeQL Analysis`

---

## 🔐 Secrets Necessários

Configure em **Settings → Secrets and variables → Actions**:

| Secret | Obrigatório | Descrição |
|--------|-------------|-----------|
| `DOCKER_USERNAME` | Sim | Usuário do Docker Hub |
| `DOCKER_TOKEN` | Sim | Access Token do Docker Hub |
| `CODECOV_TOKEN` | Não | Token do Codecov (cobertura) |

---

## 🛠️ Manutenção

### Atualizar versões de actions

Periodicamente, atualize as versões das actions usadas:

```yaml
# Exemplo
uses: actions/checkout@v4  # Verificar se há v5
uses: actions/setup-dotnet@v4  # Verificar updates
```

### Testar workflows localmente

Use o [Act](https://github.com/nektos/act):

```bash
# Testar PR checks
act pull_request -W .github/workflows/pr-checks.yml

# Testar CI
act push -W .github/workflows/ci.yml

# Testar Code Quality
act push -W .github/workflows/code-quality.yml
```

---

## 📚 Documentação

Para mais informações, consulte:
- [CI_WORKFLOW_GUIDE.md](../../docs/CI_WORKFLOW_GUIDE.md) - Guia completo
- [CI_QUICK_REFERENCE.md](../../docs/CI_QUICK_REFERENCE.md) - Referência rápida

---

**Última atualização:** Janeiro 2026
