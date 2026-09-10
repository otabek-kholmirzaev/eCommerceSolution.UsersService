# eCommerce Users Service

A lightweight **Users microservice** for a distributed eCommerce system, built with **ASP.NET Core 8** and **PostgreSQL**.

The service provides user registration and login through a REST API and is structured using separate **API, Core, and Infrastructure** layers to keep business logic, contracts, and data access concerns isolated.

## Features

* User registration
* User login
* PostgreSQL persistence
* Email and request validation
* DTO-based API contracts
* AutoMapper-based object mapping
* Repository pattern
* Service layer abstraction
* Global exception-handling middleware
* Docker support
* Nullable reference types and implicit usings enabled

## Tech Stack

| Category          | Technology        |
| ----------------- | ----------------- |
| Language          | C#                |
| Framework         | ASP.NET Core 8    |
| Database          | PostgreSQL        |
| Data Access       | Dapper            |
| PostgreSQL Driver | Npgsql            |
| Validation        | FluentValidation  |
| Object Mapping    | AutoMapper        |
| API Documentation | Swagger / OpenAPI |
| Containerization  | Docker            |

The API targets .NET 8, while the Infrastructure layer uses Dapper and Npgsql for PostgreSQL connectivity.

## Architecture

```text
                    ┌─────────────────────┐
                    │       Client        │
                    └──────────┬──────────┘
                               │ HTTP
                               ▼
                    ┌─────────────────────┐
                    │     eCommerce.API   │
                    │  Controllers / HTTP │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │    eCommerce.Core   │
                    │                     │
                    │ Services             │
                    │ DTOs                │
                    │ Validators           │
                    │ Mappers              │
                    │ Contracts            │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │ eCommerce.Infrastructure │
                    │                     │
                    │ Repositories        │
                    │ Dapper               │
                    │ Npgsql               │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │     PostgreSQL      │
                    └─────────────────────┘
```

## Project Structure

```text
eCommerceSolution.UsersService/
├── eCommerce.API/
│   ├── Controllers/
│   │   └── AuthController.cs
│   ├── Middlewares/
│   │   └── ExceptionHandlingMiddleware.cs
│   ├── Program.cs
│   ├── Dockerfile
│   └── appsettings.json
│
├── eCommerce.Core/
│   ├── DTO/
│   ├── Entities/
│   ├── Mappers/
│   ├── RepositoryContracts/
│   ├── ServiceContracts/
│   ├── Services/
│   ├── Validators/
│   └── DependencyInjection.cs
│
├── eCommerce.Infrastructure/
│   ├── DbContext/
│   ├── Repositories/
│   └── DependencyInjection.cs
│
└── eCommerceSolution.UsersService.sln
```

The Core layer contains DTOs, entities, service and repository contracts, services, mappers, and validators, while Infrastructure contains the database context and repository implementation.

## API

### Register

```http
POST /api/Auth/register
Content-Type: application/json
```

Request:

```json
{
  "email": "user@example.com",
  "password": "password123",
  "personName": "John Doe",
  "gender": 0
}
```

The registration request validates the email format, password presence, person name, and gender enum.

### Login

```http
POST /api/Auth/login
Content-Type: application/json
```

Request:

```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

A successful request returns an authentication response containing the user's ID, email, name, gender, success status, and token field.

## Data Access

The service uses **Dapper** for lightweight SQL-based data access with **Npgsql** as the PostgreSQL provider.

User creation is performed through parameterized SQL, while login retrieves a user by email and password.

Example database operation:

```sql
INSERT INTO public."Users"
    ("UserID", "Email", "PersonName", "Gender", "Password")
VALUES
    (@UserID, @Email, @PersonName, @Gender, @Password);
```

## Validation

FluentValidation is integrated into the ASP.NET Core pipeline.

Registration validates:

* Required email
* Valid email format
* Required password
* Person name length of 1–50 characters
* Valid gender value

Login validates:

* Required email
* Valid email format
* Required password

## Error Handling

The API uses a custom global exception-handling middleware:

```text
Request
   │
   ▼
Controller
   │
   ▼
Service
   │
   ▼
Repository
   │
   └── Exception
          │
          ▼
ExceptionHandlingMiddleware
          │
          ▼
      HTTP Response
```

This keeps exception processing centralized instead of duplicating error-handling logic across controllers.

## Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* PostgreSQL 14+
* Docker (optional)

### 1. Clone

```bash
git clone https://github.com/otabek-kholmirzaev/eCommerceSolution.UsersService.git
cd eCommerceSolution.UsersService
```

### 2. Configure PostgreSQL

Configure the `PostgresConnection` connection string in your development configuration:

```json
{
  "ConnectionStrings": {
    "PostgresConnection": "Host=localhost;Port=5432;Database=eCommerce;Username=postgres;Password=your_password"
  }
}
```

The service creates its PostgreSQL connection through `NpgsqlConnection` using this configuration key.

### 3. Run the application

```bash
dotnet restore
dotnet build
dotnet run --project eCommerce.API
```

The API is configured as an ASP.NET Core application and exposes its HTTP endpoints through the `eCommerce.API` project.

## Docker

A multi-stage Dockerfile is included for the API.

Build the image:

```bash
docker build -f eCommerce.API/Dockerfile -t ecommerce-users-service .
```

Run the container:

```bash
docker run -p 8080:8080 ecommerce-users-service
```

The container exposes ports `8080` and `8081`.

## Design Principles

This project demonstrates several common backend architecture patterns:

* **Layered architecture** — API, Core, and Infrastructure are separated.
* **Dependency inversion** — Core defines repository and service contracts while Infrastructure provides implementations.
* **Repository pattern** — database operations are isolated behind `IUsersRepository`.
* **Service layer** — authentication logic is separated from HTTP controllers.
* **DTOs** — API contracts are separated from domain entities.
* **Centralized validation** — FluentValidation handles request validation.
* **Centralized exception handling** — errors are processed through middleware.

## Current Scope

This repository is intentionally focused on the **Users Service** rather than the entire eCommerce platform.

Currently implemented:

```text
Registration ──► PostgreSQL
Login        ──► PostgreSQL
Validation   ──► FluentValidation
Mapping      ──► AutoMapper
Data Access  ──► Dapper + Npgsql
Errors       ──► Exception Middleware
```

## Security Note

This project is currently a **learning/prototype implementation**, not a production authentication service.

The current implementation stores and queries passwords directly, and the returned token is currently a placeholder value rather than a generated access token. These areas should be replaced with secure password hashing and a real authentication/token flow before production use.

## Future Improvements

* Password hashing with ASP.NET Core Identity or BCrypt/Argon2
* JWT access and refresh tokens
* Refresh-token rotation and revocation
* User authorization and roles
* Duplicate-email validation
* Account/profile management endpoints
* Integration and unit tests
* Database migrations and automated initialization
* Health checks
* Structured logging
* CI/CD pipeline
* Kubernetes deployment
