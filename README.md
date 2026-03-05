# Microservices Assignment

A microservices-based system built with .NET 10, featuring Clean Architecture, CQRS, JWT authentication, PostgreSQL, and MongoDB.

---

## Architecture Overview

```
Client
  ↓
ApiGateway (port 5000)        ← Single entry point, YARP reverse proxy
  ↓               ↓
UserService     ContentService
(port 5001)     (port 5002)
PostgreSQL      MongoDB
```

### Services

| Service | Description | Database |
|---|---|---|
| `ApiGateway` | Reverse proxy, single entry point | — |
| `UserService` | Registration, login, JWT issuing | PostgreSQL |
| `ContentService` | Content creation and retrieval | MongoDB |

---

## Project Structure

```
Microservices/
  docker-compose.yml
  Gateway/
    ApiGateway/
  Services/
    UserService/
      UserService.API/
      UserService.Application/
      UserService.Domain/
      UserService.Infrastructure/
    ContentService/
      ContentService.API/
      ContentService.Application/
      ContentService.Domain/
      ContentService.Infrastructure/
  Shared/
    SharedKernel/
```

---

## Getting Started

### Option A — Run with Docker (Recommended)

#### 1. Clone the repository

```bash
git clone <repository-url>
cd Microservices
```

#### 2. Create `.env` file in the root

```env
JWT_KEY=my-super-secret-key-minimum-32-characters!!
JWT_ISSUER=UserService
JWT_AUDIENCE=ApiGateway

POSTGRES_PASSWORD=secret123
POSTGRES_DB=userservice_db

MONGO_DB=content_db
```

#### 3. Start all services

```bash
docker-compose up --build
```

#### 4. Open Swagger UI

```
http://localhost:5000/swagger
```

---

### Option B — Run Locally with Visual Studio

#### 1. Start databases

**Option 1 — via Docker (recommended):**
```bash
docker-compose up postgres mongo
```

**Option 2 — install locally:**
- [PostgreSQL 16](https://www.postgresql.org/download/) — default port `5432`
- [MongoDB 7](https://www.mongodb.com/try/download/community) — default port `27017`

> If installed locally — make sure both services are running before starting the application.

#### 2. Configure User Secrets

> ⚠️ `Jwt:Key` must be **identical** in both services and **minimum 32 characters** long

**UserService:**

```bash
cd Services/UserService/UserService.API

dotnet user-secrets set "Jwt:Key" "my-super-secret-key-minimum-32-characters!!"
dotnet user-secrets set "Jwt:Issuer" "UserService"
dotnet user-secrets set "Jwt:Audience" "ApiGateway"
dotnet user-secrets set "ConnectionStrings:Postgres" "Host=localhost;Port=5432;Database=userservice_db;Username=postgres;Password=secret123"
```

**ContentService:**

```bash
cd Services/ContentService/ContentService.API

dotnet user-secrets set "Jwt:Key" "my-super-secret-key-minimum-32-characters!!"
dotnet user-secrets set "Jwt:Issuer" "UserService"
dotnet user-secrets set "Jwt:Audience" "ApiGateway"
dotnet user-secrets set "MongoDB:ConnectionString" "mongodb://localhost:27017"
dotnet user-secrets set "MongoDB:Database" "content_db"
```

#### 3. Set Multiple Startup Projects in Visual Studio

```
Right click on Solution
→ Set Startup Projects
→ Multiple startup projects
→ Set Action to "Start" for:
   ✅ ApiGateway          (port 5000)
   ✅ UserService.API     (port 5001)
   ✅ ContentService.API  (port 5002)
→ OK
```

#### 4. Run

```
Press F5
```

---

## Testing the API

### Swagger UI URLs

| Service | URL |
|---|---|
| API Gateway | `http://localhost:5000/swagger` |
| UserService | `http://localhost:5001` |
| ContentService | `http://localhost:5002` |

---

### Step by Step

#### Step 1 — Register a user

```
POST /api/Users/Register
```

```json
{
  "login": "testuser",
  "password": "password123",
  "fullName": "Test User"
}
```

Expected response: `201 Created`

---

#### Step 2 — Login and get token

```
POST /api/Users/Login
```

```json
{
  "login": "testuser",
  "password": "password123"
}
```

Expected response:

```json
{
  "accessToken": "eyJhbGci..."
}
```

> Copy the `accessToken` value

---

#### Step 3 — Authorize in Swagger

Click 🔒 **Authorize** button → enter:

```
Bearer eyJhbGci...
```

---

#### Step 4 — Create text content

```
POST /api/Content/Create
```

```json
{
  "name": "My Article",
  "payload": {
    "type": "text",
    "text": "Hello World"
  }
}
```

Expected response: `201 Created`

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

---

#### Step 5 — Create media content

```
POST /api/Content/Create
```

```json
{
  "name": "My Video",
  "payload": {
    "type": "media",
    "url": "https://example.com/video.mp4",
    "mediaType": "Video"
  }
}
```

Supported `mediaType` values: `Image`, `Video`, `Audio`

---

#### Step 6 — Get content by id

```
GET /api/Content/GetById/{id}
```

Expected response: `200 OK`

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "My Article",
  "createdBy": "200809bf-d446-447a-b1de-672906a1c87f",
  "payload": {
    "type": "text",
    "text": "Hello World"
  }
}
```

---

## HTTP Response Codes

| Code | Description |
|---|---|
| `200 OK` | Successful retrieval |
| `201 Created` | Resource successfully created |
| `400 Bad Request` | Validation error |
| `401 Unauthorized` | Missing or invalid JWT token |
| `403 Forbidden` | Authenticated but not allowed |
| `404 Not Found` | Resource does not exist |
| `409 Conflict` | Login already exists |
| `500 Internal Server Error` | Database or server error |

---

## Payload Types

| Type | Required Fields |
|---|---|
| `text` | `text` (string) |
| `media` | `url` (string), `mediaType` (`Image` / `Video` / `Audio`) |
