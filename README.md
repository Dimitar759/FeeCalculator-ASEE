# FeeCalculator-ASEE

## 🧱 Architecture

This solution is structured across multiple projects for clear separation of concerns:

| Project                     | Description |
|-----------------------------|-------------|
| `FeeCalculator.Domain`      | Contains core business models (`Transaction`, `FeeRule`, etc.) |
| `FeeCalculator.Application` | Contains business logic and interfaces (fee calculation services, etc.) |
| `FeeCalculator.DataAccess`  | Handles DB context and Entity Framework setup |
| `FeeCalculator.API`         | ASP.NET Core Web API project exposing RESTful endpoints |

## Technologies Used

- .NET 8
- Entity Framework Core
- SQL Server
- Swagger (Swashbuckle)
- Clean Architecture Pattern
- Dependency Injection


## Libraries Used

  - `Microsoft.EntityFrameworkCore` 
  - `Microsoft.EntityFrameworkCore.SqlServer` 
  - `Microsoft.EntityFrameworkCore.Design` 
  - `Microsoft.EntityFrameworkCore.Tools` 
  - `Newtonsoft.Json` 
  - `Swashbuckle.AspNetCore` 


## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/FeeCalculator.git
cd FeeCalculator
```

### 2. Update the Connection String

In `appsettings.json` of the `FeeCalculator.API` project:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=FeeCalculatorDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

Update `Server` and `Database` values as needed.

### 3. Apply EF Core Migrations

Open the terminal in the solution directory and run:

```bash
cd FeeCalculator.API
dotnet ef database update
```

This will create the database and tables in SQL Server based on the models.

### 4. Run the API

```bash
dotnet run --project FeeCalculator.API
```

Swagger UI will be available at:

```
https://localhost:{port}/swagger
```



##  Design Considerations

- Clean separation of concerns through Clean Architecture
- Easily extendable fee rules logic
- Database-first fee configuration (dynamic rules)
- History and audit support for fee calculations

