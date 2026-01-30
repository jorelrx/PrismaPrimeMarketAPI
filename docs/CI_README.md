# 📚 Documentação do CI

Bem-vindo à documentação do sistema de Integração Contínua (CI) do Prisma Prime Market API.

---

## 📖 Documentos Disponíveis

### 🚀 Para Começar

**[CI_WORKFLOW_GUIDE.md](CI_WORKFLOW_GUIDE.md)** - **LEIA PRIMEIRO!**
- Guia completo e detalhado
- Configuração do GitHub passo a passo
- Explicação de todos os workflows
- Fluxo de trabalho completo
- Troubleshooting

**[CI_SETUP_CHECKLIST.md](CI_SETUP_CHECKLIST.md)** - **Use para configurar**
- Checklist completo de configuração
- Verificação de cada etapa
- Testes de validação
- Confirmação de que tudo está funcionando

**[LOCAL_VALIDATION_SETUP.md](LOCAL_VALIDATION_SETUP.md)** - **Validação local**
- Bloqueio de commits fora da convenção
- Bloqueio de push se testes falharem
- Instalação do Husky e commitlint
- Troubleshooting e bypass de emergência

### ⚡ Referência Rápida

**[CI_QUICK_REFERENCE.md](CI_QUICK_REFERENCE.md)** - **Consulta diária**
- Comandos úteis
- Conventional Commits
- Fluxo de trabalho resumido
- Troubleshooting rápido
- Badges e monitoramento

---

## 🎯 Por Onde Começar?

### Se você é novo no projeto:

1. **Leia:** [CI_WORKFLOW_GUIDE.md](CI_WORKFLOW_GUIDE.md)
   - Entenda como funciona o CI
   - Aprenda o fluxo de trabalho
   - Veja exemplos práticos

2. **Configure:** Use [CI_SETUP_CHECKLIST.md](CI_SETUP_CHECKLIST.md)
   - Siga o checklist passo a passo
   - Marque cada item concluído
   - Valide se tudo funciona

3. **Use:** Tenha [CI_QUICK_REFERENCE.md](CI_QUICK_REFERENCE.md) à mão
   - Consulte quando precisar
   - Comandos prontos para copiar/colar
   - Referência rápida de conventional commits

### Se você já conhece o projeto:

- Use [CI_QUICK_REFERENCE.md](CI_QUICK_REFERENCE.md) para consultas rápidas
- Consulte [CI_WORKFLOW_GUIDE.md](CI_WORKFLOW_GUIDE.md) quando tiver dúvidas

---

## 📋 Resumo do Sistema de CI

### O que é CI?

**Integração Contínua (CI)** é a prática de automatizar testes e validações sempre que código é enviado ao repositório. No nosso projeto:

- ✅ **Impede push direto na `main`**
- ✅ **Todo código passa por Pull Request**
- ✅ **Validações automáticas em cada PR**
- ✅ **Build de Docker automático após merge**

### Workflows Principais

1. **PR Checks** - Valida título e commits
2. **CI Pipeline** - 5 etapas de testes e validações
3. **Code Quality** - Análise de segurança e qualidade
4. **Docker Build** - Cria e publica imagem Docker

### Fluxo Simplificado

```
Criar branch → Commits → Push → PR → Validações → Aprovação → Merge → Docker
```

---

## 🔧 Configuração Necessária

### No GitHub

1. Habilitar GitHub Actions
2. Configurar Branch Protection na `main`
3. Adicionar Secrets (Docker Hub)
4. Criar Labels de tamanho

### No Docker Hub

1. Criar conta
2. Gerar Access Token
3. Adicionar token nos secrets do GitHub

### Documentação Completa

Veja todos os detalhes em [CI_WORKFLOW_GUIDE.md](CI_WORKFLOW_GUIDE.md)

---

## ✅ Regras Principais

### Branch `main`

- ❌ Push direto **BLOQUEADO**
- ✅ Apenas via Pull Request
## 🎯 Convenções de Commits e PRs

### Commits

- ✅ Devem seguir **Conventional Commits**
- ✅ Formato: `tipo: Descrição começando com maiúscula`
- ✅ Tipos: feat, fix, docs, style, refactor, perf, test, build, ci, chore

**Guia completo:** [CONTRIBUTING.md](CONTRIBUTING.md#conventional-commits)

### Pull Requests

- ✅ Título segue Conventional Commits
- ✅ Todos os workflows devem passar
- ✅ Pelo menos 1 aprovação
- ✅ Conversas resolvidas

**Guia completo:** [CONTRIBUTING.md](CONTRIBUTING.md#pull-requests)

---

## 🧪 Como Trabalhar no Projeto

### 1. Criar Branch

```bash
git checkout main
git pull origin main
git checkout -b feat/minha-feature
```

### 2. Fazer Commits

```bash
git add .
git commit -m "feat: Adicionar nova funcionalidade"
```

**Consulte:** [CONTRIBUTING.md](CONTRIBUTING.md#conventional-commits) para regras completas

### 3. Push e PR

```bash
git push origin feat/minha-feature
# Abrir PR no GitHub
```

### 4. Aguardar Validações

- Workflows executam automaticamente
- Corrija se algo falhar
- Aguarde aprovação

### 5. Merge

- Clique em "Merge pull request"
- Delete a branch
- Imagem Docker é criada automaticamente

**Detalhes completos em:** [CI_WORKFLOW_GUIDE.md](CI_WORKFLOW_GUIDE.md)

---

## 🚨 Troubleshooting

### Workflow Falhou

1. Acesse **Actions** no GitHub
2. Clique no workflow que falhou
3. Leia o log de erro
4. Consulte a seção **Troubleshooting** em [CI_WORKFLOW_GUIDE.md](CI_WORKFLOW_GUIDE.md)

### Problemas Comuns

- **Build failed**: Verifique se o código compila localmente
- **Tests failed**: Rode os testes localmente
- **Format check failed**: Execute `dotnet format`
- **Conventional commits failed**: Consulte [CONTRIBUTING.md](CONTRIBUTING.md#conventional-commits)

**Mais soluções em:** [CI_WORKFLOW_GUIDE.md](CI_WORKFLOW_GUIDE.md#-troubleshooting)

---

## 📊 Monitoramento

### GitHub Actions

- **Actions** tab → Ver todos os workflows
- Clique em um workflow → Ver execuções
- Clique em uma execução → Ver logs detalhados

### Docker Hub

- https://hub.docker.com/r/jorelrx/prismaprime-market-api
- Ver todas as tags/versões publicadas

---

## 📞 Ajuda

### Onde Buscar Informações

1. **Primeiro**: [CI_QUICK_REFERENCE.md](CI_QUICK_REFERENCE.md)
2. **Detalhes**: [CI_WORKFLOW_GUIDE.md](CI_WORKFLOW_GUIDE.md)
3. **Configuração**: [CI_SETUP_CHECKLIST.md](CI_SETUP_CHECKLIST.md)
4. **Workflows**: [../.github/workflows/README.md](../.github/workflows/README.md)

### Links Úteis

- [GitHub Actions Docs](https://docs.github.com/actions)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Docker Hub](https://hub.docker.com/)

---

## ✨ Recursos Adicionais

### Workflows

Todos os workflows estão em `.github/workflows/`:
- `pr-checks.yml`
- `ci.yml`
- `code-quality.yml`
- `docker-build.yml`

**README dos workflows:** [../.github/workflows/README.md](../.github/workflows/README.md)

### Configuração

- `.commitlintrc.json` - Configuração de validação de commits

---

**Última atualização:** Janeiro 2026  
**Versão da documentação:** 1.0

---

## 🎉 Comece Agora!

1. 📖 Leia o [CI_WORKFLOW_GUIDE.md](CI_WORKFLOW_GUIDE.md)
2. ✅ Use o [CI_SETUP_CHECKLIST.md](CI_SETUP_CHECKLIST.md)
3. 🚀 Comece a codar!
