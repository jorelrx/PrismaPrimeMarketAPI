# ADR 002: Escolha do Banco de Dados

**Status:** Aceito  
**Data:** 2026-01-09  
**Decisão:** Joel Victor  
**Contexto:** Escolha da tecnologia de persistência de dados

---

## Contexto

Precisávamos escolher um banco de dados que atendesse aos requisitos do marketplace:

- Transações ACID para operações críticas (pedidos, pagamentos)
- Suporte a relacionamentos complexos (produtos, categorias, usuários, pedidos)
- Performance para queries complexas
- Escalabilidade
- Ferramentas robustas de administração
- Suporte a migrations
- Boa integração com .NET/Entity Framework Core
- **Suporte avançado para IA e vetores (integração futura com Agentes IA)**
- Suporte a JSON nativo e tipos avançados

## Decisão

Decidimos usar **PostgreSQL** como banco de dados relacional principal, com **Redis** para cache distribuído.

### PostgreSQL como Principal

```
Products ←→ Categories
    ↓
OrderItems ←→ Orders ←→ Users
    ↓           ↓
Reviews     Payments
```

### Redis para Cache

- Session data
- Frequently accessed data
- Rate limiting counters
- Temporary data

### Extensões PostgreSQL para IA

- **pgvector**: Armazenamento e busca de embeddings vetoriais
- **pg_trgm**: Busca fuzzy e similaridade de texto
- **jsonb**: Dados semi-estruturados e flexíveis

## Consequências

### Positivas ✅

**PostgreSQL:**
- **ACID Compliance**: Garantia de consistência transacional
- **Relacionamentos**: Suporte nativo a foreign keys e joins
- **Open Source**: Sem custos de licenciamento
- **EF Core**: Excelente integração via Npgsql
- **Performance**: Indexes avançados, query optimization, EXPLAIN ANALYZE
- **Escalabilidade**: Replication, partitioning, sharding
- **Backup/Recovery**: Ferramentas robustas (pg_dump, pg_basebackup)
- **Segurança**: Row-level security, encryption, auditing
- **JSON Nativo**: JSONB com indexação e queries eficientes
- **Extensibilidade**: Sistema de extensões robusto
- **IA Ready**: 
  - pgvector para embeddings e semantic search
  - Integração com LangChain, LlamaIndex
  - Full-text search nativo
  - Vetores para RAG (Retrieval-Augmented Generation)
- **Multi-Cloud**: Funciona identicamente em Azure, AWS, GCP
- **Comunidade**: Grande comunidade ativa e documentação extensa

**Redis:**
- **Performance**: Operações em memória extremamente rápidas
- **TTL**: Expiração automática de dados
- **Pub/Sub**: Suporte a messaging patterns
- **Distributed**: Cache compartilhado entre instâncias

### Negativas ❌

**PostgreSQL:**
- **Configuração Inicial**: Pode requerer mais ajustes de performance
- **Ferramentas Windows**: Menos integrado ao ecossistema Microsoft que SQL Server

**Redis:**
- **Persistência**: Dados em memória (voláteis por natureza)
- **Complexidade**: Mais uma tecnologia para gerenciar

### Neutras ⚖️

- **Cross-platform**: PostgreSQL funciona igualmente bem em Linux, Windows e macOS
- **Cloud**: Excelente suporte em todos os principais clouds
- **Tamanho**: PostgreSQL tem footprint similar ou menor que SQL Server

## Alternativas Consideradas

### 1. SQL Server
**Prós:**
- Ferramentas nativas Microsoft
- Familiar para equipes .NET
- Integração com Azure

**Contras:**
- Custo de licenciamento
- Menos adequado para IA/ML (sem pgvector nativo)
- Pior suporte multi-cloud

### 2. MongoDB
**Prós:**
- Flexibilidade de schema
- Escalabilidade horizontal
- Performance em reads

**Contras:**
- Falta de transações multi-documento robustas
- Não ideal para dados relacionais complexos
- Possíveis problemas de consistência

### 3. MySQL
**Prós:**
- Open source
- Amplamente usado
- Boa performance

**Contras:**
- Menos features avançadas que PostgreSQL
- Suporte a JSON inferior
- Sem extensões como pgvector

## Estratégia de Dados

### Dados Relacionais (PostgreSQL)
```sql
-- Produtos, Pedidos, Usuários, Pagamentos
-- Tudo que precisa de ACID e relacionamentos

-- Extensões para IA
CREATE EXTENSION IF NOT EXISTS vector;     -- pgvector para embeddings
CREATE EXTENSION IF NOT EXISTS pg_trgm;    -- Busca fuzzy
CREATE EXTENSION IF NOT EXISTS pg_stat_statements; -- Monitoramento de queries
```

### Dados de Cache (Redis)
```
product:123         → Product object (10 min TTL)
user:session:abc    → User session (30 min TTL)
ratelimit:ip:xyz    → Rate limit counter (1 min TTL)
```

### Preparação para IA
```sql
-- Exemplo de tabela com vetores para busca semântica
CREATE TABLE product_embeddings (
    product_id UUID PRIMARY KEY,
    embedding vector(1536),  -- OpenAI embeddings dimension
    updated_at TIMESTAMP DEFAULT NOW()
);

-- Índice para busca de similaridade
CREATE INDEX ON product_embeddings 
USING ivfflat (embedding vector_cosine_ops);
```

### Futura Consideração: NoSQL
Para analytics, logs e dados não estruturados:
- **Elasticsearch**: Busca avançada de produtos
- **MongoDB**: Logs de auditoria, analytics

## Configuração

### Connection Strings
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=prismaprimemarketapi;Username=postgres;Password=YourPassword;",
    "Redis": "localhost:6379"
  }
}
```

### Entity Framework Core
- Code-First approach
- Migrations para versionamento de schema
- Npgsql provider para PostgreSQL
- Suporte a tipos PostgreSQL específicos (JSONB, Arrays, etc.)

### Extensões PostgreSQL Recomendadas
```sql
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";        -- UUID generation
CREATE EXTENSION IF NOT EXISTS "pgcrypto";         -- Cryptography
CREATE EXTENSION IF NOT EXISTS "vector";           -- Vector similarity (IA)
CREATE EXTENSION IF NOT EXISTS "pg_trgm";          -- Trigram matching
CREATE EXTENSION IF NOT EXISTS "btree_gin";        -- GIN indexes for btree
CREATE EXTENSION IF NOT EXISTS "pg_stat_statements"; -- Query monitoring
```

## Performance

### Indexes
```sql
-- Indexes em colunas de busca frequente
CREATE INDEX idx_products_category_id ON products(category_id);
CREATE INDEX idx_orders_user_id ON orders(user_id);
CREATE INDEX idx_products_name_trgm ON products USING gin(name gin_trgm_ops); -- Busca fuzzy

-- Index para full-text search
CREATE INDEX idx_products_search ON products 
USING gin(to_tsvector('portuguese', name || ' ' || description));

-- Index para busca de similaridade vetorial (IA)
CREATE INDEX idx_product_embeddings_vector ON product_embeddings 
USING ivfflat (embedding vector_cosine_ops) WITH (lists = 100);
```

### Cache Strategy
```
1. Verifica cache (Redis)
2. Se não encontrar, busca no DB
3. Armazena no cache com TTL
4. Retorna resultado
```

## Backup Strategy

- **pg_dump**: Backup lógico diário (3 AM)
- **pg_basebackup**: Backup físico semanal
- **WAL Archiving**: Continuous archiving para PITR
- **Retenção**: 30 dias
- **Disaster Recovery**: Réplicas geo-distribuídas (Streaming Replication)

## Migração

### Development
```bash
# Adicionar pacote Npgsql
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

# Criar migration
dotnet ef migrations add MigrationName

# Aplicar migration
dotnet ef database update
```

### Production
- Script migrations com `dotnet ef migrations script`
- Review manual
- Apply em maintenance window
- Rollback plan pronto

## Monitoramento

- Query performance tracking via pg_stat_statements
- Slow query logging
- Index usage statistics (pg_stat_user_indexes)
- Cache hit/miss ratio
- Connection pool monitoring
- pgAdmin ou DBeaver para administração visual

## Integração com IA (Futuro)

### Casos de Uso
1. **Busca Semântica de Produtos**
   - Converter descrições de produtos em embeddings
   - Buscar produtos similares por significado, não apenas palavras-chave

2. **Recomendações Personalizadas**
   - Vetores de preferências do usuário
   - Matching com vetores de produtos

3. **RAG (Retrieval-Augmented Generation)**
   - Base de conhecimento vetorizada
   - Agentes IA podem consultar contexto relevante

4. **Detecção de Fraude**
   - Padrões de comportamento em vetores
   - Anomaly detection

### Stack de IA Sugerida
```
PostgreSQL (pgvector)
    ↓
LangChain / LlamaIndex
    ↓
OpenAI API / Ollama (local)
    ↓
Agentes IA
```

## Referências

- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Entity Framework Core - PostgreSQL Provider](https://www.npgsql.org/efcore/)
- [pgvector - Vector Similarity Search](https://github.com/pgvector/pgvector)
- [Redis Documentation](https://redis.io/documentation)
- [Database Design Best Practices](https://www.postgresql.org/docs/current/ddl.html)
- [PostgreSQL for AI Applications](https://supabase.com/blog/pgvector-vs-pinecone)

## Revisão Futura

Esta decisão deve ser revisada:
- Se requisitos de escalabilidade mudarem drasticamente
- Após implementação completa de features de IA
- Se houver necessidade específica de SQL Server (ex: integração com Azure específica)
- Após 1 ano de operação em produção

---

**Última atualização:** 2026-01-09
