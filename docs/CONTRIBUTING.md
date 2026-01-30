# 🤝 Guia de Contribuição - Prisma Prime Market API

Este documento descreve o fluxo de trabalho completo para contribuir com o projeto, incluindo criação de features, correções de bugs, commits, pull requests e todas as regras do projeto.

---

## 📋 Índice

- [Fluxo de Trabalho Resumido](#fluxo-de-trabalho-resumido)
- [Regras do Projeto](#regras-do-projeto)
- [Tipos de Contribuições](#tipos-de-contribuições)
- [Conventional Commits](#conventional-commits)
- [Processo Passo a Passo](#processo-passo-a-passo)
- [Validações Automáticas](#validações-automáticas)
- [Pull Requests](#pull-requests)
- [Troubleshooting](#troubleshooting)
- [Boas Práticas](#boas-práticas)

---

## 🎯 Fluxo de Trabalho Resumido

```
1. Criar branch → 2. Desenvolver → 3. Commit → 4. Push → 5. PR → 6. Review → 7. Merge
```

**Regra de Ouro:** 🚫 **NUNCA faça commit ou push direto na branch `main`!**

---

## 📜 Regras do Projeto

### 🔒 **Branch Protection**

#### Branch `main`:
- ❌ **Commit direto bloqueado** (localmente via Husky)
- ❌ **Push direto bloqueado** (remotamente via GitHub)
- ✅ **Apenas via Pull Request**
- ✅ **Requer aprovação** (mínimo 1 reviewer)
- ✅ **Todos os checks devem passar**

#### Outras branches:
- ✅ Commits permitidos normalmente
- ✅ Push permitido após validações locais

### 🛡️ **Validações Locais (Husky)**

Executadas automaticamente antes de commit/push:

1. **Pre-commit**:
   - Bloqueia commits na branch `main`

2. **Commit-msg (Commitlint)**:
   - Valida formato usando commitlint com `.commitlintrc.json`
   - Garante Conventional Commits

3. **Pre-push**:
   - Executa `dotnet build`
   - Executa todos os testes (`dotnet test`)
   - Bloqueia push se algo falhar

### 🤖 **Validações Remotas (GitHub Actions)**

Executadas automaticamente em PRs:

1. **PR Checks**: Valida título do PR e commits
2. **CI Pipeline**: 5 etapas de testes e build
3. **Code Quality**: CodeQL e análise de segurança
4. **Docker Build**: Apenas após merge na `main`

---

## 🎨 Tipos de Contribuições

### 🆕 **Nova Feature** (`feat`)
Adicionar uma nova funcionalidade ao sistema.

**Exemplo:**
- Novo endpoint de API
- Nova entidade de domínio
- Novo caso de uso

### 🐛 **Bug Fix** (`fix`)
Corrigir um problema existente.

**Exemplo:**
- Corrigir validação incorreta
- Resolver erro de lógica
- Ajustar comportamento inesperado

### 📚 **Documentação** (`docs`)
Atualizar ou criar documentação.

**Exemplo:**
- Atualizar README
- Adicionar comentários
- Criar guias

### 💅 **Estilo/Formatação** (`style`)
Mudanças que não afetam a lógica do código.

**Exemplo:**
- Formatação de código
- Adicionar espaços
- Remover linhas em branco

### ♻️ **Refatoração** (`refactor`)
Melhorar código existente sem mudar comportamento.

**Exemplo:**
- Extrair método
- Renomear variável
- Simplificar lógica

### ⚡ **Performance** (`perf`)
Melhorias de performance.

**Exemplo:**
- Otimizar query
- Cache
- Reduzir alocações

### 🧪 **Testes** (`test`)
Adicionar ou corrigir testes.

**Exemplo:**
- Novos testes unitários
- Testes de integração
- Corrigir testes quebrados

### 🔧 **Build/CI** (`build`, `ci`)
Mudanças no sistema de build ou CI/CD.

**Exemplo:**
- Atualizar workflow
- Mudar configuração do Docker
- Ajustar pipeline

### 🔨 **Manutenção** (`chore`)
Tarefas de manutenção geral.

**Exemplo:**
- Atualizar dependências
- Configurar ferramentas
- Limpar código morto

---

## 📝 Conventional Commits

### Formato

```
tipo(escopo): Descrição curta começando com letra maiúscula

[corpo opcional]

[rodapé opcional]
```

**📌 Regras importantes:**
- ✅ Tipo sempre em **minúsculo** (`feat`, `fix`, `docs`, etc.)
- ✅ Descrição sempre começando com **letra maiúscula**
- ✅ Sem ponto final na descrição
- ✅ Máximo de 100 caracteres no header

### Exemplos Corretos ✅

```bash
feat: Adicionar endpoint de produtos
feat(api): Implementar listagem de produtos com paginação
fix: Corrigir validação de CPF
fix(domain): Resolver erro ao criar usuário sem email
docs: Atualizar guia de contribuição
style: Formatar código com dotnet format
refactor(application): Extrair lógica de validação para service
test: Adicionar testes para ProductService
build: Atualizar pacote AutoMapper para v13
ci: Corrigir workflow de docker build
chore: Atualizar dependências do projeto
```

### Exemplos Incorretos ❌

```bash
# ❌ Sem tipo
Adicionar endpoint de produtos

# ❌ Tipo inválido
add: Adicionar endpoint de produtos

# ❌ Não começar com letra maiúscula
feat: adicionar endpoint de produtos

# ❌ Todas as palavras em maiúscula (Pascal Case)
feat: Adicionar Endpoint De Produtos

# ❌ Ponto final na descrição
feat: Adicionar endpoint de produtos.

# ❌ Descrição muito longa (> 100 caracteres)
feat: Adicionar endpoint de produtos com listagem paginada e filtros avançados por categoria e preço
```

### Tipos Válidos

| Tipo | Descrição | Emoji |
|------|-----------|-------|
| `feat` | Nova funcionalidade | ✨ |
| `fix` | Correção de bug | 🐛 |
| `docs` | Documentação | 📚 |
| `style` | Formatação | 💅 |
| `refactor` | Refatoração | ♻️ |
| `perf` | Performance | ⚡ |
| `test` | Testes | 🧪 |
| `build` | Build | 📦 |
| `ci` | CI/CD | 🤖 |
| `chore` | Manutenção | 🔨 |
| `revert` | Reverter commit | ⏪ |

---

## 🚀 Processo Passo a Passo

### **1️⃣ Atualizar a Main**

Sempre comece com a `main` atualizada:

```bash
git checkout main
git pull origin main
```

### **2️⃣ Criar Nova Branch**

Use nomenclatura clara seguindo o padrão: `tipo/descrição-curta`

```bash
# Nova feature
git checkout -b feat/adicionar-endpoint-produtos

# Bug fix
git checkout -b fix/corrigir-validacao-cpf

# Documentação
git checkout -b docs/atualizar-readme

# Refatoração
git checkout -b refactor/melhorar-product-service
```

### **3️⃣ Desenvolver**

Faça suas alterações seguindo as regras do projeto:

- ✅ Clean Architecture + DDD + SOLID
- ✅ Testes unitários e de integração
- ✅ Documentação XML nos métodos públicos
- ✅ Seguir convenções de nomenclatura C#
- ✅ Usar async/await
- ✅ Logging estruturado

**Consulte:** [.github/copilot-instructions.md](../.github/copilot-instructions.md) para detalhes completos.

### **4️⃣ Fazer Commits**

Commits pequenos e atômicos, seguindo Conventional Commits:

```bash
# Adicionar arquivos
git add .

# Commit (será validado automaticamente)
git commit -m "feat: Adicionar entidade Product"
git commit -m "feat: Implementar ProductRepository"
git commit -m "test: Adicionar testes para Product"
```

**O que acontece no commit:**
Husky configura *git hooks* que rodam em sequência durante o `git commit`:
1. 🔁 **Hook `pre-commit`**: roda antes de abrir o editor da mensagem
   - Verifica se você não está na branch `main` (bloqueia commits diretos em `main`)
2. 🔁 **Hook `commit-msg`**: roda após escrever a mensagem
   - Valida usando **commitlint** com `.commitlintrc.json`
   - Garante formato **Conventional Commits**
   - ❌ Se o formato estiver incorreto, o commit é abortado

**Se o commit for bloqueado:**
```bash
# Exemplo de erro:
# ❌ 🚫 Commit direto na branch main é proibido!
# ❌ subject may not be empty [subject-empty]

# Corrija e tente novamente
git commit -m "feat: Adicionar entidade Product"
```

### **5️⃣ Push**

Envie sua branch para o GitHub:

```bash
# Primeiro push da branch
git push origin feat/adicionar-endpoint-produtos

# Pushes subsequentes
git push
```

**O que acontece no push:**
1. ✅ Husky executa `dotnet build`
2. ✅ Husky executa `dotnet test`
3. ❌ Bloqueia se build ou testes falharem

**Se o push for bloqueado:**
```bash
# Exemplo de erro:
# ❌ Build falhou! Push bloqueado.
# ❌ Testes falharam! Push bloqueado.

# Corrija os problemas
dotnet build
dotnet test

# Faça commit da correção
git add .
git commit -m "fix: Corrigir testes"
git push
```

**Bypass de emergência** (use apenas em casos extremos):
```bash
git push --no-verify
```

### **6️⃣ Criar Pull Request**

No GitHub:

1. Vá para o repositório
2. Clique em **Compare & pull request**
3. **Título** deve seguir Conventional Commits:
   ```
   feat: Adicionar endpoint de listagem de produtos
   ```
4. Preencha a descrição seguindo o template
5. Clique em **Create pull request**

### **7️⃣ Validações Automáticas**

O GitHub Actions vai executar automaticamente:

```
⏳ PR Checks / Validar Pull Request
⏳ CI Pipeline / Etapa 1: Testes em Docker
⏳ CI Pipeline / Etapa 2: Build & Testes com Cobertura
⏳ CI Pipeline / Etapa 3: Qualidade de Código
⏳ CI Pipeline / Etapa 4: Análise Estática
⏳ CI Pipeline / Etapa 5: Verificação Final
⏳ Code Quality / CodeQL Analysis
```

**Tempo estimado:** 3-10 minutos

### **8️⃣ Code Review**

- Aguarde aprovação de pelo menos 1 reviewer
- Responda a comentários se necessário
- Faça ajustes solicitados

**Para fazer ajustes:**
```bash
# Fazer alterações
git add .
git commit -m "refactor: Aplicar sugestões do code review"
git push

# Os workflows vão executar novamente automaticamente
```

### **9️⃣ Merge**

Após aprovação e todos os checks passarem:

1. Clique em **Squash and merge** (recomendado) ou **Merge pull request**
2. Confirme o merge
3. A branch será automaticamente deletada (opcional)
4. O Docker build será executado automaticamente

### **🔟 Limpar Branch Local**

```bash
# Voltar para main
git checkout main
git pull origin main

# Deletar branch local
git branch -D feat/adicionar-endpoint-produtos
```

---

## ✅ Validações Automáticas

### 🏠 **Validações Locais (Husky)**

#### **pre-commit**
- ✅ Bloqueia commits na branch `main`
- ⚡ Executa: Antes de criar o commit

#### **commit-msg**
- ✅ Valida formato Conventional Commits usando **commitlint**
- ✅ Configurado em `.commitlintrc.json`
- ⚡ Executa: Imediatamente após escrever mensagem de commit

#### **pre-push**
- ✅ Executa build do projeto
- ✅ Executa todos os testes
- ✅ Bloqueia push se falhar
- ⚡ Executa: Antes de enviar para o GitHub

### ☁️ **Validações Remotas (GitHub Actions)**

#### **PR Checks** (`pr-checks.yml`)
- ✅ Valida título do PR (Conventional Commits)
- ✅ Valida mensagens de todos os commits
- ✅ Adiciona label de tamanho do PR (xs, s, m, l, xl)
- ⚡ Executa: Ao abrir/atualizar PR

#### **CI Pipeline** (`ci.yml`)
**Etapa 1: Testes em Docker**
- ✅ Executa testes em ambiente isolado
- ✅ Valida compatibilidade com Docker

**Etapa 2: Build & Testes com Cobertura**
- ✅ Build em Release mode
- ✅ Executa todos os testes
- ✅ Coleta cobertura de código
- ✅ Envia para Codecov (target: 80%)

**Etapa 3: Qualidade de Código**
- ✅ Verifica formatação (`dotnet format`)
- ✅ Scan de pacotes vulneráveis
- ✅ Scan de pacotes deprecados

**Etapa 4: Análise Estática**
- ✅ Build com analisadores Roslyn
- ✅ Análise de código estática

**Etapa 5: Verificação Final**
- ✅ Confirma que todas as etapas passaram
- ✅ Bloqueia merge se algo falhou

#### **Code Quality** (`code-quality.yml`)
- ✅ CodeQL Analysis (análise de segurança)
- ✅ Executa semanalmente
- ⚡ Executa: Em PRs e schedule

#### **Docker Build** (`docker-build.yml`)
- ✅ Build da imagem Docker
- ✅ Push para Docker Hub
- ⚡ Executa: Em push para as branches `main` e `develop`

---

## 📋 Pull Requests

### Título do PR

Deve seguir **Conventional Commits**:

```
feat: Adicionar endpoint de listagem de produtos
fix: Corrigir validação de email no registro
docs: Atualizar documentação da API
refactor: Melhorar estrutura do ProductService
```

### Descrição do PR

Use o template fornecido. Inclua:

1. **Descrição**: O que foi feito e por quê
2. **Tipo de Mudança**: Feature, Bug Fix, etc.
3. **Checklist**: Testes, documentação, etc.
4. **Screenshots**: Se aplicável
5. **Issues relacionadas**: Links para issues

### Tamanho do PR

Labels automáticas baseadas em linhas alteradas:

| Label | Tamanho | Recomendação |
|-------|---------|--------------|
| `size/xs` | < 10 linhas | ✅ Ideal |
| `size/s` | < 100 linhas | ✅ Bom |
| `size/m` | < 500 linhas | ⚠️ Aceitável |
| `size/l` | < 1000 linhas | ⚠️ Grande - considere dividir |
| `size/xl` | > 1000 linhas | 🚨 Muito grande - divida! |

**Recomendação:** Mantenha PRs pequenos e focados!

### Code Review

#### Como Revisor:

1. ✅ Verifique se segue Clean Architecture + DDD
2. ✅ Valide SOLID e padrões do projeto
3. ✅ Revise testes unitários e de integração
4. ✅ Confirme que documentação foi atualizada
5. ✅ Teste localmente se necessário

#### Como Autor:

1. ✅ Responda a todos os comentários
2. ✅ Faça ajustes solicitados
3. ✅ Marque conversas como resolvidas
4. ✅ Notifique quando pronto para re-review

---

## 🐛 Troubleshooting

### ❌ Commit Bloqueado

**Erro:** "Commit direto na branch main é proibido!"

```bash
# Solução: Mude para uma branch de feature
git checkout -b feat/minha-feature
git commit -m "feat: Minha alteração"
```

**Erro:** "subject may not be empty [subject-empty]"

```bash
# Solução: Use formato correto
git commit -m "feat: Adicionar nova funcionalidade"
```

### ❌ Push Bloqueado

**Erro:** "Build falhou! Push bloqueado."

```bash
# Solução: Corrija os erros de build
dotnet build

# Veja os erros e corrija
# Depois faça novo commit
git add .
git commit -m "fix: Corrigir erros de build"
git push
```

**Erro:** "Testes falharam! Push bloqueado."

```bash
# Solução: Corrija os testes
dotnet test --verbosity detailed

# Veja quais testes falharam e corrija
git add .
git commit -m "fix: Corrigir testes falhando"
git push
```

### ❌ PR Checks Falhando

**Erro:** "PR title does not match Conventional Commits"

```bash
# Solução: Edite o título do PR no GitHub
# De: "Add products endpoint"
# Para: "feat: Adicionar endpoint de produtos"
```

**Erro:** "Commit messages do not match Conventional Commits"

```bash
# Solução: Reescreva os commits
git rebase -i HEAD~3  # Para os últimos 3 commits
# Use 'reword' para editar mensagens

# Ou use squash para combinar commits:
git rebase -i HEAD~3
# Marque commits com 's' para squash
```

### ❌ CI Pipeline Falhando

**Erro:** "Tests failed in Docker"

```bash
# Solução: Rode testes localmente com Docker
docker compose -f docker-compose.test.yml up --build

# Veja os logs e corrija
```

**Erro:** "Code coverage below threshold"

```bash
# Solução: Adicione mais testes
dotnet test --collect:"XPlat Code Coverage"

# Veja relatório de cobertura
```

### 🚨 Bypass de Emergência

**Apenas em casos extremos!**

Use o bypass **somente** quando as validações estiverem impedindo a correção de um problema crítico e não houver tempo hábil para corrigir o próprio pipeline primeiro. Exemplos de uso legítimo:

- Hotfix urgente em produção onde:
  - os hooks locais ou o pipeline de CI estão quebrados (ex.: script falhando sem relação com sua mudança), **e**
  - o problema impacta usuários em produção (ex.: API fora do ar, falha grave de segurança ou perda de dados).
- Teste automatizado conhecido como instável/flaky ou quebrado por motivo já rastreado em issue, mas que **não** é afetado pela mudança de hotfix.
- Falha temporária de infraestrutura (ex.: indisponibilidade de feed de pacotes, serviço externo crítico) que impede o pipeline, mas a alteração precisa ser registrada/entregue imediatamente.

Sempre que usar bypass:
- Documente claramente no commit/PR o motivo do bypass (incluindo link para issue se existir).
- Abra ou atualize uma issue para corrigir o hook/pipeline quebrado **o quanto antes**.
- Não utilize para agilizar desenvolvimento normal, refactors ou features não críticas.

```bash
# Bypass validações locais (use apenas nos cenários descritos acima)
git commit -m "fix: Corrigir problema crítico em produção (bypass explicado no PR)" --no-verify
git push --no-verify

# ⚠️ Não abuse! Use apenas em emergências reais e sempre documente o motivo.
```

---

## 💡 Boas Práticas

### ✅ Commits

- **Faça commits pequenos e frequentes**
- **Um commit = uma mudança lógica**
- **Mensagens claras e descritivas**
- **Use o corpo do commit para explicar o "porquê"**

```bash
# ✅ Bom
git commit -m "feat: Adicionar validação de CPF"
git commit -m "test: Adicionar testes para validação de CPF"
git commit -m "docs: Documentar classe CPF"

# ❌ Ruim
git commit -m "WIP"
git commit -m "fix"
git commit -m "mudanças gerais"
```

### ✅ Branches

- **Nome descritivo e curto**
- **Use prefixo do tipo** (feat/, fix/, docs/, etc.)
- **Separe palavras com hífen**

```bash
# ✅ Bom
feat/adicionar-endpoint-produtos
fix/corrigir-validacao-email
docs/atualizar-architecture-md

# ❌ Ruim
nova-branch
teste
username/mudancas
```

### ✅ Pull Requests

- **Um PR = uma funcionalidade/fix**
- **Descrição detalhada**
- **Screenshots quando aplicável**
- **Link para issues relacionadas**
- **Mantenha PRs pequenos** (< 500 linhas)

### ✅ Code Review

- **Seja respeitoso e construtivo**
- **Sugira melhorias, não exija**
- **Explique o "porquê" das sugestões**
- **Aprove rapidamente se está OK**

### ✅ Testes

- **Sempre adicione testes para novo código**
- **Mantenha cobertura >= 80%**
- **Testes devem ser independentes**
- **Use nomes descritivos**

```csharp
// ✅ Bom
[Fact]
public void Create_WithValidData_ShouldCreateProduct()

// ❌ Ruim
[Fact]
public void Test1()
```

### ✅ Documentação

- **Documente classes e métodos públicos**
- **Atualize README quando necessário**
- **Mantenha ADRs para decisões importantes**
- **Documente quebras de API**

---

## 📚 Referências

### Documentação do Projeto
- [Arquitetura](ARCHITECTURE.md)
- [Estrutura do Projeto](PROJECT_STRUCTURE.md)
- [API](API.md)
- [CI/CD](CI_README.md)

### Guias de CI/CD
- [Workflow Guide](CI_WORKFLOW_GUIDE.md)
- [Setup Checklist](CI_SETUP_CHECKLIST.md)
- [Quick Reference](CI_QUICK_REFERENCE.md)

### Padrões
- [Copilot Instructions](../.github/copilot-instructions.md)
- [ADRs](adr/)

### Links Externos
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [C# Coding Conventions](https://docs.microsoft.com/dotnet/csharp/fundamentals/coding-style/)

---

## 🆘 Suporte

Se tiver dúvidas ou problemas:

1. **Consulte a documentação** em `docs/`
2. **Verifique issues existentes** no GitHub
3. **Crie uma issue** se for um problema novo
4. **Pergunte no canal do time** (se aplicável)

---

## ✅ Checklist Rápido

Use este checklist antes de criar um PR:

- [ ] Branch criada a partir da `main` atualizada
- [ ] Nome da branch segue padrão: `tipo/descrição`
- [ ] Commits seguem Conventional Commits
- [ ] Código segue Clean Architecture + DDD + SOLID
- [ ] Testes unitários e/ou integração adicionados
- [ ] Todos os testes passando localmente
- [ ] Documentação atualizada (se necessário)
- [ ] Build local passou
- [ ] Cobertura de código >= 80% (se aplicável)
- [ ] PR com título em Conventional Commits
- [ ] Descrição do PR completa
- [ ] Pronto para code review

---

**Última atualização:** Janeiro 2026  
**Versão:** 1.0

---

**Bem-vindo ao projeto! 🚀**

Siga este guia e contribua com qualidade. Se tiver sugestões de melhoria para este documento, abra um PR!
