---
layout: default
title: Database Schema
---

# Database Schema

[Back to Home](./)

---

## Entity Relationship Diagram

```
AspNetUsers (Identity)
    |
    | 1:1
    v
Farmer
    |
    | 1:Many
    v
Product
```

## Entities

### ApplicationUser

Extends ASP.NET Identity's `IdentityUser` with application-specific fields.

| Column          | Type     | Constraints        |
|----------------|----------|--------------------|
| Id             | string   | PK (Identity)      |
| FirstName      | string   |                    |
| LastName       | string   |                    |
| IsFarmer       | bool     |                    |
| IsEmployee     | bool     |                    |
| UserName       | string   | Unique (Identity)  |
| Email          | string   | (Identity)         |
| PasswordHash   | string   | (Identity)         |
| *...other Identity columns* | | |

### Farmer

Represents a registered farmer profile linked to an Identity user.

| Column        | Type        | Constraints                    |
|--------------|-------------|--------------------------------|
| FarmerId     | int         | PK, Auto-increment             |
| FarmName     | string(100) | Required                       |
| ContactPerson| string(100) | Required                       |
| PhoneNumber  | string(20)  | Required                       |
| Address      | string(200) | Required                       |
| Email        | string(100) | Required, Indexed              |
| UserId       | string      | FK -> AspNetUsers.Id, Indexed  |

### Product

Represents an agricultural product listed by a farmer.

| Column         | Type        | Constraints                      |
|---------------|-------------|----------------------------------|
| ProductId     | int         | PK, Auto-increment               |
| Name          | string(100) | Required                         |
| Category      | string(50)  | Required, Indexed                |
| ProductionDate| DateTime    | Required, Indexed                |
| Description   | string      | Optional                         |
| Price         | decimal     | Required, Range(0.01 - 10000)    |
| Quantity      | int         | Required, Range(1+)              |
| FarmerId      | int         | FK -> Farmer.FarmerId, Indexed   |

## Relationships

| Relationship          | Type    | Cascade Delete |
|----------------------|---------|----------------|
| User -> Farmer       | 1:1     | Yes            |
| Farmer -> Products   | 1:Many  | Yes            |

## Indexes

Indexes are configured in `ApplicationDbContext.OnModelCreating()` for query performance:

| Table   | Column(s)              | Purpose                              |
|---------|------------------------|--------------------------------------|
| Farmer  | Email                  | Lookup farmers by email              |
| Farmer  | UserId                 | Link Identity user to farmer profile |
| Farmer  | FarmName               | Search farmers by farm name          |
| Product | Category               | Filter products by category          |
| Product | ProductionDate         | Filter products by date range        |
| Product | (FarmerId, Category)   | Composite: farmer's products by type |

## Database Seeding

On first startup, `DbInitializer.Initialize()` seeds:

1. **Roles** - "Farmer" and "Employee"
2. **Employee user** - employee@agrienergy.com with Employee role
3. **Farmer user** - farmer@agrienergy.com with Farmer role
4. **Farmer profile** - Green Valley Farms linked to the farmer user
5. **Sample products** - Organic Wheat (Grains) and Free-range Eggs (Dairy)

Seeding is idempotent: it checks `context.Users.Any()` before inserting.

## Connection String

Configured via environment variable in Docker Compose:

```
Server=db;Database=AgriEnergyConnect;User Id=sa;Password=YourStr0ng!Pass;TrustServerCertificate=True;
```

The `db` hostname resolves to the SQL Server container within the Docker network.
