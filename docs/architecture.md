---
layout: default
title: Architecture
---

# System Architecture

[Back to Home](./)

---

## Overview

AgriEnergyConnect follows a layered architecture built on ASP.NET Core 9.0 MVC:

```
Browser (User)
    |
    v
[ASP.NET Core MVC]
    |
    ├── Controllers (Request handling, authorization)
    |       |
    |       v
    ├── Repositories (Data access abstraction)
    |       |
    |       v
    ├── Entity Framework Core (ORM)
    |       |
    |       v
    └── SQL Server 2022 (Docker container)
```

## Design Patterns

### Repository Pattern

All data access is abstracted behind repository interfaces, separating business logic from database operations.

```
IFarmerRepository  <──  FarmerRepository
IProductRepository <──  ProductRepository
```

**Benefits:**
- Controllers depend only on interfaces, not concrete implementations
- Data access logic is centralized and reusable
- Enables unit testing with mock repositories
- Query optimizations (e.g., `AsNoTracking()`) are applied consistently

### Dependency Injection

Services are registered in `Program.cs` and injected into controllers:

```csharp
builder.Services.AddScoped<IFarmerRepository, FarmerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
```

The `Scoped` lifetime ensures one repository instance per HTTP request.

### MVC Pattern

- **Models** - Domain entities (`Farmer`, `Product`, `ApplicationUser`) with Data Annotation validation
- **Views** - Razor views with a shared layout, partial views for pagination and login
- **Controllers** - Handle HTTP requests, enforce authorization, coordinate between repositories

## Authentication & Authorization

### ASP.NET Identity

The application uses ASP.NET Identity with custom `ApplicationUser` extending `IdentityUser`:

```csharp
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsFarmer { get; set; }
    public bool IsEmployee { get; set; }
}
```

### Role-Based Access Control

Two roles govern access:

| Role     | Access                                              |
|----------|-----------------------------------------------------|
| Farmer   | View own products, add new products                 |
| Employee | View all products, manage farmer directory          |

Authorization is enforced at the controller level:

```csharp
[Authorize(Roles = Roles.Employee)]    // FarmersController
[Authorize(Roles = Roles.Farmer)]      // ProductsController.Create
[Authorize]                            // ProductsController (all actions)
```

## Containerization

### Docker Architecture

```
docker-compose.yml
    |
    ├── db (SQL Server 2022)
    |   └── Port 1433, health-checked
    |
    └── web (ASP.NET Core app)
        └── Port 8080, depends on db health
```

### Multi-Stage Build

The Dockerfile uses a two-stage build to minimize image size:

1. **Build stage** (`sdk:9.0`) - Restores packages, compiles, and publishes
2. **Runtime stage** (`aspnet:9.0`) - Contains only the published output

### Health Checks

The SQL Server container includes a health check that verifies database connectivity before the web container starts:

```yaml
healthcheck:
  test: /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "..." -C -Q "SELECT 1"
  interval: 10s
  retries: 10
  start_period: 30s
```

## Data Flow

### Product Creation (Farmer Role)

1. Farmer navigates to Products > Create
2. `ProductsController.Create` [GET] renders the form
3. Farmer submits the form
4. `ProductsController.Create` [POST] validates input
5. Controller resolves the farmer profile via `UserManager` + `IFarmerRepository`
6. Product is saved via `IProductRepository.AddAsync()`
7. Redirect to Products index

### Farmer Registration (Employee Role)

1. Employee navigates to Farmers > Register New Farmer
2. `FarmersController.Create` [GET] renders the form
3. Employee submits farmer details
4. `FarmersController.Create` [POST] validates and saves via `IFarmerRepository.AddAsync()`
5. Redirect to Farmers index
