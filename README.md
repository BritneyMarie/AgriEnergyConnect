# AgriEnergyConnect

**Student:** ST10494165  
**Module:** PROG7311 - Programming 3A  
**Portfolio of Evidence:** Part 3  

A web application connecting farmers with renewable energy solutions through an intuitive digital platform. Built with ASP.NET Core 9.0 MVC, Entity Framework Core, and SQL Server, containerized with Docker.

## Features

- **Role-Based Authentication** - Separate Farmer and Employee roles with ASP.NET Identity
- **Product Catalog Management** - Farmers can add and manage agricultural products
- **Farmer Directory** - Employees can register, search, and view farmer profiles
- **Advanced Filtering** - Search by category, date range, and keywords
- **Pagination** - Scalable list views with configurable page sizes
- **Database Indexing** - Optimized queries on frequently searched columns
- **Repository Pattern** - Clean separation of data access from business logic
- **Docker Deployment** - Multi-stage build with SQL Server containerization
- **Responsive Design** - Mobile-friendly earthy/agricultural theme

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (Windows/Mac/Linux)
- No other dependencies required - everything runs in containers

## Quick Start

1. **Clone the repository**
   ```bash
   git clone https://github.com/BritneyMarie/AgriEnergyConnect.git
   cd AgriEnergyConnect
   ```

2. **Start the application**
   ```bash
   docker compose up --build -d
   ```

3. **Open in browser**
   ```
   http://localhost:8080
   ```

4. **Log in with seeded accounts**

   | Role     | Email                      | Password      |
   |----------|---------------------------|---------------|
   | Employee | employee@agrienergy.com   | Employee123!  |
   | Farmer   | farmer@agrienergy.com     | Farmer123!    |

5. **Stop the application**
   ```bash
   docker compose down
   ```
   To also wipe the database:
   ```bash
   docker compose down -v
   ```

## Project Structure

```
AgriEnergyConnect/
├── AgriEnergyConnect/          # ASP.NET Core MVC project
│   ├── Authorization/          # Role constants
│   ├── Controllers/            # MVC controllers
│   │   ├── HomeController.cs
│   │   ├── FarmerController.cs
│   │   └── ProductsController.cs
│   ├── Models/                 # Domain models + EF context
│   │   ├── ApplicationUser.cs
│   │   ├── ApplicationDBContext.cs
│   │   ├── DbInitializer.cs
│   │   ├── Famer.cs            # Farmer entity
│   │   └── Product.cs
│   ├── Repositories/           # Data access layer
│   │   ├── IFarmerRepository.cs
│   │   ├── FarmerRepository.cs
│   │   ├── IProductRepository.cs
│   │   └── ProductRepository.cs
│   ├── Views/                  # Razor views
│   │   ├── Home/
│   │   ├── Farmers/
│   │   ├── Products/
│   │   └── Shared/
│   ├── wwwroot/css/site.css    # Custom theme
│   └── Program.cs             # App entry point + DI config
├── Dockerfile                  # Multi-stage Docker build
├── docker-compose.yml          # Docker Compose orchestration
└── docs/                       # GitHub Pages documentation
```

## Architecture

### Design Patterns

- **Repository Pattern** - `IFarmerRepository` and `IProductRepository` abstract EF Core operations behind interfaces, enabling testability and clean controller code
- **Dependency Injection** - All services registered in `Program.cs` via ASP.NET Core's built-in DI container
- **MVC Pattern** - Clear separation of Models, Views, and Controllers

### Database

- **SQL Server 2022** running in Docker
- **Entity Framework Core 9.0** with Code-First approach
- **ASP.NET Identity** for authentication and role management
- **Database seeding** via `DbInitializer` creates roles, users, and sample data on first run

### Performance Optimizations

Database indexes on frequently queried columns:
- `Farmer.Email`, `Farmer.UserId`, `Farmer.FarmName`
- `Product.Category`, `Product.ProductionDate`
- Composite index on `Product(FarmerId, Category)`

All read queries use `AsNoTracking()` for reduced memory overhead.

### Security

- Role-based authorization (`[Authorize(Roles = "Employee")]`, `[Authorize(Roles = "Farmer")]`)
- Anti-forgery token validation on all POST actions
- Input validation with Data Annotations
- Parameterized queries via Entity Framework (SQL injection prevention)

## Role Permissions

| Feature              | Farmer | Employee |
|---------------------|--------|----------|
| View Products       | Own only | All     |
| Add Products        | Yes    | No       |
| View Farmers        | No     | Yes      |
| Register Farmers    | No     | Yes      |
| View Farmer Details | No     | Yes      |

## Technology Stack

| Component        | Technology                    |
|-----------------|-------------------------------|
| Framework       | ASP.NET Core 9.0 MVC         |
| Language        | C# 13                        |
| ORM             | Entity Framework Core 9.0    |
| Database        | SQL Server 2022              |
| Authentication  | ASP.NET Identity             |
| Frontend        | Razor Views + Bootstrap 5    |
| Containerization| Docker + Docker Compose      |

## Documentation

Full project documentation is available on [GitHub Pages](https://BritneyMarie.github.io/AgriEnergyConnect/), including:

- System architecture and design decisions
- Database schema and entity relationships
- Controller and repository API reference
- Deployment guide
- UI/UX design rationale

## License

This project was developed for academic purposes as part of the PROG7311 module.
