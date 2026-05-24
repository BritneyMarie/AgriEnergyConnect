---
layout: default
title: Home
---

# AgriEnergyConnect Documentation

**Student:** ST10494165 | **Module:** PROG7311 - Programming 3A | **POE Part 3**

AgriEnergyConnect is a web application connecting farmers with renewable energy solutions through an intuitive digital platform. Built with ASP.NET Core 9.0 MVC, it features role-based authentication, product catalog management, and a farmer directory.

---

## Table of Contents

- [Getting Started](#getting-started)
- [Architecture](architecture)
- [Database Schema](database)
- [Controllers & Repositories](api-reference)
- [UI/UX Design](ui-design)
- [Deployment Guide](deployment)
- [Design Decisions](design-decisions)

---

## Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running

### Quick Start

```bash
# Clone the repository
git clone https://github.com/YOUR_USERNAME/AgriEnergyConnect.git
cd AgriEnergyConnect

# Build and run
docker compose up --build -d

# Open in browser
# http://localhost:8080
```

### Default Accounts

| Role     | Email                    | Password     |
|----------|--------------------------|--------------|
| Employee | employee@agrienergy.com  | Employee123! |
| Farmer   | farmer@agrienergy.com    | Farmer123!   |

**Employee** users can view all products, manage the farmer directory, and register new farmers.

**Farmer** users can view and add their own products to the catalog.

### Stopping the Application

```bash
# Stop containers (preserves data)
docker compose down

# Stop and wipe database
docker compose down -v
```
