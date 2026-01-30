# ✅ Checklist de Configuração do CI

Use este checklist para garantir que toda a configuração está completa.

---

## ✅ Pré-requisitos

- [ ] Acesso de Admin ao repositório GitHub
- [ ] Conta Docker Hub (para build de imagens)
- [ ] Codecov configurado (para cobertura de código)
- [ ] .NET 8 SDK instalado localmente
- [ ] **Node.js 18+** instalado (para validação local com Husky)

---

## 📋 Configuração do GitHub

### 1. GitHub Actions

- [ ] Ir em **Settings → Actions → General**
- [ ] Marcar **Allow all actions and reusable workflows**
- [ ] Marcar **Read and write permissions**
- [ ] Marcar **Allow GitHub Actions to create and approve pull requests**
- [ ] Clicar em **Save**

---

### 2. Branch Protection - `main`

- [ ] Ir em **Settings → Branches**
- [ ] Clicar em **Add branch protection rule**
- [ ] Branch name pattern: `main`

#### Require a pull request before merging
- [ ] Enabled
- [ ] Require approvals: 1 (ou mais)
- [ ] Dismiss stale pull request approvals when new commits are pushed
- [ ] Require review from Code Owners (opcional)

#### Require status checks to pass before merging
- [ ] Enabled
- [ ] Require branches to be up to date before merging

#### Status checks required (marcar todos):
- [ ] `validate-pr / Validar Título do PR`
- [ ] `validate-pr / Validar Commits`
- [ ] `build / Etapa 1: Testes em Docker`
- [ ] `build / Etapa 2: Build & Testes com Cobertura`
- [ ] `build / Etapa 3: Qualidade de Código`
- [ ] `build / Etapa 4: Análise Estática`
- [ ] `build / Etapa 5: Verificação Final`
- [ ] `codeql / CodeQL Analysis`

#### Outras configurações
- [ ] Require conversation resolution before merging
- [ ] Do not allow bypassing the above settings
- [ ] Restrict who can push to matching branches (opcional)

- [ ] Clicar em **Create**

---

### 3. Secrets

- [ ] Ir em **Settings → Secrets and variables → Actions**
- [ ] Clicar em **New repository secret**

#### Secrets obrigatórios:

**DOCKER_USERNAME**
- [ ] Nome: `DOCKER_USERNAME`
- [ ] Value: Seu usuário do Docker Hub
- [ ] Clicar em **Add secret**

**DOCKER_TOKEN**
- [ ] Obter token: https://hub.docker.com/ → Account Settings → Security → Access Tokens
- [ ] Nome: `DOCKER_TOKEN`
- [ ] Value: Token copiado
- [ ] Permissions: Read, Write, Delete
- [ ] Clicar em **Add secret**

#### Secrets opcionais:

**CODECOV_TOKEN**
- [ ] Obter token: https://codecov.io/
- [ ] Nome: `CODECOV_TOKEN`
- [ ] Value: Token do Codecov
- [ ] Clicar em **Add secret**

---

### 4. Labels

- [ ] Ir em **Issues → Labels**

#### Labels de tamanho do PR:
- [ ] `size/xs` - Cor: `#3CBF00` - "Extra Small PR (< 10 linhas)"
- [ ] `size/s` - Cor: `#5D9801` - "Small PR (< 100 linhas)"
- [ ] `size/m` - Cor: `#7F7203` - "Medium PR (< 500 linhas)"
- [ ] `size/l` - Cor: `#A14C05` - "Large PR (< 1000 linhas)"
- [ ] `size/xl` - Cor: `#C32607` - "Extra Large PR (> 1000 linhas)"

#### Labels de tipo (opcional mas recomendado):
- [ ] `feat` - Cor: `#0E8A16` - "Nova funcionalidade"
- [ ] `fix` - Cor: `#D73A4A` - "Correção de bug"
- [ ] `docs` - Cor: `#0075CA` - "Documentação"
- [ ] `refactor` - Cor: `#FBCA04` - "Refatoração"
- [ ] `test` - Cor: `#BFD4F2` - "Testes"
- [ ] `ci` - Cor: `#000000` - "CI/Workflows"
- [ ] `chore` - Cor: `#FEF2C0` - "Manutenção"

---

## 📁 Arquivos do Projeto

### Workflows (`.github/workflows/`)
- [ ] `pr-checks.yml` existe
- [ ] `ci.yml` existe
- [ ] `code-quality.yml` existe
- [ ] `docker-build.yml` existe
- [ ] `README.md` existe

### Configuração
- [ ] `.commitlintrc.json` existe na raiz

### Documentação (`docs/`)
- [ ] `CI_WORKFLOW_GUIDE.md` existe
- [ ] `CI_QUICK_REFERENCE.md` existe

---

## 🧪 Testes

### Teste 1: Push Direto na Main (Deve Falhar)

- [ ] Tentar fazer push direto na `main`
- [ ] Deve ser bloqueado com mensagem de erro
- [ ] ✅ **Esperado:** Push bloqueado

### Teste 2: Pull Request

- [ ] Criar branch: `git checkout -b test/ci-setup`
- [ ] Fazer commit: `git commit -m "test: configurar CI" --allow-empty`
- [ ] Push: `git push origin test/ci-setup`
- [ ] Abrir Pull Request no GitHub
- [ ] Verificar se workflows executaram:
  - [ ] PR Checks
  - [ ] CI Pipeline
  - [ ] Code Quality
- [ ] ✅ **Esperado:** Todos os workflows devem executar

### Teste 3: Validação de Conventional Commits

- [ ] Abrir PR com título inválido: "teste"
- [ ] ✅ **Esperado:** `validate-pr` deve falhar
- [ ] Corrigir título para: "test: configurar CI"
- [ ] ✅ **Esperado:** `validate-pr` deve passar

### Teste 4: Merge e Docker Build

- [ ] Aprovar PR
- [ ] Fazer merge (se branch protection permitir teste)
- [ ] Verificar workflow `docker-build.yml` executou
- [ ] ✅ **Esperado:** Imagem deve ser enviada para Docker Hub

---

## 🔍 Verificações Finais

### GitHub Actions
- [ ] Workflows aparecem em **Actions** tab
- [ ] Histórico de execuções está visível
- [ ] Logs são acessíveis

### Branch Protection
- [ ] Push direto na `main` é bloqueado
- [ ] PR sem aprovação não pode fazer merge
- [ ] PR com checks falhando não pode fazer merge
- [ ] Status checks aparecem no PR

### Docker Hub
- [ ] Login funciona com os secrets configurados
- [ ] Repositório existe (ou será criado no primeiro push)
- [ ] Imagem foi enviada com sucesso

### Documentação
- [ ] README do projeto menciona o CI
- [ ] Badges dos workflows estão no README (opcional)
- [ ] Desenvolvedores sabem onde encontrar a documentação

---

## 📚 Documentação Lida

- [ ] Li o [CI_WORKFLOW_GUIDE.md](CI_WORKFLOW_GUIDE.md)
- [ ] Li o [CI_QUICK_REFERENCE.md](CI_QUICK_REFERENCE.md)
- [ ] Li o [.github/workflows/README.md](../.github/workflows/README.md)
- [ ] Entendi como fazer commits (Conventional Commits)
- [ ] Entendi o fluxo de trabalho (branch → commit → push → PR → merge)
- [ ] Sei onde buscar ajuda

---

## 🛡️ Etapa 7: Configurar Validação Local (Husky + Commitlint)

### 7.1. Instalação Local

Cada desenvolvedor precisa executar (uma vez):

```bash
# No diretório raiz do projeto
npm install
npm run prepare
```

**Windows**: Funciona automaticamente com Git Bash.

**Linux/Mac**: Tornar hooks executáveis:
```bash
chmod +x .husky/commit-msg
chmod +x .husky/pre-commit
chmod +x .husky/pre-push
```

### 7.2. Testar Validação de Commit

```bash
# ❌ Deve bloquear
git commit -m "atualizando codigo"

# ✅ Deve passar
git commit -m "test: valida commit local"
```

### 7.3. Testar Validação de Push

```bash
# Criar branch de teste
git checkout -b test/validacao-local

# Commit válido
git commit --allow-empty -m "test: valida push com testes"

# Push (vai rodar build + testes locais antes)
git push origin test/validacao-local
```

**Resultado esperado**: 
- Build executado
- Testes executados
- Push liberado se tudo passar ✅
- Push bloqueado se algo falhar ❌

### 7.4. Documentação Completa

- [ ] Leitura de [LOCAL_VALIDATION_SETUP.md](LOCAL_VALIDATION_SETUP.md)
- [ ] Entendi como funciona o bloqueio de commits/push
- [ ] Sei como fazer bypass em emergências (somente!)
- [ ] Compartilhei com todo o time

---

## ✅ Conclusão

Se todos os itens acima estiverem marcados:

🎉 **Parabéns! Seu CI está completamente configurado!**

Você tem agora:
- ✅ Validações automáticas no GitHub Actions
- ✅ Branch protection configurado
- ✅ Docker builds automatizados
- ✅ **Validação local de commits e testes**

Você pode começar a trabalhar no projeto seguindo o fluxo:
1. Criar branch
2. Fazer commits (validação automática de mensagem)
3. Push da branch (testes rodados automaticamente)
4. Abrir Pull Request
5. Aguardar validações e aprovação
6. Merge

---

## 🆘 Problemas?

Se algo não funcionar:

1. **Validação local**: Consulte [LOCAL_VALIDATION_SETUP.md](LOCAL_VALIDATION_SETUP.md) seção Troubleshooting
2. **CI no GitHub**: Verifique os logs dos workflows em **Actions**
3. **Workflow geral**: Consulte [CI_WORKFLOW_GUIDE.md](CI_WORKFLOW_GUIDE.md) seção Troubleshooting
4. **Secrets**: Certifique-se de que todos os secrets estão configurados corretamente
5. **Workflows**: Verifique se todos os arquivos existem em `.github/workflows/`

---

**Data de configuração:** __________  
**Configurado por:** __________  
**Revisado por:** __________

---

**Última atualização:** Janeiro 2026
