# Personal Finance API

A REST API for managing personal finances and bank accounts. The application supports user registration and authentication, account management, financial operations, and operation history.

## Features

- **User management** — user registration.
- **Authentication and authorization** — JWT-based authentication and role-based authorization.
- **Account management** — create, view, and close bank accounts.
- **Financial operations** — deposit funds, withdraw funds, and transfer money between accounts.
- **Operation history** — view account history with filtering by operation type and date, and pagination.
- **Account ownership** — users can access and manage only their own accounts.
- **Concurrency control** — optimistic concurrency control prevents updates based on stale account data.

## Architecture

The solution follows Clean Architecture principles and is divided into four layers:

- **PersonalFinance.Domain** — domain entities, business rules, and domain logic.
- **PersonalFinance.Application** — application use cases, commands, queries, handlers, DTOs, and contracts for external components.
- **PersonalFinance.Infrastructure** — implementations of application contracts, data access, repositories, Entity Framework Core configuration, and PostgreSQL integration.
- **PersonalFinanceApi** — ASP.NET Core REST API, HTTP endpoints, authentication, authorization, and dependency registration.

Dependencies are directed inward. The Infrastructure layer implements contracts defined by the Application layer, while the Domain layer remains independent of infrastructure technologies. This makes it possible to replace technical implementations without changing business logic.

The application uses MediatR to dispatch commands and queries. This keeps HTTP controllers thin and separates request handling from the API layer.

## Technologies

- **C# / .NET 10**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **PostgreSQL**
- **Npgsql**
- **MediatR**
- **JWT (JSON Web Token)**
- **Swagger / OpenAPI**
- **Docker Compose**
- **MSTest**

## Project Structure

```text
.
├── PersonalFinance.Domain/
├── PersonalFinance.Application/
├── PersonalFinance.Infrastructure/
├── PersonalFinanceApi/
│   └── Controllers/
├── PersonalFinanceApiTest/
├── docker-compose.yml
├── PersonalFinanceApi.slnx
└── README.md
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- Docker Desktop (or Docker Engine with Docker Compose)
- Git

### 1. Clone the repository

```bash
git clone https://github.com/andreyrusov2106/personal-finance-api.git
cd personal-finance-api
```

### 2. Start PostgreSQL

The Docker Compose configuration starts PostgreSQL 16 and exposes it on port `5432`.

Create a local `.env` file from the example:

**PowerShell**

```powershell
Copy-Item .env.example .env
```

**Linux / macOS**

```bash
cp .env.example .env
```

Set your own value for `POSTGRES_PASSWORD` in `.env`, then start the database:

```bash
docker compose up -d
```

The container creates the `personal_finance` database and uses the `postgres` user. The named Docker volume `postgres_data` persists database files between container restarts.

### 3. Configure application secrets

The API reads its database connection string from `ConnectionStrings:DefaultConnection` and its signing key from `Jwt:Key`.

The project already has a `UserSecretsId`, so there is no need to initialize User Secrets again. From the repository root, configure the values for the API project:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=personal_finance;Username=postgres;Password=<your-postgres-password>" --project PersonalFinanceApi

dotnet user-secrets set "Jwt:Key" "<your-long-random-signing-key>" --project PersonalFinanceApi
```

Use a long random signing key (at least 32 bytes). The JWT issuer, audience, and token lifetime are configured in `PersonalFinanceApi/appsettings.json`.

Do not commit `.env`, connection strings, passwords, or JWT signing keys to source control. The repository contains `.env.example` only as a template.

### 4. Apply database migrations

Install the Entity Framework Core CLI tool if it is not already installed:

```bash
dotnet tool install --global dotnet-ef --version 10.0.12
```

Apply the migrations from the Infrastructure project using the API project as the startup project:

```bash
dotnet ef database update --project PersonalFinance.Infrastructure --startup-project PersonalFinanceApi
```

### 5. Run the API

Run the API using the HTTP launch profile:

```bash
dotnet run --project PersonalFinanceApi --launch-profile http
```

With the default launch settings, the API is available at:

- API base URL: `http://localhost:5207`
- Swagger UI: `http://localhost:5207/swagger`
- OpenAPI document: `http://localhost:5207/openapi/v1.json`

Swagger UI is enabled in the Development environment.

## Configuration Reference

| Configuration key | Purpose |
|---|---|
| `ConnectionStrings:DefaultConnection` | Connection string for the application database |
| `ConnectionStrings:TestConnection` | Connection string used by integration tests |
| `Jwt:Key` | Secret key used to sign and validate JWTs |
| `Jwt:Issuer` | Token issuer |
| `Jwt:Audience` | Token audience |
| `Jwt:ExpirationMinutes` | Token lifetime in minutes |

For local development, use .NET User Secrets for sensitive values. For deployment, provide the corresponding configuration through environment variables or a dedicated secrets provider.

## API Overview

All endpoints are exposed by the `PersonalFinanceApi` project.

### Authentication

| Method | Endpoint | Description | Authorization |
|---|---|---|---|
| `POST` | `/api/auth/register` | Register a user | Public |
| `POST` | `/api/auth/login` | Authenticate and receive a JWT | Public |
| `GET` | `/api/auth/me` | Get information about the authenticated user | Bearer token |

### Accounts and financial operations

| Method | Endpoint | Description | Authorization |
|---|---|---|---|
| `GET` | `/api/accounts` | Get the current user's accounts | Bearer token |
| `GET` | `/api/accounts/{id}` | Get an account by ID | Bearer token |
| `POST` | `/api/accounts` | Create an account | Bearer token |
| `PUT` | `/api/accounts/{id}` | Close an account | Bearer token |
| `POST` | `/api/accounts/{id}/deposit` | Deposit funds | Bearer token |
| `POST` | `/api/accounts/{id}/withdraw` | Withdraw funds | Bearer token |
| `POST` | `/api/accounts/{fromAccountId}/transfer` | Transfer funds to another account | Bearer token |
| `GET` | `/api/accounts/{accountId}/history` | Get account operation history | Bearer token |
| `GET` | `/api/accounts/admin` | Example of an admin-only endpoint | Admin role |

The account history endpoint supports the following query parameters:

- `page` — page number (default: `1`).
- `pageSize` — number of items per page (default: `20`).
- `type` — optional operation type: `Deposit`, `Withdrawal`, or `Transfer`.
- `from` — optional start date.
- `to` — optional end date (inclusive).

Example:

```http
GET /api/accounts/{accountId}/history?page=1&pageSize=20&type=Transfer&from=2026-10-01&to=2026-10-31
Authorization: Bearer <jwt-token>
```

### Authentication flow

1. Register a user through `POST /api/auth/register`.
2. Authenticate through `POST /api/auth/login` and receive a JWT.
3. Pass the token in the `Authorization` header for protected endpoints:

```http
Authorization: Bearer <jwt-token>
```

## Operation History

Financial operations are recorded in the operation history.

A deposit or withdrawal creates an operation with one transaction entry. A transfer creates one operation associated with two transaction entries: a debit from the source account and a credit to the destination account. Transaction amounts are signed, which allows incoming and outgoing movements to be represented consistently.

## Testing

Run the test suite from the repository root:

```bash
dotnet test PersonalFinanceApi.slnx
```

The test project uses MSTest and includes unit and integration tests for application behavior, account ownership, operation history, concurrency handling, and API exception mapping.

**Important:** integration tests use the dedicated `ConnectionStrings:TestConnection` database. The custom test application factory deletes and recreates that database before applying migrations. Configure it to point to a separate test database (for example, `personal_finance_test`) and never point it to your development or production database.

Example configuration:

```bash
dotnet user-secrets set "ConnectionStrings:TestConnection" "Host=localhost;Port=5432;Database=personal_finance_test;Username=postgres;Password=<your-postgres-password>" --project PersonalFinanceApi
```

## Error Handling

The API uses a centralized exception handler and returns errors in the Problem Details format. Domain and application errors are mapped to appropriate HTTP responses instead of exposing unhandled exceptions to API clients.
