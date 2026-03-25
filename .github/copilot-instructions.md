# Ecosystem Microservices — Copilot Instructions

## Project Overview

This is a **monorepo** containing a suite of microservices migrated from independent poly-repos. The migration consolidates services into a single repository while applying architectural improvements following **Domain-Driven Design (DDD)**, **Clean Architecture**, and **CQRS + Event-Driven** patterns.

### Origin (Poly-Repo)

The original services were standalone .NET projects with tightly coupled layers, direct database access, and inconsistent patterns across services. The migration preserves existing business logic while restructuring into a clean, modular architecture.

### Target (Mono-Repo)

Each microservice follows a uniform layered structure with shared infrastructure. Services communicate asynchronously via **RabbitMQ/MassTransit** and synchronously via **gRPC**. Deployment is managed through **Terraform** and **Kubernetes**.

---

## Architecture

### DDD Layered Architecture (per Microservice)

```
ecosystem-microservices/
├── Domain/                          # Shared Kernel (Ecosystem.Domain.Core)
│   ├── Bus/                         # IEventBus abstraction
│   ├── Commands/                    # Base Command class (CQRS)
│   ├── Events/                      # Base Event, Message classes
│   └── Configuration/               # Shared configs (CookieConfig, etc.)
│
├── Infra.Bus/                       # Shared: MassTransit → RabbitMQ bus
├── Infra.IoC/                       # Shared: Root DI container
├── Infrastructure/                  # Shared infra (Terraform, K8s manifests)
├── Tools/                           # Shared utilities, scripts
│
└── Microservices/
    ├── AccountService/              # ✅ Migrated
    ├── ConfigurationService/        # 🔲 Pending
    ├── GatewayService/              # 🔲 Pending (API Gateway)
    ├── InventoryService/            # 🔲 Pending
    ├── NotificationService/         # 🔲 Pending
    └── WalletService/               # 🔲 Pending
```

### Microservice Internal Structure (5 Layers)

Every microservice follows this exact structure:

```
Microservices/{ServiceName}/
├── Api/
│   └── Ecosystem.{ServiceName}.Api/           # Presentation Layer
│       ├── Controllers/                        # REST API endpoints
│       ├── Protos/                             # gRPC service definitions (.proto)
│       ├── GrpcServices/                       # gRPC service implementations
│       ├── Middlewares/                         # Custom middleware (auth, logging, error handling)
│       ├── Program.cs                          # App startup & pipeline config
│       └── appsettings.json
│
├── Application/
│   └── Ecosystem.{ServiceName}.Application/    # Application Layer (Use Cases)
│       ├── Commands/                           # CQRS Command definitions + Handlers
│       ├── Queries/                            # CQRS Query definitions + Handlers
│       ├── Events/                             # Integration event handlers (consumers)
│       ├── DTOs/                               # Data Transfer Objects
│       ├── Validators/                         # FluentValidation validators
│       ├── Mappings/                           # AutoMapper profiles
│       └── Services/                           # Application services (orchestration)
│
├── Domain/
│   └── Ecosystem.{ServiceName}.Domain/         # Domain Layer (Core Business)
│       ├── Models/                             # Entity models (EF Core entities)
│       │   └── CustomModels/                   # Query-only models (raw SQL projections)
│       ├── Interfaces/                         # Repository interfaces (contracts)
│       ├── Enums/                              # Domain enumerations
│       ├── Events/                             # Domain events (internal to bounded context)
│       └── ValueObjects/                       # Value objects (if applicable)
│
├── Data/
│   └── Ecosystem.{ServiceName}.Data/           # Infrastructure/Data Layer
│       ├── Context/                            # EF Core DbContext
│       ├── Repositories/                       # Repository implementations
│       ├── Migrations/                         # EF Core migrations
│       ├── Adapters/                           # External service adapters (HTTP clients)
│       └── Outbox/                             # Outbox pattern implementation
│
└── Infra.IoC/
    └── Ecosystem.{ServiceName}.Infra.IoC/      # Dependency Injection (per service)
        └── DependencyContainer.cs              # Service-specific DI registration
```

### Project Reference Graph

```
Api → Application → Domain ← Data
 │                              │
 └──→ Infra.IoC (service) ─────┘
         │
         └──→ Infra.IoC (shared) → Infra.Bus → Domain.Core
```

**Dependency Rule (strict):**
- Domain layer has ZERO external dependencies (only .NET BCL + Newtonsoft.Json for serialization attributes)
- Application depends on Domain only
- Data depends on Domain only (implements interfaces defined in Domain)
- Api depends on Application and Infra.IoC
- Dependencies always point inward (toward Domain)

---

## Technology Stack

| Layer | Technology | Version | Purpose |
|-------|-----------|---------|---------|
| **Runtime** | .NET | 8.0 | Target framework |
| **API** | ASP.NET Core | 8.0 | REST API hosting |
| **API** | Asp.Versioning.Mvc | 8.1.0 | API versioning |
| **API** | Swashbuckle | 6.6.2 | Swagger/OpenAPI docs |
| **API** | gRPC (Grpc.AspNetCore) | 2.62.0 | Service-to-service sync communication |
| **CQRS** | MediatR | 12.2.0 | Command/Query dispatching |
| **Validation** | FluentValidation | 12.1.1 | Input validation pipeline |
| **Mapping** | AutoMapper | 13.0.1 | Entity ↔ DTO mapping |
| **Messaging** | MassTransit | 8.1.3 | Message bus abstraction |
| **Messaging** | RabbitMQ | (via MassTransit) | Async messaging broker |
| **Database** | PostgreSQL | — | Primary data store |
| **ORM** | EF Core + Npgsql | 8.0.11 | Data access |
| **Auth** | JWT (System.IdentityModel.Tokens.Jwt) | 8.0.1 | Token-based authentication |
| **Auth** | BCrypt.Net-Next | 4.0.3 | Password hashing |
| **Auth** | Google.Apis.Auth | 1.68.0 | Google OAuth |
| **Auth** | OTP.NET | 1.4.0 | Two-factor authentication |
| **Serialization** | Newtonsoft.Json | 13.0.3 | JSON serialization (domain attributes) |
| **Infra** | Terraform | — | Infrastructure as Code |
| **Infra** | Kubernetes | — | Container orchestration |
| **Infra** | Docker | — | Containerization |

---

## Patterns & Practices

### 1. CQRS (Command Query Responsibility Segregation)

Use **MediatR** to separate reads from writes. Every use case is a Command or Query with its own Handler.

```
Application/
├── Commands/
│   ├── CreateUser/
│   │   ├── CreateUserCommand.cs        # IRequest<TResponse>
│   │   ├── CreateUserHandler.cs        # IRequestHandler<TCommand, TResponse>
│   │   └── CreateUserValidator.cs      # AbstractValidator<CreateUserCommand>
│   └── UpdateUser/
│       ├── UpdateUserCommand.cs
│       └── UpdateUserHandler.cs
└── Queries/
    └── GetUserById/
        ├── GetUserByIdQuery.cs         # IRequest<TResponse>
        └── GetUserByIdHandler.cs       # IRequestHandler<TQuery, TResponse>
```

**Rules:**
- Commands mutate state and may return a result DTO
- Queries are read-only and never modify state
- Handlers are the only place that orchestrates repository calls
- Controllers only dispatch commands/queries via `IMediator.Send()`
- Validators run automatically via MediatR pipeline behavior

### 2. MediatR Pipeline Behaviors

Register pipeline behaviors in this order:

```csharp
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
```

### 3. Repository Pattern

- **Interfaces** live in `Domain/Interfaces/` — these are contracts
- **Implementations** live in `Data/Repositories/` — these use EF Core
- Every repository inherits from `BaseRepository` which holds `AccountServiceDbContext`
- Use `IQueryable<T>` for composable queries, materialize with `ToListAsync()`
- Raw SQL via `FromSqlRaw()` is acceptable for stored procedures and complex queries

### 4. Outbox Pattern (Transactional Messaging)

Guarantees **at-least-once delivery** of integration events. When a domain operation occurs, the event is persisted in the same database transaction before being published.

```
Data/Outbox/
├── OutboxMessage.cs                # Entity: Id, Type, Payload, CreatedAt, ProcessedAt
├── IOutboxStore.cs                 # Interface in Domain/Interfaces/
└── OutboxProcessor.cs              # Background service that polls and publishes
```

**Flow:**
1. Command Handler executes business logic
2. Within the same `DbContext.SaveChangesAsync()` transaction, writes an `OutboxMessage`
3. A background `IHostedService` (OutboxProcessor) polls for unprocessed messages
4. Publishes each message to RabbitMQ via `IEventBus.Publish()`
5. Marks the message as processed (`ProcessedAt = DateTime.UtcNow`)

**OutboxMessage entity:**
```csharp
public class OutboxMessage
{
    public Guid Id { get; set; }
    public string EventType { get; set; }       // Full type name
    public string Payload { get; set; }          // JSON serialized event
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int RetryCount { get; set; }
    public string? Error { get; set; }
}
```

### 5. Inbox Pattern (Idempotent Consumers)

Prevents **duplicate processing** of the same message. Every consumer checks if the message was already handled before processing.

```csharp
public class InboxMessage
{
    public Guid MessageId { get; set; }          // Correlation ID from the event
    public string ConsumerType { get; set; }     // Full type name of the consumer
    public DateTime ProcessedAt { get; set; }
}
```

**Flow:**
1. Consumer receives a message from RabbitMQ
2. Checks `InboxMessage` table for existing `(MessageId, ConsumerType)` pair
3. If exists → skip (already processed)
4. If not → process the message and insert `InboxMessage` in the same transaction

### 6. Event-Driven Communication (MassTransit + RabbitMQ)

**Integration Events** flow between bounded contexts (microservices):

```
Service A (Publisher)                    Service B (Consumer)
┌─────────────────────┐                 ┌─────────────────────┐
│ Command Handler     │                 │ Event Consumer      │
│   ↓                 │                 │   (IConsumer<T>)    │
│ Domain Operation    │                 │   ↓                 │
│   ↓                 │   RabbitMQ      │ Command/Query via   │
│ Outbox Write ───────┼──→ Exchange ──→─┤ MediatR             │
│   ↓                 │                 │   ↓                 │
│ SaveChangesAsync()  │                 │ Inbox Check + Save  │
└─────────────────────┘                 └─────────────────────┘
```

**MassTransit configuration** is centralized in `Infra.IoC/DependencyContainer.cs`. Each microservice registers its consumers in its own `Infra.IoC`.

**Naming conventions:**
- Exchange: `Ecosystem.{ServiceName}.Events:{EventName}`
- Queue: `{consuming-service}-{event-name}`
- Events inherit from `Ecosystem.Domain.Core.Events.Event`

### 7. gRPC (Synchronous Service-to-Service)

Use gRPC for **real-time, low-latency** calls between services when async messaging is not suitable (e.g., fetching user data for validation).

```
Api/Protos/
└── account_service.proto           # Service & message definitions

Api/GrpcServices/
└── AccountGrpcService.cs           # Implementation using MediatR
```

**Rules:**
- gRPC services dispatch to MediatR (same as REST controllers)
- Proto files define the contract; implementations go in `GrpcServices/`
- Use `Grpc.AspNetCore` server-side and `Grpc.Net.Client` for client calls
- Register gRPC clients via `services.AddGrpcClient<T>()` in consuming service's IoC

### 8. Error Handling

- Use a global exception middleware in the Api layer
- Domain exceptions are specific (`EntityNotFoundException`, `BusinessRuleViolationException`)
- Application layer catches and maps to appropriate HTTP status codes
- Never expose stack traces or internal details in API responses

### 9. Soft Deletes

Most entities use soft deletes via a `DeletedAt` nullable column. EF Core global query filters exclude soft-deleted records automatically:

```csharp
modelBuilder.Entity<User>().HasQueryFilter(e => e.DeletedAt == null);
```

To include deleted records, use `.IgnoreQueryFilters()`.

---

## Database Conventions

- **Provider:** PostgreSQL via Npgsql
- **Schema:** Each microservice owns its schema (e.g., `account_service`, `configuration_service`)
- **Table naming:** `snake_case` (e.g., `users`, `users_affiliate`, `ticket_messages`)
- **Column naming:** `snake_case` (e.g., `created_at`, `deleted_at`, `brand_id`)
- **Primary keys:** `id` (integer, auto-increment) or composite keys where appropriate
- **Timestamps:** `created_at`, `updated_at`, `deleted_at` (nullable for soft deletes)
- **Entity configuration:** Fluent API in `OnModelCreating()` — no data annotations for table/column mapping
- **Multi-tenant:** `brand_id` column distinguishes tenants (Ecosystem=1, RecyCoin=2, HouseCoin=3, ExitoJuntos=4)

---

## Naming Conventions

### Namespaces

```
Ecosystem.{ServiceName}.{Layer}[.SubFolder]

Examples:
  Ecosystem.AccountService.Domain.Models
  Ecosystem.AccountService.Domain.Models.CustomModels
  Ecosystem.AccountService.Domain.Interfaces
  Ecosystem.AccountService.Domain.Enums
  Ecosystem.AccountService.Data.Context
  Ecosystem.AccountService.Data.Repositories
  Ecosystem.AccountService.Application.Commands.CreateUser
  Ecosystem.AccountService.Application.Queries.GetUserById
  Ecosystem.AccountService.Application.DTOs
  Ecosystem.AccountService.Api.Controllers
```

### Files

| Type | Convention | Example |
|------|-----------|---------|
| Entity model | `{EntityName}.cs` | `User.cs`, `UsersAffiliate.cs` |
| Custom model (query) | `{ModelName}.cs` in CustomModels/ | `BinaryFamilyTree.cs` |
| Repository interface | `I{Entity}Repository.cs` | `IUserRepository.cs` |
| Repository impl | `{Entity}Repository.cs` | `UserRepository.cs` |
| Command | `{Action}{Entity}Command.cs` | `CreateUserCommand.cs` |
| Command handler | `{Action}{Entity}Handler.cs` | `CreateUserHandler.cs` |
| Query | `Get{Entity}By{Criteria}Query.cs` | `GetUserByIdQuery.cs` |
| DTO | `{Entity}Dto.cs` | `UserDto.cs` |
| Validator | `{Command}Validator.cs` | `CreateUserCommandValidator.cs` |
| Event | `{Entity}{Action}Event.cs` | `UserCreatedEvent.cs` |
| Consumer | `{EventName}Consumer.cs` | `UserCreatedEventConsumer.cs` |
| gRPC service | `{Service}GrpcService.cs` | `AccountGrpcService.cs` |
| Controller | `{Entity}Controller.cs` | `UserController.cs` |

### Projects (csproj)

```
Ecosystem.{ServiceName}.{Layer}.csproj

Examples:
  Ecosystem.AccountService.Api.csproj
  Ecosystem.AccountService.Application.csproj
  Ecosystem.AccountService.Domain.csproj
  Ecosystem.AccountService.Data.csproj
  Ecosystem.AccountService.Infra.IoC.csproj
```

---

## Infrastructure & Deployment

### Terraform

Infrastructure as Code for provisioning cloud resources:

```
Infrastructure/
├── terraform/
│   ├── environments/
│   │   ├── dev/
│   │   ├── staging/
│   │   └── production/
│   ├── modules/
│   │   ├── kubernetes/          # EKS/AKS/GKE cluster
│   │   ├── database/            # PostgreSQL instances
│   │   ├── messaging/           # RabbitMQ (CloudAMQP or self-hosted)
│   │   ├── networking/          # VPC, subnets, security groups
│   │   └── monitoring/          # Observability stack
│   ├── main.tf
│   ├── variables.tf
│   └── outputs.tf
```

### Kubernetes

Each microservice is deployed as an independent pod with its own:

```
Infrastructure/
├── k8s/
│   ├── base/
│   │   ├── namespace.yaml
│   │   └── configmap.yaml
│   ├── services/
│   │   ├── account-service/
│   │   │   ├── deployment.yaml
│   │   │   ├── service.yaml
│   │   │   ├── hpa.yaml          # Horizontal Pod Autoscaler
│   │   │   └── ingress.yaml
│   │   ├── configuration-service/
│   │   ├── wallet-service/
│   │   └── ...
│   └── infrastructure/
│       ├── rabbitmq/
│       ├── postgresql/
│       └── monitoring/
```

**Deployment principles:**
- One Deployment per microservice
- Health checks: `/health` endpoint (liveness + readiness)
- ConfigMaps and Secrets for environment-specific configuration
- HPA based on CPU/memory metrics
- Rolling updates with zero downtime
- Each service has its own PostgreSQL database/schema (database-per-service)

### Docker

Each microservice has its own `Dockerfile`:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Multi-stage build: restore → build → publish → runtime
```

---

## Code Generation Rules

When generating code for this project, follow these rules:

### General

- Use C# 12 / .NET 8.0 features (file-scoped namespaces, primary constructors where appropriate)
- Use `nullable` reference types (project-wide `<Nullable>enable</Nullable>`)
- Use `async/await` consistently — never block with `.Result` or `.Wait()`
- All public methods in repositories and handlers must be `async Task<T>`
- Use `CancellationToken` in all async method signatures
- Prefer constructor injection over service locator

### Controllers

```csharp
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => Ok(await _mediator.Send(new GetUserByIdQuery(id), ct));
}
```

- Controllers are thin — only dispatch to MediatR
- No business logic in controllers
- Use `[ApiVersion]` for versioning
- Return `IActionResult` with appropriate status codes

### Commands & Queries

```csharp
// Command
public record CreateUserCommand(string UserName, string Email) : IRequest<UserDto>;

// Handler
public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;

    public CreateUserHandler(IUserRepository userRepo, IMapper mapper)
    {
        _userRepo = userRepo;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken ct)
    {
        // Business logic here
    }
}
```

### Repositories

- Inherit from `BaseRepository` (which holds the DbContext)
- Use constructor injection for the DbContext
- Use `async` methods with `ToListAsync()`, `FirstOrDefaultAsync()`, etc.
- Raw SQL is acceptable for stored procedures and complex queries using `FromSqlRaw()`
- Never expose `IQueryable` outside the repository — always materialize

### Events

```csharp
// Integration event (crosses bounded context boundaries)
public class UserCreatedEvent : Event
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int BrandId { get; set; }
}

// Consumer in another service
public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
{
    private readonly IMediator _mediator;

    public UserCreatedEventConsumer(IMediator mediator) => _mediator = mediator;

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        // Inbox check → process → save inbox record
        await _mediator.Send(new ProcessUserCreatedCommand(context.Message));
    }
}
```

### Validation

```csharp
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
```

- One validator per command
- Validators are auto-registered and run via MediatR pipeline behavior
- Validation errors return 400 Bad Request with structured error details

---

## Migration Notes (Poly-Repo → Mono-Repo)

### What Changed

| Aspect | Before (Poly-Repo) | After (Mono-Repo) |
|--------|--------------------|--------------------|
| **Structure** | `AccountService.Data`, `AccountService.Models` | 5-layer DDD per service |
| **Namespaces** | `AccountService.Data.Database.Models` | `Ecosystem.AccountService.Domain.Models` |
| **Interfaces** | `AccountService.Data.Repositories.IRepositories` | `Ecosystem.AccountService.Domain.Interfaces` |
| **DbContext** | `AccountService.Data.Database` | `Ecosystem.AccountService.Data.Context` |
| **Enums** | `AccountService.Models.Enums` | `Ecosystem.AccountService.Domain.Enums` |
| **Messaging** | Direct HTTP calls | MassTransit + RabbitMQ |
| **Service comm** | REST only | gRPC (sync) + RabbitMQ (async) |
| **Reliability** | No guarantees | Outbox + Inbox patterns |
| **Infrastructure** | Manual deploy | Terraform + Kubernetes |

### What Was Preserved

- All entity models with their exact database mappings (Fluent API)
- All repository implementations with their query logic
- PostgreSQL schema `account_service` with `snake_case` conventions
- Soft delete behavior via `DeletedAt` query filters
- Multi-tenant support via `BrandId`
- Stored procedure calls and raw SQL queries

### Services to Migrate

- [x] **AccountService** — User management, affiliates, tickets, auth, matrix, leaderboards
- [ ] **ConfigurationService** — System configuration, feature flags
- [ ] **InventoryService** — Product/inventory management
- [ ] **WalletService** — Wallets, transactions, balances
- [ ] **GatewayService** — API Gateway, routing, rate limiting
- [ ] **NotificationService** — Email, SMS, push notifications

---

## Quick Reference: Adding a New Feature

1. **Define the entity** in `Domain/Models/`
2. **Define the repository interface** in `Domain/Interfaces/`
3. **Implement the repository** in `Data/Repositories/`
4. **Add DbSet** and Fluent API config to `Data/Context/{Service}DbContext.cs`
5. **Create Command/Query** in `Application/Commands/` or `Application/Queries/`
6. **Create Handler** implementing `IRequestHandler<TRequest, TResponse>`
7. **Create Validator** with FluentValidation
8. **Create DTO** in `Application/DTOs/`
9. **Add AutoMapper profile** in `Application/Mappings/`
10. **Add Controller endpoint** in `Api/Controllers/`
11. **Register dependencies** in `Infra.IoC/DependencyContainer.cs`
12. **Add EF migration** via `dotnet ef migrations add {Name}`

## Quick Reference: Adding a New Microservice

1. Create folder structure under `Microservices/{ServiceName}/` with all 5 layers
2. Create `.csproj` files following naming convention `Ecosystem.{ServiceName}.{Layer}`
3. Add project references (Api → Application → Domain ← Data, Api → Infra.IoC)
4. Add projects to `EcosystemMicroservices.slnx`
5. Create `DbContext` in `Data/Context/` with schema name
6. Register in shared `Infra.IoC` and service-specific `Infra.IoC`
7. Add Dockerfile and K8s manifests
8. Add Terraform resources if needed
