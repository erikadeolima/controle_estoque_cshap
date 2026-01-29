# Inventory Control API

A RESTful API for snack bar inventory management, built with Clean Architecture principles, SOLID design, and modern C# patterns.

## 📋 Project Overview

[TBD] - Add project description and business context

**Related Documentation:**

- [REQUISITOS.md](REQUISITOS.md) - Business requirements (Portuguese)
- [REQUIREMENTS.md](REQUIREMENTS.md) - Business requirements (English)
- [PLANO_DESENVOLVIMENTO.md](PLANO_DESENVOLVIMENTO.md) - Development plan for learning (Portuguese)

---

## ✨ Features

[TBD] - Feature list to be completed

---

## 🏗️ Architecture

This project follows **Clean Architecture** with clear separation of concerns across four layers:

```
src/
├── Domain/              # Business logic and domain rules
│   ├── Entities/       # Domain entities (BaseEntity, Produto, Categoria)
│   └── Interfaces/     # Repository contracts
├── Application/         # Application use cases and orchestration
│   ├── DTOs/           # Data Transfer Objects
│   ├── Interfaces/     # Service contracts
│   ├── Strategies/     # Strategy pattern implementations
│   └── UseCases/       # Business use cases (ProdutoService)
├── Infrastructure/      # Technical implementations
│   ├── Data/           # Entity Framework DbContext
│   ├── Repositories/   # Repository implementations
│   └── Services/       # Infrastructure services (Logger)
└── API/                # Presentation layer
    └── Controllers/    # REST API endpoints
```

### Architecture Benefits

- **Independence**: Framework and database agnostic domain logic
- **Testability**: Each layer can be tested independently
- **Flexibility**: Easy to swap implementations (e.g., different databases)
- **Scalability**: Clear responsibility boundaries

---

## 🎯 Design Patterns Implemented

### 1. Strategy Pattern

- `IValidacaoStrategy<T>` - Interface for validation strategies
- `ValidacaoProdutoStrategy` - Concrete validation implementation

### 2. Repository Pattern

- `IRepository<T>` - Generic repository interface
- `Repository<T>` - Generic repository base implementation
- `ProdutoRepository`, `CategoriaRepository` - Specific repositories

### 3. Singleton Pattern

- `LoggerService` - Thread-safe singleton logging service

### 4. Factory Method Pattern

- `Create()` methods in entities control object instantiation

### 5. Dependency Injection

- Configured in `Program.cs` following DIP (Dependency Inversion Principle)

---

## ✅ SOLID Principles Applied

| Principle                     | Implementation                                                                                                     |
| ----------------------------- | ------------------------------------------------------------------------------------------------------------------ |
| **S** - Single Responsibility | Each class has one reason to change. Services, repositories, and entities have distinct responsibilities.          |
| **O** - Open/Closed           | Classes are open for extension via inheritance and interfaces, closed for modification.                            |
| **L** - Liskov Substitution   | `Produto` and `Categoria` properly substitute `BaseEntity`. `ProdutoRepository` substitutes `Repository<Produto>`. |
| **I** - Interface Segregation | Specific, focused interfaces. Clients don't depend on methods they don't use.                                      |
| **D** - Dependency Inversion  | Dependencies injected as abstractions (interfaces), not concrete implementations.                                  |

---

## 🔄 OOP Concepts

- **Inheritance**: `Produto` and `Categoria` inherit from `BaseEntity`
- **Polymorphism**: Repositories override base methods; `IValidacaoStrategy<T>` allows different implementations
- **Encapsulation**: Private setters on entity properties; business logic validation inside domain

---

## 🛠️ Technology Stack

| Technology            | Version | Purpose             |
| --------------------- | ------- | ------------------- |
| .NET                  | 8.0     | Runtime framework   |
| ASP.NET Core          | 8.0     | Web API framework   |
| Entity Framework Core | 8.0     | ORM                 |
| MySQL                 | 8.0+    | Relational database |
| Swagger/OpenAPI       | -       | API documentation   |

---

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK installed
- IDE: Visual Studio Code, Visual Studio, or JetBrains Rider

### Installation

```bash
# Clone repository
git clone [repository-url]
cd controle_estoque_cshap

# Restore dependencies
dotnet restore

# Build project
dotnet build
```

### Running the Application

```bash
# Run the API
dotnet run

# API will be available at
https://localhost:5001

# Swagger UI
https://localhost:5001/swagger
```

---

## 📡 API Endpoints

### Products (`/api/produtos`)

| Method | Endpoint                               | Description        | Status                         |
| ------ | -------------------------------------- | ------------------ | ------------------------------ |
| GET    | `/api/produtos`                        | List all products  | 200 OK                         |
| GET    | `/api/produtos/{id}`                   | Get product by ID  | 200 OK / 404 Not Found         |
| POST   | `/api/produtos`                        | Create new product | 201 Created / 400 Bad Request  |
| PUT    | `/api/produtos/{id}`                   | Update product     | 200 OK / 404 Not Found         |
| DELETE | `/api/produtos/{id}`                   | Deactivate product | 204 No Content / 404 Not Found |
| POST   | `/api/produtos/{id}/estoque/adicionar` | Add to stock       | 200 OK / 400 Bad Request       |
| POST   | `/api/produtos/{id}/estoque/remover`   | Remove from stock  | 200 OK / 400 Bad Request       |

### Request Example - Create Product

```json
POST /api/produtos
Content-Type: application/json

{
  "nome": "Hambúrguer Premium",
  "descricao": "Hambúrguer 250g com queijo cheddar",
  "preco": 25.90,
  "quantidadeEstoque": 50,
  "categoria": "Lanches",
  "codigoBarras": "7891234567890"
}
```

---

## 🗃️ Database

**Database**: MySQL 8.0+

### Setup Instructions

1. **Install MySQL** if not already installed

2. **Create database**:

   ```sql
   CREATE DATABASE estoque_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
   ```

3. **Update `appsettings.json`**:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=estoque_db;User=root;Password=your_password;"
     }
   }
   ```

4. **Install MySQL NuGet package** (if not already installed):

   ```bash
   dotnet add package Pomelo.EntityFrameworkCore.MySql
   ```

5. **Update `Program.cs`**:

   ```csharp
   builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseMySql(
           builder.Configuration.GetConnectionString("DefaultConnection"),
           new MySqlServerVersion(new Version(8, 0, 0))
       )
   );
   ```

6. **Create migrations and update database**:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

---

## 📚 Documentation

| Document                                             | Purpose                                     |
| ---------------------------------------------------- | ------------------------------------------- |
| [REQUISITOS.md](REQUISITOS.md)                       | Business requirements (Portuguese)          |
| [REQUIREMENTS.md](REQUIREMENTS.md)                   | Business requirements (English)             |
| [PLANO_DESENVOLVIMENTO.md](PLANO_DESENVOLVIMENTO.md) | Step-by-step development guide for learning |

---

## 🧪 Testing

[TBD] - Testing information to be added:

- Unit tests
- Integration tests
- Test coverage

---

## 📝 Code Standards

- Clean Code practices: meaningful names, small methods, DRY principle
- XML documentation for public methods
- Async/await for all I/O operations
- Proper exception handling

---

## 🔄 Future Enhancements

- [ ] Unit and integration tests (xUnit, Moq)
- [ ] Authentication/Authorization (JWT)
- [ ] Input validation (FluentValidation)
- [ ] Structured logging (Serilog)
- [ ] Pagination support
- [ ] API versioning
- [ ] Caching (Redis)
- [ ] CI/CD pipeline
- [ ] Database migrations (SQL Server)
- [ ] Performance optimization

---

## 🤝 Contributing

[TBD] - Add contribution guidelines

---

## 📄 License

[TBD] - Add license information

---

## 📞 Contact & Support

[TBD] - Add contact information

---

**Built with Clean Architecture and SOLID principles** ✨
