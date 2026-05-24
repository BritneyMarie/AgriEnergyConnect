---
layout: default
title: Controllers & Repositories
---

# Controllers & Repositories API Reference

[Back to Home](./)

---

## Controllers

### HomeController

Basic navigation controller, no authorization required for unauthenticated pages.

| Action   | HTTP | Route          | Auth     | Description         |
|----------|------|----------------|----------|---------------------|
| Index    | GET  | /              | None     | Landing page        |
| Privacy  | GET  | /Home/Privacy  | None     | Privacy policy page |

### ProductsController

Manages the agricultural product catalog. Requires authentication for all actions.

| Action   | HTTP | Route              | Auth   | Description                       |
|----------|------|--------------------|--------|-----------------------------------|
| Index    | GET  | /Products          | Any    | List products with filters        |
| Create   | GET  | /Products/Create   | Farmer | Show add product form             |
| Create   | POST | /Products/Create   | Farmer | Submit new product                |
| Details  | GET  | /Products/Details/5| Any    | View product details              |

**Index Parameters:**

| Parameter      | Type      | Description                          |
|---------------|-----------|--------------------------------------|
| categoryFilter | string?  | Filter by product category           |
| startDate      | DateTime?| Filter: production date from         |
| endDate        | DateTime?| Filter: production date to           |
| page           | int      | Page number (default: 1)             |

**Behavior:**
- Farmers see only their own products (filtered by `FarmerId`)
- Employees see all products across all farmers
- Page size: 10 items per page

### FarmersController

Manages the farmer directory. Restricted to Employee role only.

| Action   | HTTP | Route              | Auth     | Description                    |
|----------|------|--------------------|----------|--------------------------------|
| Index    | GET  | /Farmers           | Employee | List farmers with search       |
| Create   | GET  | /Farmers/Create    | Employee | Show registration form         |
| Create   | POST | /Farmers/Create    | Employee | Register new farmer            |
| Details  | GET  | /Farmers/Details/5 | Employee | View farmer profile + products |

**Index Parameters:**

| Parameter | Type    | Description                                    |
|----------|---------|------------------------------------------------|
| search   | string? | Search by farm name, contact person, or email  |
| page     | int     | Page number (default: 1)                       |

---

## Repository Interfaces

### IFarmerRepository

```csharp
public interface IFarmerRepository
{
    Task<IEnumerable<Farmer>> GetAllAsync();
    Task<Farmer?> GetByIdAsync(int id);
    Task<Farmer?> GetByIdWithProductsAsync(int id);
    Task<Farmer?> GetByUserIdAsync(string userId);
    Task<(IEnumerable<Farmer> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, string? search = null);
    Task AddAsync(Farmer farmer);
    Task UpdateAsync(Farmer farmer);
    Task DeleteAsync(int id);
}
```

| Method                  | Description                                           |
|------------------------|-------------------------------------------------------|
| GetAllAsync            | Returns all farmers                                   |
| GetByIdAsync           | Find farmer by primary key                            |
| GetByIdWithProductsAsync| Find farmer with eager-loaded products               |
| GetByUserIdAsync       | Find farmer by Identity user ID                       |
| GetPagedAsync          | Paginated list with optional search filter            |
| AddAsync               | Insert new farmer                                     |
| UpdateAsync            | Update existing farmer                                |
| DeleteAsync            | Remove farmer by ID                                   |

### IProductRepository

```csharp
public interface IProductRepository
{
    Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, int? farmerId = null,
        string? category = null, DateTime? startDate = null,
        DateTime? endDate = null);
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<string>> GetCategoriesAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}
```

| Method           | Description                                              |
|-----------------|----------------------------------------------------------|
| GetPagedAsync   | Paginated products with farmerId, category, date filters |
| GetByIdAsync    | Find product by primary key (includes Farmer navigation) |
| GetCategoriesAsync| Get distinct category list for filter dropdowns        |
| AddAsync        | Insert new product                                       |
| UpdateAsync     | Update existing product                                  |
| DeleteAsync     | Remove product by ID                                     |

## Implementation Details

### Query Optimizations

- All read-only queries use `AsNoTracking()` to avoid change-tracking overhead
- Products are ordered by `ProductionDate` descending (newest first)
- Pagination uses `Skip()` / `Take()` with a total count for page calculation
- The `GetByIdAsync` for products includes the `Farmer` navigation property via `Include()`

### Anti-Forgery Protection

All POST actions are decorated with `[ValidateAntiForgeryToken]` and use `[Bind()]` to whitelist accepted form fields, preventing over-posting attacks.
