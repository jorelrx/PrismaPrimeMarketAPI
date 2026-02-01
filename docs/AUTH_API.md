# API de Autenticação JWT

## Visão Geral

A API de autenticação JWT fornece endpoints seguros para gerenciar autenticação de usuários usando JSON Web Tokens (JWT).

## Endpoints

### 1. Login

Autentica um usuário e retorna tokens de acesso e refresh.

O login pode ser feito usando **email** ou **username** no campo `usernameOrEmail`.

**Endpoint:** `POST /api/v1/auth/login`

**Request Body (usando email):**
```json
{
  "usernameOrEmail": "user@example.com",
  "password": "StrongP@ssw0rd"
}
```

**Request Body (usando username):**
```json
{
  "usernameOrEmail": "johndoe",
  "password": "StrongP@ssw0rd"
}
```

**Response (200 OK):**
```json
{
  "user": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "email": "user@example.com",
    "userName": "username",
    "firstName": "John",
    "lastName": "Doe",
    "roles": ["Customer"]
  },
  "tokens": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "base64-encoded-token",
    "accessTokenExpiresAt": "2026-01-30T19:00:00Z",
    "refreshTokenExpiresAt": "2026-02-06T18:45:00Z"
  }
}
```

### 2. Refresh Token

Renova o access token usando um refresh token válido.

**Endpoint:** `POST /api/v1/auth/refresh`

**Request Body:**
```json
{
  "refreshToken": "base64-encoded-token"
}
```

**Response (200 OK):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "new-base64-encoded-token",
  "accessTokenExpiresAt": "2026-01-30T19:00:00Z",
  "refreshTokenExpiresAt": "2026-02-06T18:45:00Z"
}
```

### 3. Solicitar Reset de Senha

Envia um email com token de reset de senha.

**Endpoint:** `POST /api/v1/auth/password/reset-request`

**Request Body:**
```json
{
  "email": "user@example.com"
}
```

**Response (200 OK):**
```json
{
  "message": "Se o email existe, você receberá instruções para resetar sua senha"
}
```

### 4. Confirmar Reset de Senha

Reseta a senha usando o token recebido por email.

**Endpoint:** `POST /api/v1/auth/password/reset-confirm`

**Request Body:**
```json
{
  "token": "password-reset-token",
  "newPassword": "NewStrongP@ssw0rd"
}
```

**Response (200 OK):**
```json
{
  "message": "Senha resetada com sucesso"
}
```

## Autenticação em Endpoints Protegidos

Para acessar endpoints protegidos, inclua o access token no header `Authorization`:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## Códigos de Status

- **200 OK**: Requisição bem-sucedida
- **400 Bad Request**: Dados de entrada inválidos
- **401 Unauthorized**: Credenciais inválidas ou token expirado
- **404 Not Found**: Recurso não encontrado
- **500 Internal Server Error**: Erro do servidor

## Configuração

### Variáveis de Ambiente

Configure as seguintes variáveis no `appsettings.json` ou variáveis de ambiente:

```json
{
  "Jwt": {
    "AccessSecret": "your-super-secret-key-change-this-in-production-min-32-chars",
    "AccessExpiration": "15m",
    "RefreshExpiration": "7d",
    "Issuer": "PrismaPrimeMarket",
    "Audience": "PrismaPrimeMarket"
  }
}
```

### Requisitos de Senha

- Mínimo de 8 caracteres
- Pelo menos uma letra maiúscula
- Pelo menos uma letra minúscula
- Pelo menos um número
- Pelo menos um caractere especial

## Segurança

### Boas Práticas

1. **Secrets**: Nunca commite secrets em código. Use variáveis de ambiente ou Azure Key Vault
2. **HTTPS**: Use sempre HTTPS em produção
3. **Token Rotation**: Implemente rotação de refresh tokens
4. **Rate Limiting**: Configure rate limiting nos endpoints de autenticação
5. **Logging**: Monitore tentativas de login falhadas

### Tokens

- **Access Token**: 
  - Curta duração (15 minutos por padrão)
  - Contém claims do usuário (ID, email, roles)
  - Usado para autorização em cada requisição

- **Refresh Token**: 
  - Longa duração (7 dias por padrão)
  - Armazenado no banco de dados
  - Pode ser revogado
  - Usado apenas para renovar access tokens

## Testando com Swagger

1. Acesse `/swagger`
2. Use o endpoint `/api/v1/auth/login` para obter tokens
3. Clique no botão "Authorize" no topo
4. Digite: `Bearer {seu-access-token}`
5. Agora você pode testar endpoints protegidos

## Exemplos com cURL

### Login
```bash
curl -X POST "https://localhost:5001/api/v1/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "StrongP@ssw0rd"
  }'
```

### Usar Token em Requisição Protegida
```bash
curl -X GET "https://localhost:5001/api/v1/users/me" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### Refresh Token
```bash
curl -X POST "https://localhost:5001/api/v1/auth/refresh" \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "your-refresh-token"
  }'
```
