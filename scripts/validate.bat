@echo off
REM Script para validar código antes do commit/push

echo 🚀 Iniciando validação do código...
echo.

REM 0. Iniciar banco de dados de testes
echo ℹ️  Iniciando PostgreSQL para testes...
docker-compose -f docker-compose.test.yml up -d postgres
timeout /t 5 /nobreak >nul
echo ✅ PostgreSQL iniciado
echo.

REM 1. Restore packages
echo ℹ️  Restaurando pacotes...
dotnet restore --verbosity quiet
if %ERRORLEVEL% neq 0 (
    echo ❌ Falha ao restaurar pacotes
    exit /b 1
)
echo ✅ Pacotes restaurados
echo.

REM 2. Build
echo ℹ️  Compilando solução...
dotnet build --no-restore --verbosity quiet
if %ERRORLEVEL% neq 0 (
    echo ❌ Falha na compilação
    exit /b 1
)
echo ✅ Build concluído com sucesso
echo.

REM 3. Run tests
echo ℹ️  Executando testes...
dotnet test --no-build --verbosity normal --logger "console;verbosity=minimal"
if %ERRORLEVEL% neq 0 (
    echo ❌ Alguns testes falharam
    exit /b 1
)
echo ✅ Todos os testes passaram!
echo.

REM 4. Code formatting (opcional)
echo ℹ️  Verificando formatação do código...
dotnet format --verify-no-changes --verbosity quiet
if %ERRORLEVEL% neq 0 (
    echo ⚠️  Código não está formatado. Execute 'dotnet format' para corrigir.
    REM Não bloqueia o commit por formatação
)
echo.

REM 5. Parar banco de dados de testes
echo ℹ️  Parando PostgreSQL de testes...
docker-compose -f docker-compose.test.yml down >nul 2>&1
echo ✅ PostgreSQL parado
echo.

echo 🎉 Validação concluída com sucesso!
echo.
exit /b 0
