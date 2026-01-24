# 🚀 Quick Reference - CI/CD

Comandos rápidos e úteis para trabalhar com o pipeline CI/CD.

## 📋 Comandos Locais

### Build & Testes

```bash
# Restore dependencies
dotnet restore

# Build (Debug)
dotnet build

# Build (Release)
dotnet build --configuration Release

# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific category
dotnet test --filter "Category=Unit"
dotnet test --filter "Category=Integration"

# Verbose output
dotnet test --verbosity detailed
```

### Code Quality

```bash
# Check formatting
dotnet format --verify-no-changes

# Fix formatting
dotnet format

# Check for vulnerable packages
dotnet list package --vulnerable --include-transitive

# Check for deprecated packages
dotnet list package --deprecated --include-transitive

# Update packages
dotnet list package --outdated
```

### Build com Análise

```bash
# Build with all analyzers
dotnet build \
  --configuration Release \
  /p:EnableNETAnalyzers=true \
  /p:AnalysisLevel=latest \
  /p:EnforceCodeStyleInBuild=true

# Build treating warnings as errors
dotnet build /p:TreatWarningsAsErrors=true
```

## 🏷️ Git Workflow

### Conventional Commits

```bash
# Features
git commit -m "feat: adicionar endpoint de produtos"
git commit -m "feat(auth): implementar JWT authentication"

# Bug fixes
git commit -m "fix: corrigir validação de email"
git commit -m "fix(orders): resolver cálculo de frete"

# Documentation
git commit -m "docs: atualizar README com exemplos"
git commit -m "docs(api): adicionar swagger documentation"

# Refactoring
git commit -m "refactor: reorganizar estrutura de pastas"
git commit -m "refactor(services): extrair interface comum"

# Tests
git commit -m "test: adicionar testes de integração"
git commit -m "test(domain): cobrir casos de borda"

# CI/CD
git commit -m "ci: configurar GitHub Actions"
git commit -m "ci: adicionar workflow de deploy"

# Chores
git commit -m "chore: atualizar dependências"
git commit -m "chore: configurar editorconfig"
```

### Branches

```bash
# Feature branch
git checkout -b feat/product-management
git checkout -b feat/user-authentication

# Bug fix branch
git checkout -b fix/cart-calculation
git checkout -b fix/login-validation

# Hotfix (from main)
git checkout main
git checkout -b hotfix/critical-security-issue

# Release branch
git checkout develop
git checkout -b release/v1.0.0
```

### Pull Requests

```bash
# Push feature branch
git push origin feat/my-feature

# Update PR with latest develop
git checkout feat/my-feature
git pull origin develop
git push origin feat/my-feature

# Delete branch after merge
git branch -d feat/my-feature
git push origin --delete feat/my-feature
```

## 🏃 GitHub Actions

### Trigger Manual Workflow

Via CLI (usando GitHub CLI):

```bash
# Install GitHub CLI
# Windows: winget install --id GitHub.cli
# Mac: brew install gh

# Login
gh auth login

# Run workflow
gh workflow run ci.yml
gh workflow run ci.yml --ref develop

# List runs
gh run list --workflow=ci.yml

# View run
gh run view

# Watch run in real-time
gh run watch
```

### Check Workflow Status

```bash
# List all workflows
gh workflow list

# View workflow details
gh workflow view ci.yml

# List recent runs
gh run list --limit 10

# Download artifacts
gh run download <run-id>
```

## 📊 Coverage

### Generate Local Coverage Report

```bash
# Install report generator
dotnet tool install -g dotnet-reportgenerator-globaltool

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Generate HTML report
reportgenerator \
  -reports:"**/coverage.cobertura.xml" \
  -targetdir:"coveragereport" \
  -reporttypes:Html

# Open report
start coveragereport/index.html  # Windows
open coveragereport/index.html   # Mac
xdg-open coveragereport/index.html # Linux
```

## 🐳 Docker (quando implementado)

```bash
# Build image
docker build -t prismaprime/api:latest .

# Run container
docker run -p 8080:80 prismaprime/api:latest

# Run with docker-compose
docker-compose up -d

# View logs
docker-compose logs -f api

# Stop
docker-compose down
```

## 🔖 Releases

### Create Release

```bash
# Create and push tag
git tag -a v1.0.0 -m "Release version 1.0.0"
git push origin v1.0.0

# Or use GitHub CLI
gh release create v1.0.0 \
  --title "v1.0.0" \
  --notes "Release notes here"

# Create pre-release
gh release create v1.0.0-beta.1 \
  --title "v1.0.0 Beta 1" \
  --notes "Beta release" \
  --prerelease
```

### Delete Tag/Release

```bash
# Delete local tag
git tag -d v1.0.0

# Delete remote tag
git push origin --delete v1.0.0

# Delete release (GitHub CLI)
gh release delete v1.0.0
```

## 🔍 Debugging CI Issues

### View GitHub Actions Logs

```bash
# Get latest run for workflow
gh run list --workflow=ci.yml --limit 1

# Download logs
gh run view <run-id> --log

# Download failed logs only
gh run view <run-id> --log-failed
```

### Test Workflow Locally with Act

```bash
# Install Act
# Windows: choco install act-cli
# Mac: brew install act

# List workflows
act -l

# Test specific job
act -j build

# Test with secrets
act -s CODECOV_TOKEN=fake-token

# Dry run
act -n

# Use specific platform
act -P ubuntu-latest=catthehacker/ubuntu:full-latest
```

## 📝 Common Tasks

### Before Creating PR

```bash
# Sync with develop
git checkout feat/my-feature
git fetch origin
git rebase origin/develop

# Format code
dotnet format

# Run all tests
dotnet test

# Check for issues
dotnet build --configuration Release

# Push
git push origin feat/my-feature --force-with-lease
```

### After PR Feedback

```bash
# Make changes
git add .
git commit -m "fix: address PR feedback"
git push origin feat/my-feature
```

### Emergency Hotfix

```bash
# From main
git checkout main
git pull origin main
git checkout -b hotfix/critical-fix

# Make fix
# ... code changes ...

# Test
dotnet test

# Commit
git add .
git commit -m "fix: critical security vulnerability"

# Push and create PR
git push origin hotfix/critical-fix
gh pr create --base main --title "Hotfix: Critical Security Fix"
```

## 🛠️ Maintenance

### Update Dependencies

```bash
# List outdated packages
dotnet list package --outdated

# Update specific package
dotnet add package PackageName

# Update all packages (use with caution)
dotnet tool install -g dotnet-outdated-tool
dotnet outdated --upgrade
```

### Clean Build

```bash
# Clean solution
dotnet clean

# Remove bin/obj folders
Get-ChildItem -Recurse -Filter "bin" | Remove-Item -Recurse -Force
Get-ChildItem -Recurse -Filter "obj" | Remove-Item -Recurse -Force

# Or in bash
find . -iname "bin" -o -iname "obj" | xargs rm -rf

# Restore and rebuild
dotnet restore
dotnet build
```

## 📚 Resources

- **GitHub CLI**: https://cli.github.com/
- **Act**: https://github.com/nektos/act
- **Codecov**: https://docs.codecov.com/
- **Conventional Commits**: https://www.conventionalcommits.org/

---

## 📤 Padrão de Response Pattern

### Response<T> - Estrutura

```csharp
public class Response<T>
{
    public T? Data { get; set; }
    public bool Succeeded { get; set; }
    public string[]? Errors { get; set; }
    public string Message { get; set; }
    public ResponseType Type { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Path { get; set; }
}
```

### ResponseType (Enum)

```csharp
public enum ResponseType
{
    // Success
    Success, Created, Updated, Deleted, Retrieved,
    
    // Errors
    NotFound, ValidationError, BadRequest, 
    Unauthorized, Forbidden, Conflict, InternalServerError
}
```

### Factory Methods

```csharp
// ✅ Sucesso
Response<ProductDto>.Success(data, "Mensagem opcional");
Response<ProductDto>.Created(data);
Response<ProductDto>.Updated(data);
Response<ProductDto>.Deleted();
Response<ProductDto>.Retrieved(data);

// ❌ Erro
Response<ProductDto>.NotFound("Recurso não encontrado");
Response<ProductDto>.ValidationError(new[] { "Erro 1", "Erro 2" });
Response<ProductDto>.BadRequest("Requisição inválida");
Response<ProductDto>.Unauthorized();
Response<ProductDto>.Forbidden();
Response<ProductDto>.Conflict("Conflito detectado");
Response<ProductDto>.InternalError("Erro interno");
```

### PagedResponse<T> - Estrutura

```csharp
public class PagedResponse<T> : Response<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; }
    public bool HasNextPage { get; }
}

// Factory
PagedResponse<IEnumerable<ProductDto>>.Create(
    data, 
    pageNumber: 1, 
    pageSize: 20, 
    totalRecords: 95
);
```

### ResponseMessages (Constantes)

```csharp
public static class ResponseMessages
{
    // Success
    public const string Success = "Operação realizada com sucesso";
    public const string Created = "Recurso criado com sucesso";
    public const string Updated = "Recurso atualizado com sucesso";
    public const string Deleted = "Recurso excluído com sucesso";
    public const string Retrieved = "Recurso recuperado com sucesso";
    public const string ListRetrieved = "Lista recuperada com sucesso";
    
    // Errors
    public const string NotFound = "Recurso não encontrado";
    public const string ValidationError = "Erro de validação nos dados fornecidos";
    public const string BadRequest = "Requisição inválida";
    public const string Unauthorized = "Não autorizado para acessar este recurso";
    public const string Forbidden = "Acesso negado a este recurso";
    public const string Conflict = "Conflito detectado ao processar a requisição";
    public const string InternalServerError = "Erro interno no servidor";
}
```

### Uso nos Handlers

```csharp
// Command Handler - Criar
public async Task<Response<ProductDto>> Handle(
    CreateProductCommand request, 
    CancellationToken cancellationToken)
{
    // Lógica de criação...
    var product = Product.Create(...);
    await _repository.AddAsync(product);
    await _unitOfWork.CommitAsync();
    
    var dto = _mapper.Map<ProductDto>(product);
    return Response<ProductDto>.Created(dto);
}

// Query Handler - GetById
public async Task<Response<ProductDto>> Handle(
    GetProductByIdQuery request,
    CancellationToken cancellationToken)
{
    var product = await _repository.GetByIdAsync(request.Id);
    
    if (product == null)
        return Response<ProductDto>.NotFound($"Produto {request.Id} não encontrado");
    
    var dto = _mapper.Map<ProductDto>(product);
    return Response<ProductDto>.Retrieved(dto);
}

// Query Handler - GetList (Paginado)
public async Task<PagedResponse<IEnumerable<ProductDto>>> Handle(
    GetProductListQuery request,
    CancellationToken cancellationToken)
{
    var query = _repository.GetQuery();
    var totalRecords = await _repository.CountAsync(query);
    
    query = query
        .Skip((request.PageNumber - 1) * request.PageSize)
        .Take(request.PageSize);
    
    var products = await _repository.ExecuteQueryAsync(query);
    var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);
    
    return PagedResponse<IEnumerable<ProductDto>>.Create(
        dtos,
        request.PageNumber,
        request.PageSize,
        totalRecords
    );
}
```

### Uso no Middleware (ExceptionHandlingMiddleware)

```csharp
catch (NotFoundException ex)
{
    context.Response.StatusCode = StatusCodes.Status404NotFound;
    var response = Response<object>.NotFound(ex.Message);
    await context.Response.WriteAsJsonAsync(response);
}
catch (ValidationException ex)
{
    context.Response.StatusCode = StatusCodes.Status400BadRequest;
    var response = Response<object>.ValidationError(
        ex.Errors.Select(e => e.ErrorMessage).ToArray()
    );
    await context.Response.WriteAsJsonAsync(response);
}
```

### Exemplo JSON - Sucesso

```json
{
  "data": { "id": "123", "name": "Produto" },
  "succeeded": true,
  "errors": null,
  "message": "Recurso criado com sucesso",
  "type": "Created",
  "timestamp": "2026-01-24T10:30:00Z",
  "path": "/api/v1/products"
}
```

### Exemplo JSON - Erro

```json
{
  "data": null,
  "succeeded": false,
  "errors": ["Nome é obrigatório", "Preço inválido"],
  "message": "Erro de validação nos dados fornecidos",
  "type": "ValidationError",
  "timestamp": "2026-01-24T10:30:00Z",
  "path": "/api/v1/products"
}
```

### Exemplo JSON - Paginado

```json
{
  "data": [ /* array de objetos */ ],
  "succeeded": true,
  "errors": null,
  "message": "Lista recuperada com sucesso",
  "type": "Retrieved",
  "timestamp": "2026-01-24T10:30:00Z",
  "path": "/api/v1/products",
  "pageNumber": 1,
  "pageSize": 20,
  "totalRecords": 95,
  "totalPages": 5,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

---

**Tip:** Add aliases to your shell profile for frequently used commands!

```bash
# ~/.bashrc or ~/.zshrc
alias dnb='dotnet build'
alias dnt='dotnet test'
alias dnr='dotnet run'
alias dnf='dotnet format'
alias dnc='dotnet clean'
```

---

**Última atualização:** 2026-01-19
