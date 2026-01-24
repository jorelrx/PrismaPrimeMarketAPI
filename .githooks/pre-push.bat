@echo off
REM Pre-push hook para Windows
REM Roda os testes antes do push

echo 🔍 Executando testes antes do push...

REM Roda os testes
dotnet test --no-restore --verbosity minimal

REM Verifica o código de saída
if %ERRORLEVEL% neq 0 (
    echo.
    echo ❌ PUSH BLOQUEADO: Os testes falharam!
    echo    Corrija os erros antes de fazer push.
    exit /b 1
)

echo ✅ Todos os testes passaram! Prosseguindo com o push...
exit /b 0
