# 🚀 Quick Start Guide - Prisma Prime Market API

Este guia rápido vai te ajudar a configurar o projeto em minutos.

## 📋 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- ✅ [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- ✅ [PostgreSQL 15+](https://www.postgresql.org/download/) ou [Docker](https://www.docker.com/)
- ✅ [Git](https://git-scm.com/)
- ✅ IDE: [VS Code](https://code.visualstudio.com/)

## 🎯 Setup em 5 Minutos

### 1️⃣ Clone o Repositório

```bash
git clone https://github.com/jorelrx/PrismaPrimeMarketAPI.git
cd PrismaPrimeMarketAPI
```

### 2️⃣ Configure o Banco de Dados

**Opção A: PostgreSQL Local**
```bash
# Crie o banco de dados
psql -U postgres -c "CREATE DATABASE prismaprimemarketapi;"
```

**Opção B: Docker (Recomendado)**
```bash
# Inicie PostgreSQL no Docker
docker run --name postgres ^
  -e POSTGRES_PASSWORD=YourStrong@Passw0rd ^
  -e POSTGRES_DB=prismaprimemarketapi ^
  -p 5432:5432 ^
  -d postgres:16-alpine
```

### 3️⃣ Configure as Variáveis de Ambiente

Crie `src/PrismaPrimeMarket.API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=prismaprimemarketapi;Username=postgres;Password=YourStrong@Passw0rd;"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-change-this-in-production-min-32-chars",
    "Issuer": "PrismaPrimeMarketAPI",
    "Audience": "PrismaPrimeMarketClient",
    "ExpirationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 4️⃣ Restaure as Dependências

```bash
dotnet restore
```

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
