@echo off
REM Script para rodar testes com Docker Compose

echo 🐳 Rodando testes com Docker Compose...
echo.

REM Build e roda os testes
docker-compose -f docker-compose.test.yml up --build --abort-on-container-exit --exit-code-from tests

REM Captura o código de saída
set EXIT_CODE=%ERRORLEVEL%

REM Limpa os containers
echo.
echo 🧹 Limpando containers...
docker-compose -f docker-compose.test.yml down -v

REM Verifica resultado
if %EXIT_CODE% equ 0 (
    echo.
    echo ✅ Testes passaram com sucesso!
    echo.
    exit /b 0
) else (
    echo.
    echo ❌ Testes falharam!
    echo.
    exit /b 1
)
