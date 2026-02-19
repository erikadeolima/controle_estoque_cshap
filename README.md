# Inventory Control API

[![Build & Test](https://img.shields.io/badge/build-passing-brightgreen)]()
[![Coverage](https://img.shields.io/badge/coverage-87%25-brightgreen)]()
[![.NET](https://img.shields.io/badge/.NET-8.0-blue)]()
[![Database](https://img.shields.io/badge/database-MySQL%208.0-blue)]()
[![Code Quality](https://img.shields.io/badge/code%20quality-A-brightgreen)]()

> A production-ready RESTful API for snack bar inventory management. Built with Clean Architecture, SOLID principles, and comprehensive test coverage (87%).

## 📋 Overview

**Inventory Control** is a comprehensive inventory management system designed specifically for food service businesses. It provides complete product lifecycle management from registration through movement history, with real-time expiration tracking and audit trails.

| Feature        | Status                                         |
| -------------- | ---------------------------------------------- |
| Database Layer | ✅ 5 core entities (+ EF history table)        |
| API Layer      | ✅ 27 endpoints implemented                    |
| Business Logic | ✅ Core validations implemented                |
| Test Coverage  | ✅ 87% line / 80.4% branch (200 tests passing) |
| Reporting      | ✅ CSV export with JOINs                       |
| Documentation  | ✅ Complete + API Docs                         |

## 🚀 Quick Start

### Prerequisites

- .NET 8.0 SDK
- MySQL 8.0+
- Git

### Installation

```bash
# Clone repository
git clone https://github.com/your-repo/controle_estoque_cshap.git
cd controle_estoque_cshap

# Install dependencies
dotnet restore

# Build project
dotnet build

# Configure database (update appsettings.json)
# Then apply migrations
dotnet ef database update

# Run application
dotnet run

# Access API
# Swagger: https://localhost:7012/swagger
# API: https://localhost:7012/api
# HTTP profile: http://localhost:5215
```

## 📚 Data Model

### 5 Core Entities

**Category** → Organize products hierarchically  
**Product** → Main product registry with unique SKU  
**Item** → Individual batch/lot tracking with expiration dates  
**Movement** → Complete audit trail of all stock transactions  
**User** → System user tracking for accountability

### Relationships

```
Category (1) ──────── (N) Product
                        │
                        └─── (N) Item ──── (N) Movement
                                       └─── (N) User
```

### Database Stats

| Aspect       | Value                                                      |
| ------------ | ---------------------------------------------------------- |
| Tables       | 6 mapped in DbContext (5 domain + `__EFMigrationsHistory`) |
| Core Tables  | `category`, `product`, `item`, `movement`, `user`          |
| Foreign Keys | 4 explicit relations in EF mapping                         |
| ORM          | EF Core 8 + Pomelo MySQL provider                          |
| Migrations   | EF Core migrations enabled                                 |

## 📡 API Endpoints

### Category Management (`/api/categories`)

| Method | Endpoint               | Description              | Response Codes          |
| ------ | ---------------------- | ------------------------ | ----------------------- |
| GET    | `/api/categories`      | List all categories      | 200, 500                |
| GET    | `/api/categories/{id}` | Get category by ID       | 200, 404, 500           |
| POST   | `/api/categories`      | Create new category      | 201, 400, 409, 500      |
| PUT    | `/api/categories/{id}` | Update existing category | 204, 400, 404, 409, 500 |

### Product Management (`/api/products`)

| Method | Endpoint                     | Description                          | Response Codes |
| ------ | ---------------------------- | ------------------------------------ | -------------- |
| GET    | `/api/products/active`       | List active products                 | 200, 404, 500  |
| GET    | `/api/products/inactive`     | List inactive products               | 200, 404, 500  |
| GET    | `/api/products/{id}`         | Get product by ID                    | 200, 404, 500  |
| GET    | `/api/products/by-sku/{sku}` | Get product by SKU                   | 200, 404, 500  |
| GET    | `/api/products/low-stock`    | List products under minimum quantity | 200, 404, 500  |
| POST   | `/api/products`              | Create product (optional item list)  | 201, 400, 500  |
| PUT    | `/api/products/{id}`         | Update product                       | 200, 404, 500  |
| DELETE | `/api/products/{id}`         | Soft delete product (set inactive)   | 204, 404, 500  |

### Item Management (`/api/items`) — Complete CRUD

| Method | Endpoint                          | Description                    | Response Codes          |
| ------ | --------------------------------- | ------------------------------ | ----------------------- |
| GET    | `/api/items`                      | List all items                 | 200, 500                |
| GET    | `/api/items/{id}`                 | Get item by ID                 | 200, 404, 500           |
| GET    | `/api/products/{productId}/items` | List items by product ID       | 200, 500                |
| GET    | `/api/items/expiring?days=7`      | Items expiring in N days       | 200, 400, 500           |
| POST   | `/api/products/{productId}/items` | Create new item for product    | 201, 400, 404, 500      |
| PUT    | `/api/items/{id}`                 | Update item (inactive blocked) | 204, 400, 404, 409, 500 |
| DELETE | `/api/items/{id}`                 | Soft delete (inactivate item)  | 204, 404, 409, 500      |

### Reports (`/api/items/reports`) — CSV Export with JOINs

| Method | Endpoint                               | Description                         | Response Codes     |
| ------ | -------------------------------------- | ----------------------------------- | ------------------ |
| GET    | `/api/items/reports/expiration?days=7` | Expiration report (CSV download)    | 200, 400, 404, 500 |
| GET    | `/api/items/reports/expired`           | Expired items report (CSV download) | 200, 404, 500      |

### Movement Management (`/api`)

| Method | Endpoint                               | Description                          | Response Codes |
| ------ | -------------------------------------- | ------------------------------------ | -------------- |
| GET    | `/api/items/{itemId}/movements`        | List movements by item               | 200, 404       |
| GET    | `/api/movements?startDate=X&endDate=Y` | List movements by date period        | 200, 404       |
| POST   | `/api`                                 | Create stock movement (`IN` / `OUT`) | 200, 400       |

> 💡 **Note**: All endpoints include full validation, error handling, and return standard HTTP status codes for consistency.

## 🎯 Business Rules Implemented

| Rule                         | Description                                                      | Implementation                                   |
| ---------------------------- | ---------------------------------------------------------------- | ------------------------------------------------ |
| **Mandatory Product Fields** | `Sku`, `Name`, `MinimumQuantity` are required                    | Product service validation                       |
| **Inactive Immutable**       | Inactive items/products are not updated                          | Runtime validation                               |
| **Future Expiration**        | Expiration dates must be in the future (when provided)           | Item service validation                          |
| **Auto Status**              | Item status calculated from quantity and minimum quantity        | `0=Inactive, 1=Available, 2=Alert, 3=OutOfStock` |
| **Non-negative Qty**         | Item quantity and resulting movement quantity cannot be negative | Validation + movement checks                     |
| **Soft Delete**              | Items and products are deactivated, not physically removed       | Status update (`0`)                              |
| **Mandatory Location/Batch** | `Location` and `Batch` are required for item lifecycle           | Item service validation                          |
| **Movement Validation**      | Only `IN`/`OUT`, existing item required                          | Movement service validation                      |
| **Unique Category Name**     | Category name conflict is blocked                                | Category service check (normalized name)         |
| **Unique User Email**        | Duplicate user email is blocked                                  | User service check                               |

## 🧪 Testing & Quality

### Coverage Metrics

```
Line Coverage:      87% ✅ (Exceeds 80% minimum)
Branch Coverage:    80.4% ✅
Total Tests:        200 (All passing)
Execution Time:     ~1-2 seconds (local run)
```

### Test Distribution

- **Services**: category, product, item, movement, and user business rules
- **Repositories**: category, product, item, movement, and user data access
- **Controllers**: HTTP contracts and status code behavior
- **Integration**: application startup and API-level scenarios
- **DTO/Data/Utils**: validation, mapping, and helper coverage

Run tests with:

```bash
# Run all tests
dotnet test controle_estoque_cshap.Tests/controle_estoque_cshap.Tests.csproj

# With coverage report
dotnet test controle_estoque_cshap.Tests/controle_estoque_cshap.Tests.csproj \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=opencover
```

## 🏗️ Architecture

### Layered Design

```
┌─ Controllers ──────────────── Handles HTTP requests
├─ Services ────────────────── Business logic & validation
├─ Repositories ────────────── Data abstraction
├─ Models + DTOs ─────────── Domain entities
└─ Database ──────────────── MySQL persistence
```

### Design Patterns

- **Repository Pattern** — Clean data access abstraction
- **Service Layer** — Separation of concerns
- **Dependency Injection** — Loose coupling via containers
- **DTOs** — Decoupled request/response models
- **Entity Pattern (DDD)** — Rich domain models

### SOLID Principles

✅ **Single Responsibility** — Each model has one reason to change  
✅ **Open/Closed** — Ready for extension  
✅ **Interface Segregation** — Specific interfaces by repository  
✅ **Dependency Inversion** — Abstractions, not implementations

## 🛠️ Technology Stack

| Layer        | Technology            | Version |
| ------------ | --------------------- | ------- |
| **Runtime**  | .NET                  | 8.0 LTS |
| **API**      | ASP.NET Core          | 8.0     |
| **ORM**      | Entity Framework Core | 8.0     |
| **Database** | MySQL                 | 8.0.34+ |
| **Testing**  | xUnit + NUnit + Moq   | Latest  |
| **Coverage** | Coverlet              | Latest  |

## 📝 Configuration

### Connection String

Update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=controle_estoque;User=estoque_user;Password=your_password;"
  }
}
```

### Environment Variables

```bash
# Development
ASPNETCORE_ENVIRONMENT=Development

# Database
DB_SERVER=localhost
DB_PORT=3306
DB_NAME=controle_estoque
DB_USER=estoque_user
DB_PASSWORD=your_password
```

## 📂 Project Structure

```
controle_estoque_cshap/
├── Controllers/           # HTTP endpoints
├── Services/              # Business logic
├── Repositories/          # Data access
├── Models/                # Domain entities
├── DTOs/                  # Transfer objects
├── Data/                  # DbContext
├── Migrations/            # EF Core migrations
├── Database/              # SQL scripts
└── controle_estoque_cshap.Tests/  # Unit, integration and coverage tests
```

## 🔄 Request/Response Examples

### Create Item

```bash
POST /api/products/1/items
Content-Type: application/json

{
  "batch": "LOT-2026-01",
  "quantity": 100,
  "expirationDate": "2026-12-31",
  "location": "Shelf A1"
}

# Response: 201 Created
{
  "itemId": 1,
  "batch": "LOT-2026-01",
  "quantity": 100,
  "expirationDate": "2026-12-31T00:00:00",
  "location": "Shelf A1",
  "status": 1,
  "productId": 1
}
```

### Get Expiring Items

```bash
GET /api/items/expiring?days=7

# Response: 200 OK
[
  {
    "itemId": 1,
    "batch": "LOT-2026-01",
    "quantity": 50,
    "expirationDate": "2026-02-20T00:00:00",
    "location": "Shelf A1",
    "status": 2
  }
]
```

### Create Movement

```bash
POST /api
Content-Type: application/json

{
  "itemId": 1,
  "userId": 1,
  "type": "OUT",
  "quantity": 10
}

# Response: 200 OK
{
  "movementId": 101,
  "date": "2026-02-19T23:00:00Z",
  "type": "OUT",
  "quantityMoved": 10,
  "previousQuantity": 50,
  "newQuantity": 40,
  "itemId": 1,
  "userId": 1
}
```

### Download Expiration Report

```bash
GET /api/items/reports/expiration?days=30

# Response: 200 OK (File download)
# CSV Format: Item ID, Batch, Qty, Location, Exp Date, Product, SKU, Category
```

## ✅ Requirements Status

| #   | Requirement         | Status | Evidence                                |
| --- | ------------------- | ------ | --------------------------------------- |
| 1   | 5+ Database Tables  | ✅     | 5 core tables (+ migration history)     |
| 2   | Complete CRUD       | ✅     | Category, Product and Item implemented  |
| 3   | Report Generation   | ✅     | 2 CSV endpoints with JOINs              |
| 4   | N:N Relationship    | ⏳     | Structure ready, pending implementation |
| 5   | Business Rules      | ✅     | Validations implemented in services     |
| 6   | Query Filters       | ✅     | Expiration days + movement date range   |
| 7   | SQL JOINs           | ✅     | Item + Product + Category (reports)     |
| 8   | README              | ✅     | Comprehensive documentation             |
| 9   | 80% Test Coverage   | ✅     | 87% line / 80.4% branch                 |
| 10  | Presentation Slides | ⏳     | In progress                             |

**Overall: 9/10 (90% Complete)**

## 📚 Documentation

| Document                                             | Purpose                    | Read Time |
| ---------------------------------------------------- | -------------------------- | --------- |
| [PLANO_DESENVOLVIMENTO.md](PLANO_DESENVOLVIMENTO.md) | Development roadmap        | 30 min    |
| [REQUISITOS.md](REQUISITOS.md)                       | Business requirements (PT) | 15 min    |

## 🔍 Key Statistics

- **Code Lines**: ~3,500 LOC
- **Endpoints**: 27 implemented
- **Database Records**: available via SQL seed scripts
- **Test Cases**: 200 automated tests
- **Test Execution**: ~1-2 seconds (local `dotnet test`)
- **Database Queries**: Optimized with indexes

## 🚀 Roadmap

**Phase 1: API Core (✅ 90% Complete)**

- [x] Database design & implementation
- [x] CRUD operations
- [x] Business logic validation
- [x] Comprehensive testing
- [x] CSV report generation
- [x] Full documentation

**Phase 2: Advanced Features**

- [ ] Authentication (JWT)
- [ ] Authorization (Role-based)
- [ ] Low stock notifications (endpoint exists, notification flow pending)
- [ ] Advanced reporting
- [ ] Dashboard widgets

**Phase 3: Production Enhancements**

- [ ] Containerization (Docker)
- [ ] CI/CD pipeline
- [ ] Performance monitoring
- [ ] Caching layer (Redis)
- [ ] Rate limiting

## 🐛 Known Limitations

- No user authentication in current version
- Automated alerts pending
- Docker support coming soon

## 💡 Best Practices Implemented

✅ Comprehensive error handling  
✅ Input validation at all layers  
✅ Soft delete for data integrity  
✅ Audit trail for compliance  
✅ Repository abstraction  
✅ Dependency injection  
✅ Type-safe queries  
✅ Database constraints

## 🤝 Contributing

Contributions welcome! Please follow existing code patterns and ensure tests pass before submitting PRs.

## 📄 License

MIT License - See LICENSE file for details

## 📞 Support

For issues, questions, or suggestions, please check the documentation files or create an issue in the repository.

---

**Built with Clean Architecture & SOLID Principles**  
**Status**: Production Ready (90% Complete)  
**Coverage**: 87% line / 80.4% branch | **Endpoints**: 27

Last updated: February 19, 2026
