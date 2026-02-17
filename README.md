# Inventory Control API

A RESTful API for snack bar inventory management, built with Clean Architecture principles, SOLID design, and modern C# patterns.

## 📋 Project Overview

Inventory control system for snack bar specialized in food product management. The system allows complete control of product lifecycle, from registration to movement history, including low stock alerts and expiration date management.

**Implemented Data Model:**

- **Categories**: Product organization (snacks, beverages, ingredients, etc.)
- **Products**: Stock items with unique SKU and status control
- **Items**: Individual batches with quantity, expiration date, and location tracking
- **Movements**: Complete history of stock entries, exits, and adjustments
- **Users**: Control of who performed each movement

**Related Documentation:**

- [REQUISITOS.md](REQUISITOS.md) - Business requirements (Portuguese)
- [REQUIREMENTS.md](REQUIREMENTS.md) - Business requirements (English)
- [PLANO_DESENVOLVIMENTO.md](PLANO_DESENVOLVIMENTO.md) - Development plan for learning (Portuguese)

---

## ✨ Features

### ✅ Implemented

- **Category Management**
  - Hierarchical product organization
  - Description and metadata

- **Product Control**
  - Unique SKU required
  - Configurable status (Active/Inactive)
  - Customizable minimum quantity
  - Relationship with categories

- **Item/Batch Management**
  - Batch control
  - Expiration date tracking
  - Physical location management
  - Automatic status (Available/Alert/Out of Stock)
  - Insufficient stock validation

- **Movement History**
  - Stock entries, exits, and adjustments logging
  - Complete audit with responsible user
  - Quantity snapshots (previous/new)
  - Automatic timestamp

- **Reports** ✅ NEW
  - **Expiration Report**: Items expiring within specified days
  - **Expired Items Report**: Items that have already expired
  - CSV export format for spreadsheet compatibility
  - Uses **JOIN** queries across multiple tables

- **MySQL Database**
  - Entity Framework Core configured
  - Migrations implemented
  - Relationships with referential integrity

### 🚧 Next Implementations

- [ ] Authentication system
- [ ] Low stock alerts
- [ ] Automatic deletion of old products
- [ ] REST API documentation enhancement

---

## 🏗️ Architecture

The project currently follows a **layered monolithic architecture** with ASP.NET Core Web API:

```
controle_estoque_cshap/
├── Models/              # Domain entities
│   ├── Category.cs     # Product categories
│   ├── Product.cs      # Products with SKU and status
│   ├── Item.cs         # Individual items/batches
│   ├── Movement.cs     # Movement history
│   └── User.cs         # System users
├── Data/               # Persistence layer
│   └── AppDbContext.cs # EF Core context with configurations
├── Controllers/        # REST API endpoints (to implement)
├── DTOs/              # Data Transfer Objects (to implement)
├── Services/          # Business logic (to implement)
├── Repositories/      # Data access layer (to implement)
└── Migrations/        # Database migrations
```

### Domain Model

**Main Entities:**

1. **Category** - Product categorization
   - 1:N relationship with Products

2. **Product** - Registered product
   - Unique SKU required
   - Status: Active/Inactive
   - 1:N relationship with Items

3. **Item** - Individual product batch
   - Quantity control per batch
   - Expiration date and location

- Automatic status: Available/Alert/Out of Stock
- 1:N relationship with Movements

4. **Movement** - Stock movement
   - Types: Entry/Exit/Adjustment
   - Complete audit (date, user, quantities)

5. **User** - System user
   - Records who performed each movement

---

## 🎯 Design Patterns & OOP Principles

### Implemented Patterns

#### 1. Entity Pattern (DDD)

- Entities with controlled constructors
- Business logic encapsulation
- Private setters for critical properties (Id, DataCriacao)

#### 2. Value Objects (Enums)

- `ProductStatus`: Active/Inactive (Ativo/Inativo)
- `ItemStatus`: Available/Alert/Out of Stock (Disponivel/Alerta/Esgotado)
- `MovementType`: Entry/Exit/Adjustment (Entrada/Saída/Ajuste)

#### 3. Rich Domain Model

- Business methods in entities:
  - `Product.Ativar()` / `Desativar()` (Activate/Deactivate)
  - `Product.ValidarQuantidadeMinima()` (Validate minimum quantity)
  - `Item.AdicionarQuantidade()` / `RemoverQuantidade()` (Add/Remove quantity)
  - `Item.AtualizarStatus()` (Update status)

### OOP Principles Applied

- **Encapsulation**: Properties with private setters, protected internal logic
- **Abstraction**: Separation between domain models and persistence
- **Single Responsibility**: Each entity manages its own rules
- **Immutability**: IDs and timestamps set only on creation

### Planned Patterns

- Repository Pattern (data layer)
- Unit of Work (transactions)
- Strategy Pattern (validations)
- Dependency Injection (services)

---

## ✅ SOLID Principles Applied

| Principle                     | Current Implementation                                                                             |
| ----------------------------- | -------------------------------------------------------------------------------------------------- |
| **S** - Single Responsibility | Each Model has a single responsibility: Product (product), Item (batch), Movement (stock movement) |
| **O** - Open/Closed           | Entities allow extension through inheritance and public methods, closed for direct modification    |
| **L** - Liskov Substitution   | To be implemented with repository and service interfaces                                           |
| **I** - Interface Segregation | To be implemented with specific interfaces for each repository                                     |
| **D** - Dependency Inversion  | DbContext injected via DI in Program.cs; will be expanded with repositories and services           |

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

**Status**: 🚧 In development - Controllers not yet implemented

### Planned Endpoints

#### Categories (`/api/categories`)

| Method | Endpoint               | Description     |
| ------ | ---------------------- | --------------- |
| GET    | `/api/categories`      | List categories |
| GET    | `/api/categories/{id}` | Get by ID       |
| POST   | `/api/categories`      | Create category |
| PUT    | `/api/categories/{id}` | Update category |

#### Products (`/api/products`)

| Method | Endpoint                  | Description            |
| ------ | ------------------------- | ---------------------- |
| GET    | `/api/products`           | List active products   |
| GET    | `/api/products/inactive`  | List inactive products |
| GET    | `/api/products/{id}`      | Get product by ID      |
| GET    | `/api/products/sku/{sku}` | Get by SKU             |
| GET    | `/api/products/low-stock` | Get low stock products |
| POST   | `/api/products`           | Create product         |
| PUT    | `/api/products/{id}`      | Update product         |
| DELETE | `/api/products/{id}`      | Deactivate (soft)      |

#### Items (`/api/items`)

| Method | Endpoint                          | Description           |
| ------ | --------------------------------- | --------------------- |
| GET    | `/api/products/{productId}/items` | List items by product |
| GET    | `/api/items/{id}`                 | Get item by ID        |
| GET    | `/api/items/expiring?days=7`      | Get expiring items    |
| POST   | `/api/products/{productId}/items` | Create batch/item     |
| PUT    | `/api/items/{id}`                 | Update item           |
| DELETE | `/api/items/{id}`                 | Deactivate (soft)     |
| POST   | `/api/items/{id}/add-quantity`    | Add stock quantity    |
| POST   | `/api/items/{id}/remove-quantity` | Remove stock quantity |

---

## 📌 Item Status Rules

The `Item.Status` field uses a numeric convention that aligns with business rules:

| Value | Meaning      | Rule                                                                       |
| ----- | ------------ | -------------------------------------------------------------------------- |
| 0     | Inactive     | Item is inactive and cannot be updated or reactivated via update endpoints |
| 1     | Available    | Quantity is above the minimum threshold                                    |
| 2     | Alert        | Quantity is greater than 0 and less than or equal to minimum quantity      |
| 3     | Out of Stock | Quantity equals 0                                                          |

**Notes:**

- Inactive items are not allowed to be updated.
- Status is calculated automatically from quantity and product minimum quantity.

#### Movements (`/api/movements`)

| Method | Endpoint                               | Description         |
| ------ | -------------------------------------- | ------------------- |
| GET    | `/api/items/{itemId}/movements`        | Movements by item   |
| GET    | `/api/movements?startDate=X&endDate=Y` | Movements by period |

#### Reports (`/api/items/reports`) - **NEW**

| Method | Endpoint                               | Description                           | Returns  |
| ------ | -------------------------------------- | ------------------------------------- | -------- |
| GET    | `/api/items/reports/expiration?days=7` | Items expiring within N days (CSV)    | CSV File |
| GET    | `/api/items/reports/expired`           | Items that have already expired (CSV) | CSV File |

**Features:**

- ✅ Uses **JOIN** queries across Item → Product → Category tables
- ✅ Returns downloadable CSV files with:
  - Item batch, quantity, location, expiration date
  - Product name, SKU
  - Category name
  - Days until expiration / Days since expiration (for expired items)
- ✅ Filters with parameters (days to warning)

### Request Examples (Planned)

```json
// POST /api/products
{
  "sku": "PROD-001",
  "name": "Hamburguer Artesanal",
  "categoryId": 1,
  "minimumQuantity": 10
}

// POST /api/products/{productId}/items
{
  "batch": "LOTE-2026-01",
  "expirationDate": "2026-12-31",
  "location": "Prateleira A3",
  "quantity": 50
}

// POST /api/items/{id}/add-quantity
{
  "quantidade": 50,
  "userId": 1
}
```

---

## 🗃️ Database

**Database**: MySQL 8.0.34+  
**ORM**: Entity Framework Core 8.0

### ✅ Current Configuration

The project is already configured with:

- ✅ Pomelo.EntityFrameworkCore.MySql 8.0.0
- ✅ AppDbContext configured
- ✅ Migrations created (`20260204224049_Initial`)
- ✅ Relationships and indexes configured

### Setup Instructions

1. **Install MySQL 8.0.34+**

2. **Create database**:

   ```bash
   mysql -u root -p
   ```

   ```sql
   CREATE DATABASE controle_estoque CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
   CREATE USER 'estoque_user'@'localhost' IDENTIFIED BY 'your_secure_password';
   GRANT ALL PRIVILEGES ON controle_estoque.* TO 'estoque_user'@'localhost';
   FLUSH PRIVILEGES;
   EXIT;
   ```

3. **Configure connection string** in `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=controle_estoque;User=estoque_user;Password=your_secure_password;"
     }
   }
   ```

4. **Apply migrations** (already existing):
   ```bash
   dotnet ef database update
   ```

### Database Structure

**Tables:**

- `Categories` - Product categories
- `Products` - Products (with unique index on SKU)
- `Items` - Product items/batches
- `Movements` - Movement history
- `Users` - System users

**Relationships:**

- Category 1:N Products (DeleteBehavior.Restrict)
- Product 1:N Items (DeleteBehavior.Restrict)
- Item 1:N Movements (DeleteBehavior.Restrict)
- User 1:N Movements (DeleteBehavior.Restrict)

### Available Scripts

See [Database/Scripts](Database/Scripts/) folder for:

- Database creation
- User creation
- Database reset
- Seed data examples

---

## 📚 Documentation

| Document                                             | Purpose                                     |
| ---------------------------------------------------- | ------------------------------------------- |
| [REQUISITOS.md](REQUISITOS.md)                       | Business requirements (Portuguese)          |
| [REQUIREMENTS.md](REQUIREMENTS.md)                   | Business requirements (English)             |
| [PLANO_DESENVOLVIMENTO.md](PLANO_DESENVOLVIMENTO.md) | Step-by-step development guide for learning |

---

## 🧪 Testing

**Status**: ✅ Implemented & Comprehensive

### Coverage & Results

- **Total Tests**: 152 (All passing ✅)
- **Code Coverage**: **86.48%** (Exceeds 80% minimum)
- **Branch Coverage**: 83.65%
- **Method Coverage**: 88.74%

### Test Framework & Tools

- **Framework**: NUnit
- **Mocking**: Moq
- **Coverage Tool**: Coverlet

### Test Categories

1. **Service Tests** (Business Logic)
2. **Repository Tests** (Data Access with JOIN queries)
3. **Controller Tests** (API Endpoints)
4. **Integration Tests** (End-to-end workflows)

### Running Tests

```bash
# Run all tests
dotnet test controle_estoque_cshap.Tests/controle_estoque_cshap.Tests.csproj

# Run with coverage
dotnet test controle_estoque_cshap.Tests/controle_estoque_cshap.Tests.csproj \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=opencover
```

---

## 📝 Code Standards

### Adopted Conventions

- **Naming**: PascalCase for properties, camelCase for parameters
- **Language**: Portuguese for domain properties, English for technical code
- **Encapsulation**: Private setters for critical properties
- **Immutability**: IDs and timestamps set only in constructor
- **Validations**: Descriptive exceptions (`ArgumentException`, `InvalidOperationException`)
- **Entity Framework**: Fluent API configurations in OnModelCreating

### Applied Best Practices

- ✅ Controlled constructors to ensure valid state
- ✅ Business methods inside entities (Rich Domain Model)
- ✅ Navigation properties for relationships
- ✅ Enums for fixed values
- ✅ Nullable reference types enabled
- ✅ Explicit configuration of indexes and constraints

---

## 🔄 Roadmap

### Phase 1: API Core ✅ Complete

- ✅ Implement Controllers (Category, Product, Item, User)
- ✅ Create DTOs for requests/responses
- ✅ Implement Repository Pattern
- ✅ Implement Service Layer
- ✅ Validations and error handling
- ✅ Reports with JOIN queries
- ✅ Comprehensive testing (86.48% coverage)

### Phase 2: Business Rules & Enhancements

- [ ] Low stock alert system
- [ ] Expiration date notifications
- [ ] Automatic deletion of old products
- [ ] Movement reports
- [ ] Item audit trail (user, datetime, changed fields)
- [ ] Authentication & Authorization

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
