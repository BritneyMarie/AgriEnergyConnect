---
layout: default
title: Design Decisions
---

# Design Decisions

[Back to Home](./)

---

This page documents the key architectural and design choices made during development, explaining the rationale behind each decision.

## 1. Repository Pattern over Direct DbContext Access

**Decision:** Introduce `IFarmerRepository` and `IProductRepository` interfaces with concrete implementations rather than using `ApplicationDbContext` directly in controllers.

**Rationale:**
- Adheres to the Single Responsibility Principle - controllers handle HTTP concerns, repositories handle data access
- Centralizes query optimizations (e.g., `AsNoTracking()`, `Include()`, ordering) in one place
- Makes controllers testable by allowing repository mocking
- Aligns with POE Part 3 requirements for clean architecture

**Trade-off:** Adds an abstraction layer that increases file count. Acceptable given the application's scope and academic requirements.

## 2. Docker Containerization over Local SQL Server

**Decision:** Use Docker Compose with SQL Server 2022 container instead of requiring a local SQL Server installation or Azure SQL.

**Rationale:**
- Zero-dependency setup: only Docker Desktop needed
- Reproducible environment across different machines
- Health checks ensure proper startup ordering
- Easy database reset with `docker compose down -v`
- Demonstrates containerization skills per POE Part 3 requirements

**Trade-off:** Requires Docker Desktop installation (~4 GB disk). SQL Server container needs at least 2 GB RAM.

## 3. Code-First with EnsureCreated over Migrations

**Decision:** Use `context.Database.EnsureCreated()` in `DbInitializer` rather than EF Core migrations.

**Rationale:**
- Simpler for a demonstration/academic project
- Database is recreated from scratch in Docker on each fresh deployment
- No migration history to manage
- Seed data is applied immediately after schema creation

**Trade-off:** Not suitable for production where schema evolution must preserve existing data. Migrations would be the correct choice for a real-world deployment.

## 4. ASP.NET Identity with Default UI

**Decision:** Use `AddDefaultUI()` for Identity pages (Login, Register, Logout) rather than scaffolding custom pages.

**Rationale:**
- Provides battle-tested authentication UI out of the box
- Reduces development time and security risk
- Login/Register/Logout flows work correctly without custom implementation
- Custom `_LoginPartial.cshtml` provides nav integration with the earthy theme

**Trade-off:** Login and Register pages use the default Identity styling rather than the custom earthy theme. The main application pages (Home, Products, Farmers) fully match the custom design.

## 5. Role-Based Authorization over Policy-Based

**Decision:** Use simple `[Authorize(Roles = "Farmer")]` and `[Authorize(Roles = "Employee")]` attributes rather than custom authorization policies.

**Rationale:**
- Two roles with clear, non-overlapping permissions
- Role checks are straightforward and declarative
- No complex authorization logic that would benefit from policies
- Easy to understand and audit

**Trade-off:** Less flexible than policy-based authorization for complex requirements. Sufficient for this application's access control needs.

## 6. Database Indexing Strategy

**Decision:** Add indexes on `Farmer.Email`, `Farmer.UserId`, `Farmer.FarmName`, `Product.Category`, `Product.ProductionDate`, and a composite index on `Product(FarmerId, Category)`.

**Rationale:**
- `Farmer.Email` - Used for search and lookup
- `Farmer.UserId` - Foreign key lookup on every farmer-related request
- `Farmer.FarmName` - Search filter in the farmer directory
- `Product.Category` - Category filter on product listing
- `Product.ProductionDate` - Date range filter on product listing
- `(FarmerId, Category)` - Composite for farmer-specific category queries

**Trade-off:** Indexes add write overhead and storage. The chosen columns are all used in WHERE/JOIN clauses, making the read performance gain worthwhile.

## 7. Pagination with Tuple Return Type

**Decision:** Repository pagination methods return `(IEnumerable<T> Items, int TotalCount)` tuples.

**Rationale:**
- Returns both the page data and total count in a single database round-trip concept
- Enables the view to calculate total pages without a separate count query
- Clean API without introducing a dedicated `PagedResult<T>` class
- ViewBag passes `CurrentPage` and `TotalPages` to a shared `_Pagination.cshtml` partial

**Trade-off:** Tuples are less self-documenting than named types. A `PagedResult<T>` class would be more appropriate in a larger application.

## 8. Earthy UI Theme

**Decision:** Custom CSS theme with agricultural color palette (olive green, brown, wheat yellow) rather than default Bootstrap styling.

**Rationale:**
- Reflects the agricultural domain of the application
- Creates a professional, cohesive visual identity
- Card-based layouts with yellow-green borders provide visual structure
- Icon prefixes on labels improve scannability
- Privacy notices on forms demonstrate data protection awareness
- Matches the POE Part 2 UI/UX design specifications

## 9. Seeded Demo Data

**Decision:** Automatically seed two users (employee + farmer), one farmer profile, and two sample products on first startup.

**Rationale:**
- Enables immediate testing without manual data entry
- Demonstrates both roles and their capabilities
- Idempotent: checks `context.Users.Any()` before seeding
- Provides meaningful sample data (farm name, product categories, prices)
