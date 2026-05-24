---
layout: default
title: Deployment Guide
---

# Deployment Guide

[Back to Home](./)

---

## Docker Deployment (Recommended)

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running
- At least 4 GB RAM allocated to Docker (SQL Server requirement)

### Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/BritneyMarie/AgriEnergyConnect.git
   cd AgriEnergyConnect
   ```

2. **Build and start containers**
   ```bash
   docker compose up --build -d
   ```
   This creates two containers:
   - `db` - SQL Server 2022 on port 1433
   - `web` - ASP.NET Core app on port 8080

3. **Wait for startup**
   The web container waits for the database health check to pass before starting. First launch may take 30-60 seconds while SQL Server initializes.

4. **Access the application**
   ```
   http://localhost:8080
   ```

5. **Verify database seeding**
   Check container logs for seeding confirmation:
   ```bash
   docker compose logs web
   ```
   Look for EF Core INSERT statements (Farmers, Products tables).

### Managing the Application

```bash
# View logs
docker compose logs -f web

# Restart the web container
docker compose restart web

# Stop everything
docker compose down

# Stop and delete database volume
docker compose down -v

# Rebuild after code changes
docker compose up --build -d
```

### Troubleshooting

**Login fails with "Invalid login attempt"**
- This usually means stale browser cookies after a database reset
- Clear your browser cookies for `localhost:8080` and try again

**Antiforgery token error**
- Same cause as above - old cookies contain tokens encrypted with a previous data protection key
- Clear cookies or use an incognito window

**SQL Server won't start**
- Ensure Docker has at least 4 GB RAM allocated
- Check: `docker compose logs db`

**Port already in use**
- Stop other services on port 8080 or 1433
- Or modify `docker-compose.yml` ports mapping

---

## Local Development (Without Docker)

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server instance (local or remote)

### Steps

1. **Update the connection string** in `AgriEnergyConnect/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=AgriEnergyConnect;Trusted_Connection=true;TrustServerCertificate=True;"
     }
   }
   ```

2. **Restore packages and run**
   ```bash
   cd AgriEnergyConnect
   dotnet restore
   dotnet run --project AgriEnergyConnect
   ```

3. **Access at** `https://localhost:5001` or `http://localhost:5000`

---

## Docker Architecture Details

### Dockerfile (Multi-Stage Build)

```dockerfile
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY AgriEnergyConnect/AgriEnergyConnect.csproj ./
RUN dotnet restore
COPY AgriEnergyConnect/ ./
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "AgriEnergyConnect.dll"]
```

**Build stage** uses the full SDK image (~900 MB) for compilation.  
**Runtime stage** uses the slim ASP.NET image (~220 MB) for the final container.

### docker-compose.yml

```yaml
services:
  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      ACCEPT_EULA: "Y"
      MSSQL_SA_PASSWORD: "YourStr0ng!Pass"
    ports:
      - "1433:1433"
    healthcheck:
      test: /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "..." -C -Q "SELECT 1"
      interval: 10s
      retries: 10
      start_period: 30s

  web:
    build: .
    ports:
      - "8080:8080"
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      ConnectionStrings__DefaultConnection: "Server=db;..."
    depends_on:
      db:
        condition: service_healthy
```

The `depends_on` with `condition: service_healthy` ensures the web app doesn't start until SQL Server is accepting connections.
