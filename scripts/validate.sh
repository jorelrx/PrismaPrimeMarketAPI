#!/bin/bash
# Script para validar código antes do commit/push

set -e

echo "🚀 Iniciando validação do código..."
echo ""

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Função para imprimir mensagens coloridas
print_success() {
    echo -e "${GREEN}✅ $1${NC}"
}

print_error() {
    echo -e "${RED}❌ $1${NC}"
}

print_warning() {
    echo -e "${YELLOW}⚠️  $1${NC}"
}

print_info() {
    echo "ℹ️  $1"
}

# 1. Restore packages
print_info "Restaurando pacotes..."
if dotnet restore --verbosity quiet; then
    print_success "Pacotes restaurados"
else
    print_error "Falha ao restaurar pacotes"
    exit 1
fi

echo ""

# 2. Build
print_info "Compilando solução..."
if dotnet build --no-restore --verbosity quiet; then
    print_success "Build concluído com sucesso"
else
    print_error "Falha na compilação"
    exit 1
fi

echo ""

# 3. Run tests
print_info "Executando testes..."
if dotnet test --no-build --verbosity normal --logger "console;verbosity=minimal"; then
    print_success "Todos os testes passaram!"
else
    print_error "Alguns testes falharam"
    exit 1
fi

echo ""

# 4. Code formatting (opcional)
print_info "Verificando formatação do código..."
if dotnet format --verify-no-changes --verbosity quiet; then
    print_success "Código está formatado corretamente"
else
    print_warning "Código não está formatado. Execute 'dotnet format' para corrigir."
    # Não bloqueia o commit por formatação
fi

echo ""
print_success "🎉 Validação concluída com sucesso!"
echo ""
