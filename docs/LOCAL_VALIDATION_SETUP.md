# 🛡️ Configuração de Validação Local

## 📋 Índice
- [Visão Geral](#visão-geral)
- [Pré-requisitos](#pré-requisitos)
- [Instalação](#instalação)
- [Como Funciona](#como-funciona)
- [Testando o Setup](#testando-o-setup)
- [Troubleshooting](#troubleshooting)
- [Bypass (Emergência)](#bypass-emergência)

---

## 🎯 Visão Geral

Este projeto possui **validação local automática** para garantir qualidade do código **antes** que ele chegue ao repositório remoto:

### 🚫 Bloqueios Implementados

1. **Commit Bloqueado** se a mensagem não seguir convenção
   - ✅ `feat: Adicionar novo recurso`
   - ✅ `fix: Corrigir bug crítico`
   - ❌ `adicionei uma feature` (BLOQUEADO)
   
   **Regras completas:** [CONTRIBUTING.md](CONTRIBUTING.md#conventional-commits)

2. **Push Bloqueado** se os testes falharem
   - Build completo executado
   - Todos os testes unitários e de integração executados
   - Push só acontece se tudo passar

### 🎁 Benefícios

- **Código limpo no histórico**: Apenas commits com mensagens padronizadas
- **Menos CI quebrado**: Testes rodados localmente antes do push
- **Feedback rápido**: Erros detectados em segundos, não minutos
- **Economia de recursos**: Menos builds desperdiçados no CI

---

## 📦 Pré-requisitos

### Obrigatório
- **Node.js 18+**
- **npm**
- **.NET 8 SDK**
- **Git**

### Verificar Instalação
```bash
# Verificar Node.js
node --version  # Deve mostrar v18.x.x ou superior

# Verificar npm
npm --version   # Deve mostrar 9.x.x ou superior

# Verificar .NET
dotnet --version # Deve mostrar 8.0.x
```

---

## 🚀 Instalação

### Passo 1: Instalar Dependências Node

No diretório raiz do projeto:

```bash
npm install
```

Isso instalará:
- `husky` - Para gerenciar Git hooks
- `@commitlint/cli` - Para validar mensagens de commit
- `@commitlint/config-conventional` - Regras de convenção

### Passo 2: Inicializar Husky

```bash
npm run prepare
```

Isso criará a pasta `.husky/` com os hooks configurados.

### Passo 3: Tornar Hooks Executáveis (Linux/Mac)

```bash
chmod +x .husky/commit-msg
chmod +x .husky/pre-commit
chmod +x .husky/pre-push
```

**Windows**: Não é necessário, o Git Bash executa automaticamente.

### Passo 4: Verificar Instalação

```bash
# Verificar se husky está instalado
npx husky --version

# Verificar se commitlint está instalado
npx commitlint --version
```

---

## ⚙️ Como Funciona

### 1️⃣ Hook `commit-msg` (Validação com Commitlint)

**Quando**: Toda vez que você faz `git commit`

**O que faz**:
1. Captura a mensagem de commit
2. Valida usando **commitlint** com regras do `.commitlintrc.json`
3. **Bloqueia** se não estiver no formato Conventional Commits

**Exemplo de Uso**:
```bash
# ❌ Commit bloqueado
git commit -m "corrigindo bug"
# Erro: subject may not be empty [subject-empty]

# ✅ Commit aceito
git commit -m "fix: corrige erro de validação no login"
```

### 2️⃣ Hook `pre-push` (Validação de Testes)

**Quando**: Toda vez que você faz `git push`

**O que faz**:
1. Executa `dotnet build` em Release
2. Executa `dotnet test` com todos os testes
3. **Bloqueia push** se build ou testes falharem
4. **Libera push** se tudo passar

**Exemplo de Uso**:
```bash
git push origin feature/nova-funcionalidade

# Saída:
# 🧪 Executando testes locais antes do push...
# ⏳ Buildando o projeto...
# ✅ Build concluído com sucesso!
# ⏳ Executando testes...
# ✅ Todos os testes passaram!
# 🚀 Push liberado!
```

---

## 🧪 Testando o Setup

### Teste 1: Validação de Commit

```bash
# Teste mensagem inválida (deve bloquear)
git commit -m "atualizando codigo"

# Saída esperada:
# ⧗   input: atualizando codigo
# ✖   subject may not be empty [subject-empty]
# ✖   type may not be empty [type-empty]

# Teste mensagem válida (deve passar)
git commit -m "test: adiciona teste de validação local"

# Saída esperada:
# [feature/local-validation abc1234] test: adiciona teste de validação local
```

### Teste 2: Validação de Push

```bash
# Criar branch de teste
git checkout -b test/local-validation

# Fazer commit válido
git commit --allow-empty -m "test: valida push local"

# Tentar push (vai rodar testes)
git push origin test/local-validation

# Saída esperada:
# 🧪 Executando testes locais antes do push...
# ⏳ Buildando o projeto...
# ✅ Build concluído com sucesso!
# ⏳ Executando testes...
# ✅ Todos os testes passaram!
# 🚀 Push liberado!
```

### Teste 3: Simular Falha de Teste

```bash
# 1. Criar um teste que falha propositalmente
# 2. Fazer commit: git commit -m "test: teste temporário com falha"
# 3. Tentar push: git push

# Saída esperada:
# ❌ Testes falharam! Push bloqueado.
# 💡 Corrija os testes antes de fazer push.
# error: failed to push some refs
```

---

## 🔧 Troubleshooting

### Problema: "husky - command not found"

**Causa**: Node.js não instalado ou npm não configurado corretamente.

**Solução**:
```bash
# Instalar Node.js (Windows)
winget install OpenJS.NodeJS

# Ou baixar de https://nodejs.org/

# Depois:
npm install
npm run prepare
```

### Problema: "Permission denied" nos hooks (Linux/Mac)

**Causa**: Hooks não têm permissão de execução.

**Solução**:
```bash
chmod +x .husky/commit-msg
chmod +x .husky/pre-commit
chmod +x .husky/pre-push
```

### Problema: Testes passam localmente mas falham no CI

**Causa**: Diferenças de ambiente (banco de dados, configurações, etc.)

**Solução**:
1. Verificar se `.env` ou `appsettings.Development.json` estão corretos
2. Rodar testes com Docker: `docker-compose -f docker-compose.test.yml up --abort-on-container-exit`
3. Verificar logs do CI para diferenças específicas

### Problema: Hook demora muito (mais de 2 minutos)

**Causa**: Muitos testes ou projeto muito grande.

**Solução**:
```bash
# Opção 1: Rodar apenas testes rápidos no pre-push
# Editar .husky/pre-push e adicionar filtro:
dotnet test --filter "Category!=Integration" --no-build

# Opção 2: Desabilitar temporariamente (ver seção Bypass)
```

### Problema: "npx: command not found" no Windows

**Causa**: npm não está no PATH.

**Solução**:
```bash
# Reiniciar terminal após instalar Node.js
# Ou adicionar npm ao PATH manualmente:
# C:\Program Files\nodejs\
```

---

## 🚨 Bypass (Emergência)

### ⚠️ Quando Usar Bypass

**Apenas em situações EXTREMAS**:
- Hotfix crítico em produção
- CI está quebrado e precisa push para consertar
- Testes com falso-positivo bloqueando deploy

**NÃO USE** para:
- "Não quero esperar os testes"
- "Vou corrigir depois"
- "É só um commit rápido"

### Como Fazer Bypass

#### Bypass de Commit (mensagem)
```bash
# Adiciona --no-verify para pular hook
git commit -m "mensagem qualquer" --no-verify
```

#### Bypass de Push (testes)
```bash
# Adiciona --no-verify para pular hook
git push origin feature/minha-branch --no-verify
```

#### Desabilitar Hooks Temporariamente
```bash
# Mover hooks para backup
mv .husky .husky.backup

# Fazer commits/pushes

# Restaurar hooks
mv .husky.backup .husky
```

### 📝 Regras de Bypass

1. **Documente o motivo**: No commit ou PR, explique por que usou bypass
2. **Avise o time**: Em canal de comunicação (Slack, Teams, etc.)
3. **Corrija depois**: Crie issue para corrigir o problema que causou bypass
4. **Não vire hábito**: Bypass frequente indica problema no processo

---

## 📊 Estatísticas e Métricas

### Tempo Médio de Execução

| Operação | Tempo Estimado |
|----------|----------------|
| Validação de commit | < 1 segundo |
| Build (Release) | 10-30 segundos |
| Testes completos | 30-120 segundos |
| **Total (push)** | **40-150 segundos** |

### Comparação: Local vs CI

| Aspecto | Validação Local | Apenas CI |
|---------|----------------|-----------|
| Feedback | 1-2 minutos | 5-10 minutos |
| Custo | Zero | Compute time |
| Histórico | Limpo | Commits de "fix CI" |
| Experiência | ⚡ Rápida | 🐌 Lenta |

---

## 🔗 Referências

- [Commitlint](https://commitlint.js.org/)
- [Husky](https://typicode.github.io/husky/)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Git Hooks](https://git-scm.com/book/en/v2/Customizing-Git-Git-Hooks)

---

## 📞 Suporte

Problemas com a configuração local? Verifique:

1. ✅ [CI_SETUP_CHECKLIST.md](CI_SETUP_CHECKLIST.md) - Checklist completo
2. ✅ [CI_WORKFLOW_GUIDE.md](CI_WORKFLOW_GUIDE.md) - Guia do workflow
3. ✅ [CI_QUICK_REFERENCE.md](CI_QUICK_REFERENCE.md) - Referência rápida

---

**Última atualização**: Janeiro 2026  
**Versão**: 1.0
