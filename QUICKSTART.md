# 🚀 Quick Start Guide - Prisma Prime Market API

## Início Rápido em 2 Minutos ⚡

### Com Docker (Mais Fácil)

```bash
# 1. Clone
git clone https://github.com/jorelrx/PrismaPrimeMarketAPI.git
cd PrismaPrimeMarketAPI

# 2. Inicie
docker-compose up -d

# 3. Acesse
# http://localhost:8080/swagger
```

**Pronto! ✅** API + PostgreSQL + PgAdmin rodando!

---

## Comandos Essenciais

### Docker

```bash
# Iniciar
docker-compose up -d

# Ver logs
docker-compose logs -f api

# Parar
docker-compose down

# Rebuild (após mudanças)
docker-compose up -d --build

# Reset completo (apaga dados)
docker-compose down -v
```

### Testes

```bash
# Testes locais
dotnet test

# Testes com Docker
.\scripts\test-docker.bat                    # Windows
docker-compose -f docker-compose.test.yml up --build --abort-on-container-exit  # Linux

# Validação completa
.\scripts\validate.bat                       # Windows
./scripts/validate.sh                        # Linux
```

### Git Hooks

```bash
# Configurar (uma vez)
git config core.hooksPath .githooks

# Agora git push roda testes automaticamente! 🎉
```

---

## URLs Importantes

| Serviço | URL | Credenciais |
|---------|-----|-------------|
| API | http://localhost:8080 | - |
| Swagger | http://localhost:8080/swagger | - |
| PgAdmin | http://localhost:5050 | admin@prismaprime.com / admin |
| PostgreSQL | localhost:5432 | postgres / postgres |

---

## Desenvolvimento Local (Sem Docker)

```bash
# 1. Instale PostgreSQL localmente

# 2. Configure connection string
# Edite: src/PrismaPrimeMarket.API/appsettings.Development.json

# 3. Restaurar pacotes
dotnet restore

# 4. Aplicar migrations
dotnet ef database update \
  --project src/PrismaPrimeMarket.Infrastructure \
  --startup-project src/PrismaPrimeMarket.API

# 5. Rodar API
dotnet run --project src/PrismaPrimeMarket.API
```

---

## Primeiros Passos

### 1. Criar um usuário

```bash
POST http://localhost:8080/api/v1/users/register
Content-Type: application/json

{
  "userName": "testuser",
  "firstName": "Test",
  "lastName": "User",
  "email": "test@example.com",
  "password": "Test@1234"
}
```

### 2. Login

```bash
POST http://localhost:8080/api/v1/auth/login
Content-Type: application/json

{
  "userName": "testuser",
  "password": "Test@1234"
}
```

### 3. Usar o token

```bash
GET http://localhost:8080/api/v1/users
Authorization: Bearer {seu-token-aqui}
```

---

## Estrutura do Projeto

```
PrismaPrimeMarketAPI/
├── src/
│   ├── PrismaPrimeMarket.API/              # Controllers, Middlewares
│   ├── PrismaPrimeMarket.Application/       # Use Cases, DTOs, CQRS
│   ├── PrismaPrimeMarket.Domain/            # Entities, Business Rules
│   ├── PrismaPrimeMarket.Infrastructure/    # Database, Repositories
│   └── PrismaPrimeMarket.CrossCutting/      # DI, Logging, Security
├── tests/
│   ├── PrismaPrimeMarket.UnitTests/
│   └── PrismaPrimeMarket.IntegrationTests/
├── docs/                                     # Documentação completa
├── scripts/                                  # Scripts úteis
├── docker-compose.yml                        # Docker local
└── docker-compose.test.yml                   # Docker para testes
```

---

## Próximos Passos

1. **Leia a documentação completa**
   - [Arquitetura](docs/ARCHITECTURE.md)
   - [API Guide](docs/API.md)
   - [CI/CD Docker](docs/CI_CD_DOCKER.md)

2. **Configure CI/CD**
   - Ver [docs/CI_CD_DOCKER.md](docs/CI_CD_DOCKER.md)
   - Configurar GitHub Actions
   - Deploy automático

3. **Customize o projeto**
   - Adicionar novos endpoints
   - Implementar regras de negócio
   - Integrar serviços externos

---

## Troubleshooting

### Porta 8080 já em uso

```bash
# Altere a porta em docker-compose.yml
ports:
  - "8081:8080"  # Usar 8081 no host
```

### Banco de dados não conecta

```bash
# Verifique se PostgreSQL está rodando
docker ps

# Veja logs do banco
docker-compose logs postgres

# Reset completo
docker-compose down -v
docker-compose up -d
```

### Testes falhando

```bash
# Limpar e rebuild
docker-compose down -v
docker system prune -af
dotnet clean
dotnet build
dotnet test
```

---

## Ajuda

- 📖 [Documentação Completa](docs/)
- 🐛 [Reportar Bug](https://github.com/jorelrx/PrismaPrimeMarketAPI/issues)
- 💬 [Discussões](https://github.com/jorelrx/PrismaPrimeMarketAPI/discussions)

---

**Happy Coding! 🚀**

### 5️⃣ Execute as Migrations

```bash
cd src/PrismaPrimeMarket.API
dotnet ef database update
cd ../..
```

### 6️⃣ Execute a Aplicação

```bash
cd src/PrismaPrimeMarket.API
dotnet run
```

🎉 **Pronto!** A API está rodando em:
- 🌐 HTTPS: `https://localhost:5001`
- 🌐 HTTP: `http://localhost:5000`
- 📚 Swagger: `https://localhost:5001/swagger`

---

## 🧪 Executar Testes

```bash
# Todos os testes
dotnet test

# Com cobertura
dotnet test /p:CollectCoverage=true

# Apenas testes unitários
dotnet test --filter Category=Unit

# Apenas testes de integração
dotnet test --filter Category=Integration
```

---

## 📝 Comandos Úteis

### Build
```bash
# Build do projeto
dotnet build

# Build em Release
dotnet build -c Release
```

### Migrations
```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration -p src/PrismaPrimeMarket.Infrastructure -s src/PrismaPrimeMarket.API

# Aplicar migrations
dotnet ef database update -p src/PrismaPrimeMarket.Infrastructure -s src/PrismaPrimeMarket.API

# Reverter última migration
dotnet ef migrations remove -p src/PrismaPrimeMarket.Infrastructure -s src/PrismaPrimeMarket.API

# Gerar script SQL
dotnet ef migrations script -p src/PrismaPrimeMarket.Infrastructure -s src/PrismaPrimeMarket.API
```

### Clean
```bash
# Limpar build artifacts
dotnet clean

# Limpar + rebuild
dotnet clean && dotnet build
```

### Format
```bash
# Formatar código
dotnet format

# Verificar formatação
dotnet format --verify-no-changes
```

---

## 🐳 Docker

### Build da Imagem
```bash
docker build -t prismaprime-api .
```

### Executar Container
```bash
docker run -d -p 5000:80 ^
  -e ConnectionStrings__DefaultConnection="Server=sqlserver;Database=PrismaPrimeMarket;..." ^
  --name prismaprime-api ^
  prismaprime-api
```

### Docker Compose (Completo)
```bash
# Subir todos os serviços (API + SQL Server + Redis)
docker-compose up -d

# Ver logs
docker-compose logs -f

# Parar serviços
docker-compose down
```

---

## 🔍 Testando a API

### Via Swagger
Abra `https://localhost:5001/swagger` no navegador.

### Via cURL

**Health Check:**
```bash
curl https://localhost:5001/health
```

**Registrar Usuário:**
```bash
curl -X POST https://localhost:5001/api/v1/users/register ^
  -H "Content-Type: application/json" ^
  -d "{\"name\":\"Test User\",\"email\":\"test@example.com\",\"password\":\"Test123!\"}"
```

**Login:**
```bash
curl -X POST https://localhost:5001/api/v1/auth/login ^
  -H "Content-Type: application/json" ^
  -d "{\"email\":\"test@example.com\",\"password\":\"Test123!\"}"
```

**Listar Produtos (com autenticação):**
```bash
curl https://localhost:5001/api/v1/products ^
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### Via PowerShell

```powershell
# Health Check
Invoke-RestMethod -Uri "https://localhost:5001/health" -Method Get

# Login
$loginBody = @{
    email = "test@example.com"
    password = "Test123!"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "https://localhost:5001/api/v1/auth/login" `
    -Method Post `
    -Body $loginBody `
    -ContentType "application/json"

$token = $response.token

# Listar Produtos
$headers = @{
    Authorization = "Bearer $token"
}

Invoke-RestMethod -Uri "https://localhost:5001/api/v1/products" `
    -Method Get `
    -Headers $headers
```

---

## 🎓 Próximos Passos

Agora que você configurou o projeto:

1. 📖 Leia a [Documentação de Arquitetura](docs/ARCHITECTURE.md)
2. 📚 Explore a [Documentação da API](docs/API.md)
3. 🤝 Veja o [Guia de Contribuição](docs/CONTRIBUTING.md)
4. 💻 Comece a contribuir!

---

## ❓ Problemas Comuns

### Erro de Conexão com PostgreSQL
```
NpgsqlException: Connection refused...
```

**Solução:**
- Verifique se PostgreSQL está rodando
- Confirme a connection string
- Teste conexão com: `psql -U postgres -h localhost`

### Erro de Porta em Uso
```
Unable to bind to https://localhost:5001...
```

**Solução:**
- Mude a porta em `launchSettings.json`
- Ou mate o processo: `netstat -ano | findstr :5001`

### Entity Framework não encontrado
```
Could not execute because the specified command or file was not found.
```

**Solução:**
```bash
dotnet tool install --global dotnet-ef
```

### Problema com Certificado SSL
```
The SSL connection could not be established...
```

**Solução:**
```bash
dotnet dev-certs https --trust
```

---

## 📞 Suporte

- 💬 **Dúvidas**: Abra uma [Discussion](https://github.com/jorelrx/PrismaPrimeMarketAPI/discussions)
- 🐛 **Bugs**: Abra uma [Issue](https://github.com/jorelrx/PrismaPrimeMarketAPI/issues)
- 📧 **Email**: joelv.9j@gmail.com

---

## 🌟 Dica Extra

Configure o VS Code com estas extensões recomendadas:

```bash
# C# Dev Kit
code --install-extension ms-dotnettools.csdevkit

# REST Client
code --install-extension humao.rest-client

# GitLens
code --install-extension eamodio.gitlens

# Docker
code --install-extension ms-azuretools.vscode-docker
```

---

**Pronto para começar!** 🚀

Boa sorte e happy coding! 💻✨
