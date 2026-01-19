# API Documentation - Prisma Prime Market API

## 📋 Visão Geral

A Prisma Prime Market API é uma API RESTful que fornece endpoints completos para gerenciamento de um marketplace. Este documento descreve todos os endpoints disponíveis, formatos de requisição/resposta, códigos de status e exemplos de uso.

## 🔗 Base URL

```
Development: https://localhost:5001/api/v1
Production: https://api.prismaprime.market.com/api/v1
```

## 🔐 Autenticação

A API utiliza autenticação baseada em **JWT (JSON Web Tokens)**. Para acessar endpoints protegidos, inclua o token no header:

```http
Authorization: Bearer {seu_token_jwt}
```

### Obter Token

**POST** `/auth/login`

```json
Request:
{
  "email": "usuario@example.com",
  "password": "SenhaSegura123!"
}

Response: 200 OK
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "def50200a1b2c3d4...",
  "expiresIn": 3600,
  "user": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "email": "usuario@example.com",
    "name": "João Silva",
    "role": "Customer"
  }
}
```

### Refresh Token

**POST** `/auth/refresh`

```json
Request:
{
  "refreshToken": "def50200a1b2c3d4..."
}

Response: 200 OK
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "new_refresh_token_here",
  "expiresIn": 3600
}
```

---

## 📦 Produtos (Products)

### Listar Produtos

**GET** `/products`

Query Parameters:
- `pageNumber` (int, opcional, default: 1)
- `pageSize` (int, opcional, default: 20, max: 100)
- `categoryId` (guid, opcional)
- `search` (string, opcional)
- `minPrice` (decimal, opcional)
- `maxPrice` (decimal, opcional)
- `sortBy` (string, opcional: "name", "price", "createdAt")
- `sortOrder` (string, opcional: "asc", "desc")

```json
Response: 200 OK
{
  "items": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "name": "Notebook Dell Inspiron 15",
      "description": "Notebook com processador Intel Core i7",
      "price": {
        "amount": 3500.00,
        "currency": "BRL",
        "formattedValue": "R$ 3.500,00"
      },
      "stock": 15,
      "isActive": true,
      "categoryId": "234e5678-e89b-12d3-a456-426614174001",
      "categoryName": "Informática",
      "imageUrl": "https://storage.example.com/products/notebook-dell.jpg",
      "createdAt": "2025-01-01T10:00:00Z",
      "updatedAt": "2025-01-05T15:30:00Z"
    }
  ],
  "pageNumber": 1,
  "pageSize": 20,
  "totalPages": 5,
  "totalCount": 95,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

### Obter Produto por ID

**GET** `/products/{id}`

```json
Response: 200 OK
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "name": "Notebook Dell Inspiron 15",
  "description": "Notebook com processador Intel Core i7, 16GB RAM, SSD 512GB",
  "price": {
    "amount": 3500.00,
    "currency": "BRL",
    "formattedValue": "R$ 3.500,00"
  },
  "stock": 15,
  "isActive": true,
  "categoryId": "234e5678-e89b-12d3-a456-426614174001",
  "categoryName": "Informática",
  "images": [
    {
      "id": "img-001",
      "url": "https://storage.example.com/products/notebook-dell-1.jpg",
      "isPrimary": true
    },
    {
      "id": "img-002",
      "url": "https://storage.example.com/products/notebook-dell-2.jpg",
      "isPrimary": false
    }
  ],
  "specifications": [
    {
      "key": "Processador",
      "value": "Intel Core i7 11ª Geração"
    },
    {
      "key": "Memória RAM",
      "value": "16GB DDR4"
    }
  ],
  "reviews": {
    "averageRating": 4.5,
    "totalReviews": 42
  },
  "createdAt": "2025-01-01T10:00:00Z",
  "updatedAt": "2025-01-05T15:30:00Z"
}

Response: 404 Not Found
{
  "type": "NotFound",
  "title": "Produto não encontrado",
  "status": 404,
  "detail": "O produto com ID '123e4567-e89b-12d3-a456-426614174000' não foi encontrado.",
  "traceId": "0HMV8D3P6V7QD:00000001"
}
```

### Criar Produto

**POST** `/products`  
🔒 Requer autenticação (Seller, Admin)

```json
Request:
{
  "name": "Notebook Dell Inspiron 15",
  "description": "Notebook com processador Intel Core i7, 16GB RAM, SSD 512GB",
  "price": 3500.00,
  "stock": 15,
  "categoryId": "234e5678-e89b-12d3-a456-426614174001",
  "specifications": [
    {
      "key": "Processador",
      "value": "Intel Core i7 11ª Geração"
    }
  ]
}

Response: 201 Created
Location: /api/v1/products/123e4567-e89b-12d3-a456-426614174000
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "name": "Notebook Dell Inspiron 15",
  "description": "Notebook com processador Intel Core i7, 16GB RAM, SSD 512GB",
  "price": {
    "amount": 3500.00,
    "currency": "BRL"
  },
  "stock": 15,
  "isActive": true,
  "categoryId": "234e5678-e89b-12d3-a456-426614174001",
  "createdAt": "2025-01-06T10:00:00Z"
}

Response: 400 Bad Request
{
  "type": "ValidationError",
  "title": "Erro de validação",
  "status": 400,
  "errors": {
    "Name": ["O nome do produto é obrigatório"],
    "Price": ["O preço deve ser maior que zero"]
  }
}
```

### Atualizar Produto

**PUT** `/products/{id}`  
🔒 Requer autenticação (Seller, Admin)

```json
Request:
{
  "name": "Notebook Dell Inspiron 15 - Atualizado",
  "description": "Nova descrição",
  "price": 3200.00,
  "stock": 20,
  "categoryId": "234e5678-e89b-12d3-a456-426614174001"
}

Response: 200 OK
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "name": "Notebook Dell Inspiron 15 - Atualizado",
  "description": "Nova descrição",
  "price": {
    "amount": 3200.00,
    "currency": "BRL"
  },
  "stock": 20,
  "updatedAt": "2025-01-06T11:00:00Z"
}
```

### Deletar Produto

**DELETE** `/products/{id}`  
🔒 Requer autenticação (Admin)

```json
Response: 204 No Content
```

### Atualizar Estoque

**PATCH** `/products/{id}/stock`  
🔒 Requer autenticação (Seller, Admin)

```json
Request:
{
  "quantity": 10,
  "operation": "add" // ou "remove", "set"
}

Response: 200 OK
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "stock": 25
}
```

---

## 🛒 Pedidos (Orders)

### Listar Pedidos do Usuário

**GET** `/orders`  
🔒 Requer autenticação

```json
Response: 200 OK
{
  "items": [
    {
      "id": "345e6789-e89b-12d3-a456-426614174002",
      "orderNumber": "ORD-2025-001234",
      "status": "Processing",
      "totalAmount": {
        "amount": 3500.00,
        "currency": "BRL",
        "formattedValue": "R$ 3.500,00"
      },
      "itemCount": 2,
      "createdAt": "2025-01-05T10:00:00Z",
      "estimatedDelivery": "2025-01-15T00:00:00Z"
    }
  ],
  "pageNumber": 1,
  "pageSize": 20,
  "totalCount": 5
}
```

### Obter Pedido por ID

**GET** `/orders/{id}`  
🔒 Requer autenticação

```json
Response: 200 OK
{
  "id": "345e6789-e89b-12d3-a456-426614174002",
  "orderNumber": "ORD-2025-001234",
  "status": "Processing",
  "userId": "user-123",
  "items": [
    {
      "id": "item-001",
      "productId": "123e4567-e89b-12d3-a456-426614174000",
      "productName": "Notebook Dell Inspiron 15",
      "quantity": 1,
      "unitPrice": {
        "amount": 3500.00,
        "currency": "BRL"
      },
      "subtotal": {
        "amount": 3500.00,
        "currency": "BRL"
      }
    }
  ],
  "shippingAddress": {
    "street": "Rua das Flores",
    "number": "123",
    "complement": "Apto 45",
    "neighborhood": "Centro",
    "city": "São Paulo",
    "state": "SP",
    "zipCode": "01234-567",
    "country": "Brasil"
  },
  "payment": {
    "method": "CreditCard",
    "status": "Paid",
    "transactionId": "txn_123456",
    "paidAt": "2025-01-05T10:05:00Z"
  },
  "subtotal": {
    "amount": 3500.00,
    "currency": "BRL"
  },
  "shippingCost": {
    "amount": 50.00,
    "currency": "BRL"
  },
  "totalAmount": {
    "amount": 3550.00,
    "currency": "BRL"
  },
  "createdAt": "2025-01-05T10:00:00Z",
  "updatedAt": "2025-01-05T10:05:00Z"
}
```

### Criar Pedido

**POST** `/orders`  
🔒 Requer autenticação

```json
Request:
{
  "items": [
    {
      "productId": "123e4567-e89b-12d3-a456-426614174000",
      "quantity": 1
    }
  ],
  "shippingAddressId": "addr-123",
  "paymentMethod": "CreditCard"
}

Response: 201 Created
{
  "id": "345e6789-e89b-12d3-a456-426614174002",
  "orderNumber": "ORD-2025-001234",
  "status": "Pending",
  "totalAmount": {
    "amount": 3550.00,
    "currency": "BRL"
  },
  "paymentUrl": "https://payment.example.com/checkout/order-123",
  "createdAt": "2025-01-06T10:00:00Z"
}
```

### Cancelar Pedido

**POST** `/orders/{id}/cancel`  
🔒 Requer autenticação

```json
Request:
{
  "reason": "Desistência da compra"
}

Response: 200 OK
{
  "id": "345e6789-e89b-12d3-a456-426614174002",
  "status": "Cancelled",
  "cancelledAt": "2025-01-06T11:00:00Z"
}
```

---

## 👤 Usuários (Users)

### Registrar Usuário

**POST** `/users/register`

```json
Request:
{
  "name": "João Silva",
  "email": "joao.silva@example.com",
  "password": "SenhaSegura123!",
  "confirmPassword": "SenhaSegura123!",
  "cpf": "123.456.789-00",
  "phoneNumber": "(11) 98765-4321",
  "role": "Customer" // ou "Seller"
}

Response: 201 Created
{
  "id": "user-123",
  "name": "João Silva",
  "email": "joao.silva@example.com",
  "role": "Customer",
  "emailVerified": false,
  "createdAt": "2025-01-06T10:00:00Z"
}
```

### Obter Perfil do Usuário

**GET** `/users/profile`  
🔒 Requer autenticação

```json
Response: 200 OK
{
  "id": "user-123",
  "name": "João Silva",
  "email": "joao.silva@example.com",
  "cpf": "123.456.789-00",
  "phoneNumber": "(11) 98765-4321",
  "role": "Customer",
  "emailVerified": true,
  "addresses": [
    {
      "id": "addr-123",
      "street": "Rua das Flores",
      "number": "123",
      "city": "São Paulo",
      "state": "SP",
      "zipCode": "01234-567",
      "isDefault": true
    }
  ],
  "createdAt": "2025-01-01T10:00:00Z"
}
```

### Atualizar Perfil

**PUT** `/users/profile`  
🔒 Requer autenticação

```json
Request:
{
  "name": "João Silva Santos",
  "phoneNumber": "(11) 91234-5678"
}

Response: 200 OK
{
  "id": "user-123",
  "name": "João Silva Santos",
  "phoneNumber": "(11) 91234-5678",
  "updatedAt": "2025-01-06T11:00:00Z"
}
```

---

## 💳 Pagamentos (Payments)

### Processar Pagamento

**POST** `/payments/process`  
🔒 Requer autenticação

```json
Request:
{
  "orderId": "345e6789-e89b-12d3-a456-426614174002",
  "paymentMethod": "CreditCard",
  "creditCard": {
    "cardNumber": "4111111111111111",
    "cardHolderName": "JOAO SILVA",
    "expiryMonth": "12",
    "expiryYear": "2026",
    "cvv": "123"
  }
}

Response: 200 OK
{
  "paymentId": "pay-123",
  "orderId": "345e6789-e89b-12d3-a456-426614174002",
  "status": "Approved",
  "transactionId": "txn_123456",
  "amount": {
    "amount": 3550.00,
    "currency": "BRL"
  },
  "paidAt": "2025-01-06T10:05:00Z"
}
```

### Webhook de Pagamento

**POST** `/payments/webhook`

Este endpoint recebe notificações de status de pagamento de gateways externos (Stripe, PayPal, etc.)

---

## ⭐ Avaliações (Reviews)

### Listar Avaliações de um Produto

**GET** `/products/{productId}/reviews`

```json
Response: 200 OK
{
  "items": [
    {
      "id": "review-123",
      "productId": "123e4567-e89b-12d3-a456-426614174000",
      "userId": "user-456",
      "userName": "Maria Santos",
      "rating": 5,
      "title": "Excelente produto!",
      "comment": "Produto de ótima qualidade, entrega rápida.",
      "isVerifiedPurchase": true,
      "createdAt": "2025-01-04T15:00:00Z"
    }
  ],
  "averageRating": 4.5,
  "totalReviews": 42,
  "ratingDistribution": {
    "5": 25,
    "4": 10,
    "3": 5,
    "2": 1,
    "1": 1
  }
}
```

### Criar Avaliação

**POST** `/products/{productId}/reviews`  
🔒 Requer autenticação

```json
Request:
{
  "rating": 5,
  "title": "Excelente produto!",
  "comment": "Produto de ótima qualidade, entrega rápida."
}

Response: 201 Created
{
  "id": "review-123",
  "productId": "123e4567-e89b-12d3-a456-426614174000",
  "rating": 5,
  "title": "Excelente produto!",
  "comment": "Produto de ótima qualidade, entrega rápida.",
  "createdAt": "2025-01-06T10:00:00Z"
}
```

---

## 📂 Categorias (Categories)

### Listar Categorias

**GET** `/categories`

```json
Response: 200 OK
{
  "items": [
    {
      "id": "234e5678-e89b-12d3-a456-426614174001",
      "name": "Informática",
      "slug": "informatica",
      "description": "Notebooks, desktops, periféricos",
      "productCount": 150,
      "parentId": null,
      "subcategories": [
        {
          "id": "cat-002",
          "name": "Notebooks",
          "slug": "notebooks",
          "productCount": 45
        }
      ]
    }
  ]
}
```

---

## 🔍 Busca (Search)

### Busca Avançada de Produtos

**GET** `/search`

Query Parameters:
- `q` (string, obrigatório): termo de busca
- `category` (string, opcional)
- `minPrice` (decimal, opcional)
- `maxPrice` (decimal, opcional)
- `page` (int, opcional)
- `pageSize` (int, opcional)

```json
Response: 200 OK
{
  "query": "notebook dell",
  "items": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "name": "Notebook Dell Inspiron 15",
      "price": {
        "amount": 3500.00,
        "currency": "BRL"
      },
      "imageUrl": "https://storage.example.com/products/notebook-dell.jpg",
      "rating": 4.5,
      "reviewCount": 42
    }
  ],
  "totalResults": 15,
  "pageNumber": 1,
  "pageSize": 20,
  "suggestions": ["notebook dell inspiron", "notebook dell latitude"]
}
```

---

## 📊 Códigos de Status HTTP

| Código | Significado | Uso |
|--------|-------------|-----|
| 200 | OK | Requisição bem-sucedida |
| 201 | Created | Recurso criado com sucesso |
| 204 | No Content | Operação bem-sucedida sem conteúdo de retorno |
| 400 | Bad Request | Dados inválidos na requisição |
| 401 | Unauthorized | Autenticação necessária ou falhou |
| 403 | Forbidden | Sem permissão para acessar o recurso |
| 404 | Not Found | Recurso não encontrado |
| 409 | Conflict | Conflito com estado atual do recurso |
| 422 | Unprocessable Entity | Validação de negócio falhou |
| 429 | Too Many Requests | Rate limit excedido |
| 500 | Internal Server Error | Erro interno do servidor |

---

## 🚨 Tratamento de Erros

Todas as respostas de erro seguem o padrão **RFC 7807 (Problem Details)**:

```json
{
  "type": "https://api.prismaprime.com/errors/validation-error",
  "title": "Erro de validação",
  "status": 400,
  "detail": "Um ou mais campos contêm erros de validação",
  "instance": "/api/v1/products",
  "traceId": "0HMV8D3P6V7QD:00000001",
  "errors": {
    "Name": ["O nome do produto é obrigatório"],
    "Price": ["O preço deve ser maior que zero"]
  }
}
```

---

## 🔄 Paginação

Todos os endpoints que retornam listas suportam paginação:

**Query Parameters:**
- `pageNumber` (default: 1)
- `pageSize` (default: 20, max: 100)

**Response Headers:**
```
X-Pagination: {"PageNumber":1,"PageSize":20,"TotalPages":5,"TotalCount":95}
```

**Response Body:**
```json
{
  "items": [...],
  "pageNumber": 1,
  "pageSize": 20,
  "totalPages": 5,
  "totalCount": 95,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

---

## 🔒 Rate Limiting

A API implementa rate limiting para prevenir abuso:

- **Requisições autenticadas**: 100 req/min
- **Requisições não autenticadas**: 20 req/min

**Headers de resposta:**
```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 2025-01-06T10:01:00Z
```

**Quando o limite é excedido (429):**
```json
{
  "type": "RateLimitExceeded",
  "title": "Limite de requisições excedido",
  "status": 429,
  "detail": "Você excedeu o limite de 100 requisições por minuto",
  "retryAfter": 45
}
```

---

## 📝 Versionamento

A API utiliza versionamento por URL:
- `/api/v1/...` - Versão 1 (atual)
- `/api/v2/...` - Versão 2 (futuro)

Para usar uma versão específica, inclua no path da URL.

---

## 🌍 Internacionalização

A API suporta múltiplos idiomas através do header `Accept-Language`:

```http
Accept-Language: pt-BR
Accept-Language: en-US
Accept-Language: es-ES
```

Mensagens de erro e validação serão retornadas no idioma especificado.

---

## 📦 Webhooks

A API permite registro de webhooks para eventos importantes:

### Eventos Disponíveis
- `order.created`
- `order.paid`
- `order.shipped`
- `order.delivered`
- `order.cancelled`
- `product.created`
- `product.updated`
- `payment.succeeded`
- `payment.failed`

### Registrar Webhook

**POST** `/webhooks`  
🔒 Requer autenticação (Admin)

```json
Request:
{
  "url": "https://seu-sistema.com/webhooks/orders",
  "events": ["order.created", "order.paid"],
  "secret": "seu_secret_para_validacao"
}

Response: 201 Created
{
  "id": "webhook-123",
  "url": "https://seu-sistema.com/webhooks/orders",
  "events": ["order.created", "order.paid"],
  "isActive": true,
  "createdAt": "2025-01-06T10:00:00Z"
}
```

---

## 🧪 Ambiente de Testes (Sandbox)

Para testes, utilize:

```
Base URL: https://sandbox.prismaprime.com/api/v1
```

### Dados de Teste

**Usuário de Teste:**
- Email: `test@example.com`
- Password: `Test123!`

**Cartões de Teste:**
- Sucesso: `4111111111111111`
- Falha: `4000000000000002`

---

## 📚 SDKs e Bibliotecas

### C# / .NET
```bash
dotnet add package PrismaPrimeMarket.SDK
```

```csharp
var client = new PrismaPrimeClient("your_api_key");
var products = await client.Products.ListAsync();
```

### JavaScript / TypeScript
```bash
npm install @prismaprime/sdk
```

```javascript
import { PrismaPrimeClient } from '@prismaprime/sdk';

const client = new PrismaPrimeClient({ apiKey: 'your_api_key' });
const products = await client.products.list();
```

---

## 🔗 Links Úteis

- **Swagger UI**: https://api.prismaprime.com/swagger
- **Postman Collection**: https://www.postman.com/prismaprime/workspace
- **Status Page**: https://status.prismaprime.com
- **Support**: support@prismaprime.com

---

**Versão da API**: 1.0  
**Última atualização**: Janeiro 2026
