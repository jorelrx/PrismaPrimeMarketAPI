@echo off
REM Script para iniciar o banco de dados PostgreSQL de testes

echo Iniciando PostgreSQL para testes de integração...
docker-compose -f docker-compose.test.yml up -d postgres

echo Aguardando PostgreSQL ficar pronto...
timeout /t 5 /nobreak >nul

echo PostgreSQL de testes pronto na porta 5433!
echo Connection String: Host=localhost;Port=5433;Database=PrismaPrimeMarketDB_Test;Username=postgres;Password=postgres
