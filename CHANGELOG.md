# Changelog

Todas as mudanças notáveis neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/),
e este projeto adere ao [Semantic Versioning](https://semver.org/lang/pt-BR/).

## [Unreleased]

### Adicionado
- Pipeline de CI/CD completo com GitHub Actions
- Workflow de build e testes automatizados
- Workflow de validação de Pull Requests
- Workflow de análise de qualidade de código
- Workflow de build de releases
- Integração com CodeQL para análise de segurança
- Integração com Codecov para cobertura de código
- Conventional Commits validation
- PR size labeling automático
- Documentação completa de CI/CD (CI_CD_SETUP.md)
- README de workflows

### Planejado
- Sistema de autenticação e autorização (JWT)
- CRUD de produtos
- Sistema de pedidos
- Integração com gateway de pagamento
- Sistema de avaliações

## [0.1.0] - 2026-01-06

### Adicionado
- Estrutura inicial do projeto
- Documentação completa (README, ARCHITECTURE, API, CONTRIBUTING)
- Configuração do GitHub Copilot
- Configuração de camadas (API, Application, Domain, Infrastructure, CrossCutting)
- Templates para Issues e Pull Requests

### Planejado para v1.0.0
- [ ] Autenticação e autorização
- [ ] CRUD de produtos
- [ ] Sistema de pedidos
- [ ] Pagamentos
- [ ] Avaliações
- [ ] Notificações
- [ ] Cache distribuído
- [ ] Message queue
- [ ] CI/CD pipeline
- [ ] Docker support
- [ ] Kubernetes manifests

---

## Tipos de Mudanças

- `Added` - Novas funcionalidades
- `Changed` - Mudanças em funcionalidades existentes
- `Deprecated` - Funcionalidades que serão removidas
- `Removed` - Funcionalidades removidas
- `Fixed` - Correções de bugs
- `Security` - Correções de segurança

---

[Unreleased]: https://github.com/jorelrx/PrismaPrimeMarketAPI/compare/v0.1.0...HEAD
[0.1.0]: https://github.com/jorelrx/PrismaPrimeMarketAPI/releases/tag/v0.1.0
