@echo off
REM Script para parar o banco de dados PostgreSQL de testes

echo Parando PostgreSQL de testes...
docker-compose -f docker-compose.test.yml down

echo PostgreSQL de testes parado!
