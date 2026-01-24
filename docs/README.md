# 📚 Documentação do Projeto

Bem-vindo à documentação do **Prisma Prime Market API**!

## 🗂️ Índice de Documentação

### 📖 Geral
- [**ARCHITECTURE.md**](ARCHITECTURE.md) - Arquitetura detalhada do sistema
- [**API.md**](API.md) - Documentação da API REST
- [**PROJECT_STRUCTURE.md**](PROJECT_STRUCTURE.md) - Estrutura de diretórios
- [**CONTRIBUTING.md**](CONTRIBUTING.md) - Guia de contribuição

### 🤖 Inteligência Artificial
- [**AI_INTEGRATION.md**](AI_INTEGRATION.md) - Integração com IA (PostgreSQL + pgvector)

### 🚀 CI/CD e DevOps

#### Para Começar Rápido
- [**CI_CD_SUMMARY.md**](CI_CD_SUMMARY.md) ⭐ **COMECE AQUI!**  
  Resumo executivo - tudo que você precisa saber em 5 minutos

#### Detalhes de Implementação
- [**CI_CD_IMPLEMENTATION.md**](CI_CD_IMPLEMENTATION.md)  
  O que foi implementado e como funciona

#### Guias de Configuração
- [**CI_CD_SETUP.md**](CI_CD_SETUP.md)  
  Passo a passo completo para configurar o pipeline

#### Referências Rápidas
- [**QUICK_REFERENCE.md**](QUICK_REFERENCE.md)  
  Comandos úteis, git workflow, troubleshooting

#### Fluxogramas Visuais
- [**CI_CD_FLOWCHART.md**](CI_CD_FLOWCHART.md)  
  Diagramas ASCII dos workflows e processos

### 🏛️ Decisões Arquiteturais (ADRs)
- [**adr/001-architecture-style.md**](adr/001-architecture-style.md) - Escolha da arquitetura
- [**adr/002-database-choice.md**](adr/002-database-choice.md) - Escolha do banco de dados

## 🎯 Navegação por Persona

### 👨‍💻 Sou um Desenvolvedor Novo no Projeto
1. Leia: [CI_CD_SUMMARY.md](CI_CD_SUMMARY.md)
2. Configure: [CI_CD_SETUP.md](CI_CD_SETUP.md)
3. Consulte: [QUICK_REFERENCE.md](QUICK_REFERENCE.md)
4. Contribua: [CONTRIBUTING.md](CONTRIBUTING.md)

### 👨‍💼 Sou um Tech Lead / Arquiteto
1. Revise: [ARCHITECTURE.md](ARCHITECTURE.md)
2. Entenda: [CI_CD_IMPLEMENTATION.md](CI_CD_IMPLEMENTATION.md)
3. Veja decisões: [adr/](adr/)
4. Configure: [CI_CD_SETUP.md](CI_CD_SETUP.md)

### 🎓 Sou um Recrutador / Reviewer
1. Comece: [CI_CD_SUMMARY.md](CI_CD_SUMMARY.md)
2. Entenda a arquitetura: [ARCHITECTURE.md](ARCHITECTURE.md)
3. Veja o pipeline: [CI_CD_IMPLEMENTATION.md](CI_CD_IMPLEMENTATION.md)
4. Explore o código: [../src/](../src/)

### 🔧 Sou um DevOps Engineer
1. Leia: [CI_CD_IMPLEMENTATION.md](CI_CD_IMPLEMENTATION.md)
2. Configure: [CI_CD_SETUP.md](CI_CD_SETUP.md)
3. Workflows: [../.github/workflows/](.../.github/workflows/)
4. Fluxos: [CI_CD_FLOWCHART.md](CI_CD_FLOWCHART.md)

## 📊 Documentação por Tópico

### Arquitetura
```
ARCHITECTURE.md           → Visão geral da arquitetura
PROJECT_STRUCTURE.md      → Estrutura de diretórios
adr/                      → Decisões arquiteturais documentadas
```

### API
```
API.md                    → Documentação dos endpoints
(Swagger quando rodando)  → Documentação interativa
```

### CI/CD
```
CI_CD_SUMMARY.md          → 📌 Resumo executivo (START HERE)
CI_CD_IMPLEMENTATION.md   → O que foi implementado
CI_CD_SETUP.md            → Como configurar
QUICK_REFERENCE.md        → Comandos úteis
CI_CD_FLOWCHART.md        → Fluxogramas visuais
../.github/workflows/     → Código dos workflows
```

### Contribuição
```
CONTRIBUTING.md           → Como contribuir
QUICK_REFERENCE.md        → Comandos Git e convenções
CI_CD_FLOWCHART.md        → Conventional Commits
```

## 🔍 Busca Rápida

### Preciso de...
- **Entender a arquitetura** → [ARCHITECTURE.md](ARCHITECTURE.md)
- **Configurar CI/CD** → [CI_CD_SETUP.md](CI_CD_SETUP.md)
- **Fazer um commit** → [QUICK_REFERENCE.md](QUICK_REFERENCE.md#conventional-commits)
- **Criar um PR** → [CONTRIBUTING.md](CONTRIBUTING.md)
- **Executar testes** → [QUICK_REFERENCE.md](QUICK_REFERENCE.md#build--testes)
- **Entender um workflow** → [../.github/workflows/README.md](../.github/workflows/README.md)
- **Ver fluxos visuais** → [CI_CD_FLOWCHART.md](CI_CD_FLOWCHART.md)
- **Comandos rápidos** → [QUICK_REFERENCE.md](QUICK_REFERENCE.md)

### Quero saber sobre...
- **Clean Architecture** → [ARCHITECTURE.md](ARCHITECTURE.md)
- **DDD** → [ARCHITECTURE.md](ARCHITECTURE.md)
- **SOLID** → [ARCHITECTURE.md](ARCHITECTURE.md)
- **Design Patterns** → [ARCHITECTURE.md](ARCHITECTURE.md)
- **PostgreSQL + pgvector** → [AI_INTEGRATION.md](AI_INTEGRATION.md)
- **GitHub Actions** → [CI_CD_IMPLEMENTATION.md](CI_CD_IMPLEMENTATION.md)
- **Conventional Commits** → [CI_CD_FLOWCHART.md](CI_CD_FLOWCHART.md)
- **CodeQL** → [CI_CD_SETUP.md](CI_CD_SETUP.md)

## 📈 Roadmap de Leitura

### Nível 1: Essencial (30 min)
1. [README.md](../README.md) principal
2. [CI_CD_SUMMARY.md](CI_CD_SUMMARY.md)
3. [QUICK_REFERENCE.md](QUICK_REFERENCE.md)

### Nível 2: Importante (1-2 horas)
4. [ARCHITECTURE.md](ARCHITECTURE.md)
5. [CI_CD_IMPLEMENTATION.md](CI_CD_IMPLEMENTATION.md)
6. [CI_CD_SETUP.md](CI_CD_SETUP.md)
7. [CONTRIBUTING.md](CONTRIBUTING.md)

### Nível 3: Aprofundamento (3+ horas)
8. [PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md)
9. [API.md](API.md)
10. [AI_INTEGRATION.md](AI_INTEGRATION.md)
11. [adr/](adr/) - Todos os ADRs
12. [../.github/workflows/](../.github/workflows/) - Código dos workflows

## 🎯 Casos de Uso

### "Preciso configurar o projeto pela primeira vez"
```
1. README.md (principal)
2. CI_CD_SUMMARY.md
3. CI_CD_SETUP.md
4. Execute os passos de configuração
5. QUICK_REFERENCE.md para comandos
```

### "Quero fazer minha primeira contribuição"
```
1. CONTRIBUTING.md
2. QUICK_REFERENCE.md (Git Workflow)
3. CI_CD_FLOWCHART.md (Conventional Commits)
4. Faça seu código
5. Crie PR seguindo os padrões
```

### "Preciso entender a arquitetura antes de implementar"
```
1. ARCHITECTURE.md
2. PROJECT_STRUCTURE.md
3. adr/ (decisões relevantes)
4. Explore o código em src/
```

### "Vou fazer uma apresentação sobre o projeto"
```
1. README.md (principal)
2. ARCHITECTURE.md
3. CI_CD_IMPLEMENTATION.md
4. CI_CD_FLOWCHART.md (para slides visuais)
```

## 🔗 Links Externos

### Ferramentas e Tecnologias
- [.NET 8.0 Documentation](https://docs.microsoft.com/dotnet/)
- [GitHub Actions](https://docs.github.com/actions)
- [PostgreSQL](https://www.postgresql.org/docs/)
- [pgvector](https://github.com/pgvector/pgvector)

### Padrões e Convenções
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Semantic Versioning](https://semver.org/)
- [Keep a Changelog](https://keepachangelog.com/)

### Qualidade e Segurança
- [CodeQL](https://codeql.github.com/docs/)
- [Codecov](https://docs.codecov.com/)
- [OWASP](https://owasp.org/)

## 📝 Notas

- **Sempre atualize a documentação** quando fizer mudanças significativas
- **Documente decisões** importantes em ADRs
- **Mantenha exemplos** atualizados e funcionais
- **Use linguagem clara** e evite jargões desnecessários

## 🆘 Precisa de Ajuda?

1. **Documentação não está clara?**  
   Abra uma issue explicando o problema

2. **Encontrou um erro?**  
   Faça um PR corrigindo

3. **Falta algo?**  
   Sugira adições via issue

4. **Tem dúvidas sobre implementação?**  
   Veja [CONTRIBUTING.md](CONTRIBUTING.md) e depois abra uma discussion

## 📊 Status da Documentação

| Documento | Status | Última Atualização |
|-----------|--------|-------------------|
| README.md | ✅ Completo | 2026-01-19 |
| ARCHITECTURE.md | ✅ Completo | 2026-01-06 |
| API.md | ✅ Completo | 2026-01-06 |
| PROJECT_STRUCTURE.md | ✅ Completo | 2026-01-06 |
| CONTRIBUTING.md | ✅ Completo | 2026-01-06 |
| AI_INTEGRATION.md | ✅ Completo | 2026-01-06 |
| CI_CD_SUMMARY.md | ✅ Completo | 2026-01-19 |
| CI_CD_IMPLEMENTATION.md | ✅ Completo | 2026-01-19 |
| CI_CD_SETUP.md | ✅ Completo | 2026-01-19 |
| QUICK_REFERENCE.md | ✅ Completo | 2026-01-19 |
| CI_CD_FLOWCHART.md | ✅ Completo | 2026-01-19 |

---

**Mantenha a documentação viva! 📚✨**

---

**Última atualização:** 2026-01-19  
**Versão:** 1.0
