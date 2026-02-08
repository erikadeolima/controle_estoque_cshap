# 📋 Plano de Desenvolvimento - Sistema de Controle de Estoque (MVC)

## 🎯 Objetivo

Desenvolver uma API REST para gerenciamento de estoque de lanchonete com **controle de lotes, datas de validade, histórico de movimentações e auditoria**.

O sistema deve permitir rastreabilidade completa de produtos alimentícios, desde cadastro até consumo.

---

## 📊 Modelo de Dados (High Level)

```
CATEGORIA
├── Cadastro mestre de categorias de produtos

PRODUTO
├── Cadastro mestre (SKU único, quantidade mínima, status)
├── Associado a CATEGORIA

ITEM (Lote)
├── Instância específica de um PRODUTO
├── Controla: batch, data validade, quantidade, localização, status

MOVIMENTAÇÃO
├── Histórico de entrada/saída/ajuste de ITEM
├── Rastreia: tipo, quantidade, estoque anterior/novo, usuário, data

USUÁRIO
├── Auditoria de quem fez cada operação
```

---

## 🏗️ Arquitetura (Padrão MVC/MSC)

```
Controllers (API REST)
    ↓ (dependem de)
Services (Lógica e validações)
    ↓ (dependem de)
Repositories (Acesso a dados)
    ↓ (dependem de)
Models (Entidades) + DTOs + Data (DbContext)
```

**Regra principal:** Controllers NÃO conhecem Repositories. Services orquestram tudo.

---

## 📅 Timeline Sugerida

| Semana       | Foco                                          |
| ------------ | --------------------------------------------- |
| **Semana 1** | Setup + Models + Database + Repositories CRUD |
| **Semana 2** | Services + Validações + Controllers básicos   |
| **Semana 3** | Features avançadas + Documentação             |

---

## 📦 Entregáveis por Fase

---

### **FASE 1: Fundação** ⏱️ 30-45min

**O que fazer:**

- Setup de projeto e dependências
- Criar estrutura de pastas (Models, DTOs, Data, Repositories, Services, Controllers)
- Compilar sem erros

#### ✅ Checklist de Validação

**Setup Inicial:**

- [x] Projeto .NET criado (webapi template)
- [x] Pacotes NuGet instalados:
  - [x] Pomelo.EntityFrameworkCore.MySql (cmd: dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.0)
  - [x] Microsoft.EntityFrameworkCore.Design (cmd: dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0)
  - [x] Microsoft.EntityFrameworkCore.Tools (cmd: dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0)
- [x] `dotnet restore` executado com sucesso (cmd: dotnet restore)

**Estrutura de Pastas:**

- [x] Pasta `Models/` criada na raiz
- [x] Pasta `DTOs/` criada na raiz
- [x] Pasta `Data/` criada na raiz
- [x] Pasta `Repositories/` criada na raiz
- [x] Pasta `Services/` criada na raiz
- [x] Pasta `Controllers/` existe (já vem no template)

**Limpeza:**

- [x] `WeatherForecast.cs` removido
- [x] `WeatherForecastController.cs` removido

**Compilação:**

- [ ] `dotnet build` executa sem erros
- [ ] `dotnet run` inicia aplicação
- [ ] Swagger acessível em `/swagger`

---

### **FASE 2 & 3: Models + Database (Database First via Scaffold)** ⏱️ 1-2h

**⚠️ IMPORTANTE: Abordagem Database First escolhida**

Este projeto utilizou **Database First** em vez de Code First. Isso significa:

- ✅ Criamos o banco PRIMEIRO com SQL (stockRonaldSQL.sql)
- ✅ Usamos `dotnet ef dbcontext scaffold` para GERAR os Models automaticamente
- ✅ Migration apenas "registrou" que banco já existe (não criou tabelas)

**vs Code First (plano original):**

- ❌ Escrever Models manualmente em C#
- ❌ Criar migrations que criam as tabelas
- ❌ `dotnet ef database update` cria schema no MySQL

---

#### ✅ **Checklist: Criação do Banco (SQL First)**

**Script SQL:**

- [x] `Database/stockRonaldSQL.sql` criado/modificado
- [x] Script usa database `controle_estoque` (não cria novo schema)
- [x] Script tem seção de cleanup (DROP TABLE IF EXISTS)
- [x] Script cria 5 tabelas: category, product, item, movement, user (lowercase)
- [x] Colunas em snake_case (category_id, creation_date, product_id, etc)
- [x] Tipos de dado: INT AUTO_INCREMENT para PKs (não GUID/UUID)
- [x] Índice único em `product.sku`
- [x] FKs configuradas com ON DELETE/UPDATE RESTRICT

**Execução:**

- [x] Banco `controle_estoque` já existia no MySQL
- [x] Script executado: `mysql -u erikalima -perikalima < Database/stockRonaldSQL.sql`
- [x] Sem erros de permissão ou sintaxe
- [x] 5 tabelas criadas: category, product, item, movement, user

**Dados populados:**

- [x] 8 categorias (Hambúrgueres, Pães, Laticínios, Molhos, Batatas, Bebidas, Sobremesas, Embalagens)
- [x] 38 produtos com SKUs (HAM-001 a EMB-006)
- [x] 38 items (lotes) com estoque, validades e localizações
- [x] 3 usuários (Carlos Admin, Ana Operadora, Pedro Supervisor)
- [x] Movimentações de exemplo para histórico

---

#### ✅ **Checklist: Scaffold (Geração Automática dos Models)**

**Comando executado:**

```bash
dotnet ef dbcontext scaffold \
  "Server=localhost;Database=controle_estoque;User=erikalima;Password=erikalima;" \
  Pomelo.EntityFrameworkCore.MySql \
  -o Models \
  --context-dir Data \
  -f
```

**Models gerados (6 arquivos):**

- [x] `Models/Category.cs` criado automaticamente
- [x] `Models/Product.cs` criado automaticamente
- [x] `Models/Item.cs` criado automaticamente
- [x] `Models/Movement.cs` criado automaticamente
- [x] `Models/User.cs` criado automaticamente
- [x] `Models/EfmigrationsHistory.cs` criado (mapeia `__EFMigrationsHistory`)

**Características dos Models gerados:**

- [x] Classes `partial` (podem ser estendidas)
- [x] PKs tipo `int` (não Guid)
- [x] Properties PascalCase (C#) mapeadas para snake_case (MySQL)
- [x] Navigation properties configuradas (Category.Products, Product.Items, etc)
- [x] `virtual` nas navigation properties para lazy loading
- [x] Auto-property initializers (`= new HashSet<>()` para coleções)
- [x] Nullable types onde aplicável (Status?, MinimumQuantity?, etc)

---

#### ✅ **Checklist: AppDbContext**

**Arquivo gerado e modificado:**

- [x] `Data/ControleEstoqueContext.cs` gerado pelo scaffold
- [x] Renomeado manualmente para `Data/AppDbContext.cs`
- [x] Classe renomeada de `ControleEstoqueContext` para `AppDbContext`

**DbSets configurados:**

- [x] `DbSet<Category> Categories`
- [x] `DbSet<Product> Products`
- [x] `DbSet<Item> Items`
- [x] `DbSet<Movement> Movements`
- [x] `DbSet<User> Users`
- [x] `DbSet<EfmigrationsHistory> EfmigrationsHistories`

**OnModelCreating (gerado pelo scaffold):**

- [x] `ToTable()` para nomes lowercase (category, product, item, movement, user)
- [x] `HasColumnName()` mapeando PascalCase → snake_case
  - CategoryId → category_id
  - CreationDate → creation_date
  - MinimumQuantity → minimum_quantity
  - etc
- [x] `HasCharSet("utf8mb4")` e `UseCollation("utf8mb4_0900_ai_ci")`
- [x] `HasKey()` para PKs
- [x] `HasOne().WithMany()` para FKs e navigation properties
- [x] `HasForeignKey()` mapeando relacionamentos

**Modificações manuais no AppDbContext:**

- [x] Removido hardcoded connection string do `OnConfiguring`
- [x] Connection string vem agora de DI via `Program.cs`

---

#### ✅ **Checklist: Detalhes dos Models**

**Category.cs:**

- [x] CategoryId (int, PK, auto-increment)
- [x] Name (string) - não "Nome" (inglês)
- [x] Description (string?)
- [x] CreationDate (DateTime?)
- [x] Navigation: `ICollection<Product> Products`

**Product.cs:**

- [x] ProductId (int, PK)
- [x] Sku (string)
- [x] Name (string) - não "Nome"
- [x] Status (sbyte? - tinyint do MySQL)
- [x] MinimumQuantity (int?)
- [x] CategoryId (int, FK)
- [x] CreationDate (DateTime?)
- [x] Navigations: `Category Category`, `ICollection<Item> Items`

**Item.cs:**

- [x] ItemId (int, PK)
- [x] Batch (string?)
- [x] ExpirationDate (DateTime?) - não "DataValidade"
- [x] Quantity (int)
- [x] Location (string?) - não "Localizacao"
- [x] Status (sbyte?)
- [x] ProductId (int, FK)
- [x] Navigations: `Product Product`, `ICollection<Movement> Movements`

**Movement.cs:**

- [x] MovementId (int, PK)
- [x] CreatedDate (DateTime? - propriedade `Date` no código)
- [x] Type (sbyte?) - não string "Tipo"
- [x] QuantityMoved (int?)
- [x] PreviousQuantity (int?)
- [x] NewQuantity (int?)
- [x] ItemId (int?, FK)
- [x] UserId (int?, FK)
- [x] Navigations: `Item Item`, `User User`

**User.cs:**

- [x] IdUser (int, PK) - não "Id"
- [x] Name (string?)
- [x] Email (string?)
- [x] Profile (string?) - não "Perfil"
- [x] Navigation: `ICollection<Movement> Movements`

---

#### ✅ **Checklist: Migration de Sincronização**

**Problema:**

- Banco já existe (criado pelo SQL)
- EF Core precisa de migration registrada para tracking

**Solução:**

- [x] Migration criada: `dotnet ef migrations add InitialDatabaseFirst`
- [x] Migration NÃO faz CREATE TABLE (Up está vazio)
- [x] Migration apenas serve para EF saber "estado atual"
- [x] Registro manual na tabela `__EFMigrationsHistory`:
  ```sql
  DELETE FROM __EFMigrationsHistory;
  INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
  VALUES ('20260206012322_InitialDatabaseFirst', '8.0.0');
  ```
- [x] `dotnet ef migrations list` mostra migration como aplicada

---

#### ✅ **Checklist: Validação Final Database First**

**Compilação:**

- [x] `dotnet build` compila sem erros
- [x] Sem warnings sobre nullable reference types (Models usam `?` corretamente)
- [x] Namespace correto: `controle_estoque_cshap.Models`

**Banco de dados:**

- [x] 5 tabelas: category, product, item, movement, user (lowercase)
- [x] Colunas: snake_case (category_id, creation_date, etc)
- [x] PKs: INT AUTO_INCREMENT
- [x] FKs funcionando
- [x] Índice único em `product.sku`
- [x] Dados: 8 categorias, 38 produtos, 38 items, 3 users

**EF Core Mapping:**

- [x] EF consegue consultar: `db.Categories.ToList()` funciona
- [x] Navigation properties carregam com `.Include()`:
  ```csharp
  db.Products.Include(p => p.Category).FirstOrDefault()
  ```
- [x] JOIN funciona corretamente
- [x] Contagens corretas:
  - Categories.Count() = 8
  - Products.Count() = 38
  - Users.Count() = 3

**Teste de integração:**

- [x] Endpoint `/api/test-db` criado para validação
- [x] Teste realizado e passou:
  ```bash
  curl http://localhost:5000/api/test-db
  # Retornou: totalCategories=8, totalProducts=38, totalUsers=3
  # Sample product com Category.Name carregado via JOIN
  ```

---

#### ✅ **Checklist Final Fases 2 & 3**

**O que FOI feito (Database First):**

- [x] Banco criado via SQL script (stockRonaldSQL.sql)
- [x] 5 tabelas (snake_case, INT PKs, FKs configuradas)
- [x] Dados populados (38 produtos, 8 categorias, 3 users, items, movements)
- [x] Models gerados automaticamente via scaffold (6 arquivos)
- [x] AppDbContext gerado e renomeado
- [x] Fluent API completa configurada pelo scaffold
- [x] Migration de sincronização criada e registrada
- [x] EF Core ↔ MySQL funcionando (validado com teste)

**O que NÃO FOI feito (não aplicável em Database First):**

- ❌ Escrever Models manualmente linha por linha
- ❌ Criar construtores que inicializam Guid/DataCriacao
- ❌ Escrever métodos de negócio nos Models (Ativar, Desativar, etc)
- ❌ Migrations que criam CREATE TABLE statements
- ❌ `dotnet ef database update` gerando schema

**Próximos passos:**

- DTOs precisarão adaptar-se aos Models reais:
  - `int` em vez de `Guid`
  - `Name` em vez de `Nome`
  - `IdUser` em vez de `Id` (User)
  - `sbyte?` para Status em vez de enum
- Repositories usarão `int` como tipo de ID
- Services farão validações que originalmente estariam nos Models

---

### **FASE 4: DTOs** ⏱️ 1-2h

**Criar DTOs para transferência de dados**

#### ✅ Checklist: Category DTOs

**Arquivos criados:**

- [x] `DTOs/CategoryDto/CategoryDto.cs` (leitura)
- [x] `DTOs/CategoryDto/CategoryCreateDto.cs` (criação)
- [x] `DTOs/CategoryDto/CategoryUpdateDto.cs` (atualização)

**CategoryDto:**

- [x] CategoryId (int)
- [x] Name (string)
- [x] Description (string?)
- [x] CreationDate (DateTime?)
- [x] TotalProducts (int)
- [x] Apenas propriedades com get/set, sem lógica

**CreateCategoryDto:**

- [x] Name (string)
- [x] Description (string?)
- [x] Nao contem CategoryId ou CreationDate

**UpdateCategoryDto:**

- [x] Name (string?)
- [x] Description (string?)

---

#### ✅ Checklist: Product DTOs

**Arquivos criados:**

- [ ] `DTOs/ProductDto.cs`
- [ ] `DTOs/CreateProductDto.cs`
- [ ] `DTOs/UpdateProductDto.cs`

**ProductDto:**

- [ ] Id, SKU, Nome, Status, QuantidadeMinima, DataCriacao
- [ ] CategoryId, CategoryNome (denormalizado para facilitar UI)
- [ ] QuantidadeTotal (calculado pela soma dos Items)

**CreateProductDto:**

- [ ] SKU, Nome, QuantidadeMinima, CategoryId
- [ ] NÃO contém Id, Status (inicia sempre Ativo)

**UpdateProductDto:**

- [ ] Nome, QuantidadeMinima, CategoryId
- [ ] NÃO permite alterar SKU (imutável)

---

#### ✅ Checklist: Item DTOs

**Arquivos criados:**

- [ ] `DTOs/ItemDto.cs`
- [ ] `DTOs/CreateItemDto.cs`
- [ ] `DTOs/UpdateItemDto.cs`

**ItemDto:**

- [ ] Id, Batch, DataValidade, Quantidade, Localizacao, Status
- [ ] ProductId, ProductNome (denormalizado)
- [ ] DataCriacao

**CreateItemDto:**

- [ ] Batch, DataValidade, Quantidade, Localizacao, ProductId
- [ ] NÃO contém Status (calculado automaticamente)

**UpdateItemDto:**

- [ ] Batch, DataValidade, Localizacao
- [ ] NÃO permite atualizar Quantidade diretamente (usar endpoints específicos)

---

#### ✅ Checklist: Movement DTOs

**Arquivos criados:**

- [ ] `DTOs/MovementDto.cs` (apenas leitura)

**MovementDto:**

- [ ] Id, Data, Tipo, QuantidadeMovimentada
- [ ] QuantidadeAnterior, QuantidadeNova
- [ ] ItemId, ItemBatch (denormalizado)
- [ ] UserId, UserNome (denormalizado)

**Observação:**

- [ ] NÃO existe CreateMovementDto (criado automaticamente pelo sistema)
- [ ] NÃO existe UpdateMovementDto (imutável)

---

#### ✅ Checklist: User DTOs

**Arquivos criados:**

- [ ] `DTOs/UserDto.cs`
- [ ] `DTOs/CreateUserDto.cs`
- [ ] `DTOs/UpdateUserDto.cs`

**UserDto:**

- [ ] Id, Nome, Email, Perfil

**CreateUserDto:**

- [ ] Nome, Email, Perfil

**UpdateUserDto:**

- [ ] Nome, Email, Perfil

---

#### ✅ Checklist Final da Fase 4

**Validações:**

- [ ] Todos os DTOs compilam sem erros
- [ ] Nenhum DTO contém métodos ou lógica de negócio
- [ ] DTOs de criação não contêm campos gerados (Id, DataCriacao)
- [ ] DTOs de leitura contêm campos denormalizados quando necessário
- [ ] Propriedades públicas com get/set em todos os DTOs

---

### **FASE 5: Repositories (CRUD genérico + queries específicas)** ⏱️ 3-4h

#### ✅ Checklist: IRepository<T> Genérico

**Arquivo:**

- [ ] `Repositories/IRepository.cs` criado

**Métodos obrigatórios:**

- [ ] `Task<T?> ObterPorIdAsync(Guid id)`
- [ ] `Task<IEnumerable<T>> ObterTodosAsync()`
- [ ] `Task AdicionarAsync(T entity)`
- [ ] `Task AtualizarAsync(T entity)`
- [ ] `Task<bool> ExisteAsync(Guid id)`

**Validações:**

- [ ] Interface genérica com constraint `where T : class`
- [ ] Todos os métodos retornam Task (assíncronos)
- [ ] Compila sem erros

---

#### ✅ Checklist: ICategoryRepository

**Arquivo:**

- [x] `Repositories/CategoryRepository/ICategoryRepository.cs` criado

**Configuração:**

- [ ] Herda de `IRepository<Category>`
- [x] Inclui métodos específicos da Category (consultas por id/nome)

**Implementação:**

- [x] `Repositories/CategoryRepository/CategoryRepository.cs` criado
- [x] Implementa `ICategoryRepository`
- [x] Recebe `AppDbContext` via construtor
- [x] Todos os métodos implementados usando EF Core
- [x] Compila sem erros

---

#### ✅ Checklist: IProductRepository

**Arquivo:**

- [ ] `Repositories/IProductRepository.cs` criado

**Métodos herdados:**

- [ ] Herda `IRepository<Product>`

**Métodos adicionais:**

- [ ] `Task<Product?> BuscarPorSkuAsync(string sku)`
- [ ] `Task<IEnumerable<Product>> ObterAtivosAsync()`
- [ ] `Task<IEnumerable<Product>> ObterInativosAsync()`
- [ ] `Task<IEnumerable<Product>> ObterEstoqueBaixoAsync()` - produtos onde soma dos Items < QuantidadeMinima

**Implementação:**

- [ ] `Repositories/ProductRepository.cs` criado
- [ ] Implementa todos os métodos
- [ ] `ObterEstoqueBaixoAsync` usa JOIN/Include com Items
- [ ] Queries otimizadas (AsNoTracking quando leitura)
- [ ] Compila sem erros

---

#### ✅ Checklist: IItemRepository

**Arquivo:**

- [ ] `Repositories/IItemRepository.cs` criado

**Métodos adicionais:**

- [ ] `Task<IEnumerable<Item>> BuscarPorProductAsync(Guid productId)`
- [ ] `Task<IEnumerable<Item>> BuscarVencendoAsync(int dias)` - DataValidade <= DateTime.UtcNow.AddDays(dias)
- [ ] `Task<IEnumerable<Item>> BuscarPorStatusAsync(string status)`

**Implementação:**

- [ ] `Repositories/ItemRepository.cs` criado
- [ ] Todos os métodos implementados
- [ ] `BuscarPorProductAsync` inclui Product (eager loading)
- [ ] `BuscarVencendoAsync` filtra apenas itens com DataValidade não-nula
- [ ] Compila sem erros

---

#### ✅ Checklist: IMovementRepository

**Arquivo:**

- [ ] `Repositories/IMovementRepository.cs` criado

**Métodos adicionais:**

- [ ] `Task<IEnumerable<Movement>> BuscarPorItemAsync(Guid itemId)`
- [ ] `Task<IEnumerable<Movement>> BuscarPorPeriodoAsync(DateTime inicio, DateTime fim)`

**Implementação:**

- [ ] `Repositories/MovementRepository.cs` criado
- [ ] Queries incluem Item e User (eager loading)
- [ ] Ordenação por Data descendente
- [ ] Compila sem erros

---

#### ✅ Checklist: IUserRepository

**Arquivo:**

- [ ] `Repositories/IUserRepository.cs` criado

**Métodos adicionais:**

- [ ] `Task<User?> BuscarPorEmailAsync(string email)`

**Implementação:**

- [ ] `Repositories/UserRepository.cs` criado
- [ ] Compila sem erros

---

#### ✅ Checklist Final da Fase 5

**Validações gerais:**

- [ ] Todos os 5 repositórios compilam
- [ ] Todos usam async/await
- [ ] DbContext injetado via construtor em todos
- [ ] Queries retornam tipos corretos
- [ ] Eager loading usado onde necessário (Include)
- [ ] Nenhum repository contém validação de negócio

---

### **FASE 6: Services (Lógica de negócio)** ⏱️ 4-5h

#### ✅ Checklist: ICategoryService

**Arquivo:**

- [x] `Services/CategoryService/ICategoryService.cs` criado

**Métodos:**

- [x] `Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()`
- [x] `Task<CategoryDto?> GetCategoryByIdAsync(int id)`
- [x] `Task<CategoryDto?> CreateCategoryAsync(CategoryCreateDto dto)`
- [x] `Task<CategoryUpdateResult> UpdateCategoryAsync(int id, CategoryUpdateDto dto)`

**Implementação:**

- [x] `Services/CategoryService/CategoryService.cs` criado
- [x] Recebe `ICategoryRepository` via construtor
- [ ] Recebe `ILogger<CategoryService>` via construtor

**Validações no CriarAsync:**

- [ ] Nome não pode ser vazio → ArgumentException
- [ ] Nome máximo 255 caracteres → ArgumentException

**Validações no AtualizarAsync:**

- [ ] Categoria deve existir → KeyNotFoundException
- [ ] Mesmas validações de CriarAsync

**Mapeamentos:**

- [x] Converte Category → CategoryDto em todos os retornos
- [x] Converte DTOs → Category ao criar/atualizar

**Logging:**

- [ ] Log ao criar categoria (nome)
- [ ] Log ao atualizar categoria
- [ ] Log de erro em exceções

**Checklist final:**

- [ ] Service compila sem erros
- [ ] Todas as validações implementadas
- [ ] Mapeamentos funcionam

---

#### ✅ Checklist: IProductService

**Arquivo:**

- [ ] `Services/IProductService.cs` criado

**Métodos:**

- [ ] `Task<IEnumerable<ProductDto>> ObterTodosAsync()`
- [ ] `Task<IEnumerable<ProductDto>> ObterInativosAsync()`
- [ ] `Task<ProductDto?> ObterPorIdAsync(Guid id)`
- [ ] `Task<ProductDto?> ObterPorSkuAsync(string sku)`
- [ ] `Task<IEnumerable<ProductDto>> ObterEstoqueBaixoAsync()`
- [ ] `Task<CategoryDto> CriarAsync(CreateProductDto dto)`
- [ ] `Task<ProductDto> AtualizarAsync(Guid id, UpdateProductDto dto)`
- [ ] `Task DesativarAsync(Guid id)`

**Implementação:**

- [ ] `Services/ProductService.cs` criado
- [ ] Recebe `IProductRepository` e `IItemRepository` via construtor
- [ ] Recebe `ILogger<ProductService>`

**Validações no CriarAsync:**

- [ ] SKU não pode ser vazio → ArgumentException
- [ ] SKU deve ser único (usar repository) → ArgumentException "SKU já existe"
- [ ] Nome não pode ser vazio → ArgumentException
- [ ] QuantidadeMinima >= 0 → ArgumentException
- [ ] CategoryId deve existir → ArgumentException

**Validações no AtualizarAsync:**

- [ ] Produto deve existir → KeyNotFoundException
- [ ] Produto deve estar Ativo → InvalidOperationException "Produto inativo não pode ser alterado"
- [ ] Mesmas validações de campos do CriarAsync

**Regras do DesativarAsync:**

- [ ] Produto deve existir → KeyNotFoundException
- [ ] Chama método Desativar() da entidade
- [ ] Atualiza no repository

**Cálculo de QuantidadeTotal:**

- [ ] Usa IItemRepository para buscar todos os items do produto
- [ ] Soma as quantidades
- [ ] Retorna no ProductDto

**Checklist final:**

- [ ] Todas as validações implementadas
- [ ] Produto inativo não pode ser atualizado
- [ ] Soft delete funciona (DesativarAsync)
- [ ] QuantidadeTotal calculada corretamente
- [ ] Compila sem erros

---

#### ✅ Checklist: IItemService

**Arquivo:**

- [ ] `Services/IItemService.cs` criado

**Métodos:**

- [ ] `Task<IEnumerable<ItemDto>> ObterPorProductAsync(Guid productId)`
- [ ] `Task<ItemDto?> ObterPorIdAsync(Guid id)`
- [ ] `Task<IEnumerable<ItemDto>> ObterVencendoAsync(int dias)`
- [ ] `Task<ItemDto> CriarAsync(CreateItemDto dto)`
- [ ] `Task<ItemDto> AtualizarAsync(Guid id, UpdateItemDto dto)`
- [ ] `Task AdicionarQuantidadeAsync(Guid id, int quantidade, Guid userId)`
- [ ] `Task RemoverQuantidadeAsync(Guid id, int quantidade, Guid userId)`

**Implementação:**

- [ ] `Services/ItemService.cs` criado
- [ ] Recebe `IItemRepository`, `IMovementRepository`, `IProductRepository`
- [ ] Recebe `ILogger<ItemService>`

**Validações no CriarAsync:**

- [ ] Batch não pode ser vazio → ArgumentException
- [ ] Quantidade >= 0 → ArgumentException
- [ ] Localizacao não pode ser vazia → ArgumentException
- [ ] DataValidade, se informada, deve ser futura → ArgumentException
- [ ] ProductId deve existir → ArgumentException

**Validações no AtualizarAsync:**

- [ ] Item deve existir → KeyNotFoundException
- [ ] Validações de campos do CriarAsync

**Lógica do AdicionarQuantidadeAsync:**

- [ ] Item deve existir → KeyNotFoundException
- [ ] Quantidade deve ser > 0 → ArgumentException
- [ ] Chama Item.AdicionarQuantidade()
- [ ] Chama Item.AtualizarStatus()
- [ ] **CRIA registro Movement** com tipo "Entrada"
- [ ] Movement guarda: QuantidadeAnterior, QuantidadeNova, UserId
- [ ] Usa transação (SaveChanges salva Item + Movement juntos)

**Lógica do RemoverQuantidadeAsync:**

- [ ] Item deve existir → KeyNotFoundException
- [ ] Quantidade deve ser > 0 → ArgumentException
- [ ] Chama Item.RemoverQuantidade() (pode lançar exceção se insuficiente)
- [ ] Chama Item.AtualizarStatus()
- [ ] **CRIA registro Movement** com tipo "Saida"
- [ ] Usa transação

**Checklist final:**

- [ ] Validações implementadas
- [ ] AdicionarQuantidade cria Movement
- [ ] RemoverQuantidade cria Movement
- [ ] Status atualizado automaticamente
- [ ] Transações garantem consistência
- [ ] Compila sem erros

---

#### ✅ Checklist: IMovementService

**Arquivo:**

- [ ] `Services/IMovementService.cs` criado

**Métodos (apenas leitura):**

- [ ] `Task<IEnumerable<MovementDto>> ObterPorItemAsync(Guid itemId)`
- [ ] `Task<IEnumerable<MovementDto>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim)`

**Implementação:**

- [ ] `Services/MovementService.cs` criado
- [ ] Recebe `IMovementRepository`
- [ ] **NÃO tem métodos de criar/atualizar/deletar**

**Regras:**

- [ ] Movements são criados apenas via ItemService
- [ ] Ordenação por Data descendente
- [ ] Inclui informações denormalizadas (ItemBatch, UserNome)

**Checklist final:**

- [ ] Apenas leitura
- [ ] Compila sem erros

---

#### ✅ Checklist: IUserService

**Arquivo:**

- [ ] `Services/IUserService.cs` criado

**Métodos básicos:**

- [ ] `Task<IEnumerable<UserDto>> ObterTodosAsync()`
- [ ] `Task<UserDto?> ObterPorIdAsync(Guid id)`
- [ ] `Task<UserDto> CriarAsync(CreateUserDto dto)`

**Implementação:**

- [ ] `Services/UserService.cs` criado
- [ ] Validação básica (email, nome)
- [ ] Pode ser implementação mínima por enquanto

---

#### ✅ Checklist Final da Fase 6

**Validações gerais:**

- [ ] Todos os 5 services compilam
- [ ] Validações de negócio nos services, não nos controllers
- [ ] Exceptions apropriadas lançadas (ArgumentException, KeyNotFoundException, InvalidOperationException)
- [ ] Logging implementado nos pontos críticos
- [ ] Mapeamentos DTO ↔ Model funcionam
- [ ] ItemService cria Movement automaticamente
- [ ] Transações usadas onde necessário

---

### **FASE 7: Controllers (Endpoints REST)** ⏱️ 3-4h

#### ✅ Checklist: CategoryController

**Arquivo:**

- [x] `Controllers/CategoryController.cs` criado

**Configuração:**

- [x] Atributo `[ApiController]`
- [x] Atributo `[Route("api/categories")]`
- [x] Herda de `ControllerBase`
- [x] Recebe `ICategoryService` via construtor

**Endpoints:**

- [x] `GET /api/categories` - ObterTodos
  - [x] Retorna 200 OK com lista
- [x] `GET /api/categories/{id}` - ObterPorId
  - [x] Retorna 200 OK se encontrado
  - [x] Retorna 404 Not Found se não existir
- [x] `POST /api/categories` - Criar
  - [x] Recebe `[FromBody] CategoryCreateDto`
  - [x] Retorna 201 Created com CreatedAtAction
  - [x] Retorna 409 Conflict se nome ja existir
- [x] `PUT /api/categories/{id}` - Atualizar
  - [x] Recebe id e `[FromBody] CategoryUpdateDto`
  - [x] Retorna 204 No Content
  - [x] Retorna 404 Not Found se nao existir
  - [x] Retorna 409 Conflict se nome ja existir

**Tratamento de erros:**

- [x] Try-catch em todos os endpoints
- [x] Mensagens descritivas em objetos JSON
- [x] Status codes corretos

**Documentação:**

- [x] Comentários XML (///) em todos os métodos

---

#### ✅ Checklist: ProductController

**Arquivo:**

- [ ] `Controllers/ProductController.cs` criado

**Endpoints:**

- [ ] `GET /api/products` - ObterTodos (ativos)
- [ ] `GET /api/products/inactive` - ObterInativos
- [ ] `GET /api/products/{id}` - ObterPorId
- [ ] `GET /api/products/sku/{sku}` - ObterPorSku
- [ ] `GET /api/products/low-stock` - ObterEstoqueBaixo
- [ ] `POST /api/products` - Criar
  - [ ] Valida SKU único
  - [ ] Retorna 201 Created
- [ ] `PUT /api/products/{id}` - Atualizar
  - [ ] Impede atualização de produto inativo
  - [ ] Retorna 400 se inativo
- [ ] `DELETE /api/products/{id}` - Desativar (soft delete)
  - [ ] Retorna 204 No Content

**Tratamento de erros:**

- [ ] ArgumentException "SKU já existe" → 400
- [ ] InvalidOperationException "Produto inativo" → 400
- [ ] KeyNotFoundException → 404

**Checklist:**

- [ ] Todos os 8 endpoints implementados
- [ ] Comentários XML completos
- [ ] Compila sem erros

---

#### ✅ Checklist: ItemController

**Arquivo:**

- [ ] `Controllers/ItemController.cs` criado

**Endpoints:**

- [ ] `GET /api/products/{productId}/items` - ObterPorProduct
- [ ] `GET /api/items/{id}` - ObterPorId
- [ ] `GET /api/items/expiring?days=7` - ObterVencendo
  - [ ] Parâmetro query `days` (padrão 7)
- [ ] `POST /api/products/{productId}/items` - Criar
  - [ ] Valida DataValidade futura
  - [ ] Retorna 201 Created
- [ ] `PUT /api/items/{id}` - Atualizar
  - [ ] NÃO permite atualizar Quantidade (usar endpoints específicos)
- [ ] `POST /api/items/{id}/add-quantity` - AdicionarQuantidade
  - [ ] Recebe `{ quantidade: int, userId: guid }` no body
  - [ ] Cria Movement automaticamente
  - [ ] Retorna 200 OK
- [ ] `POST /api/items/{id}/remove-quantity` - RemoverQuantidade
  - [ ] Recebe `{ quantidade: int, userId: guid }` no body
  - [ ] Valida estoque suficiente
  - [ ] Cria Movement automaticamente
  - [ ] Retorna 200 OK

**Tratamento de erros:**

- [ ] InvalidOperationException "Estoque insuficiente" → 400
- [ ] ArgumentException "DataValidade inválida" → 400
- [ ] KeyNotFoundException → 404

**Checklist:**

- [ ] Todos os 7 endpoints implementados
- [ ] add-quantity e remove-quantity criam Movement
- [ ] Comentários XML completos

---

#### ✅ Checklist: MovementController

**Arquivo:**

- [ ] `Controllers/MovementController.cs` criado

**Endpoints (apenas leitura):**

- [ ] `GET /api/items/{itemId}/movements` - ObterPorItem
  - [ ] Retorna histórico ordenado por data DESC
- [ ] `GET /api/movements?startDate=X&endDate=Y` - ObterPorPeriodo
  - [ ] Valida que startDate < endDate
  - [ ] Retorna 400 se datas inválidas

**Checklist:**

- [ ] NÃO tem POST/PUT/DELETE (histórico é imutável)
- [ ] Comentários XML completos
- [ ] Compila sem erros

---

#### ✅ Checklist: UserController

**Arquivo:**

- [ ] `Controllers/UserController.cs` criado (opcional se não for foco)

**Endpoints básicos:**

- [ ] `GET /api/users`
- [ ] `GET /api/users/{id}`
- [ ] `POST /api/users`

---

#### ✅ Checklist Final da Fase 7

**Validações gerais:**

- [ ] Todos os controllers compilam
- [ ] Atributos de rota corretos
- [ ] Status codes corretos (200, 201, 204, 400, 404)
- [ ] Tratamento de exceções em todos os endpoints
- [ ] CreatedAtAction usado em POST
- [ ] Mensagens de erro descritivas em JSON
- [ ] Comentários XML completos
- [ ] Controllers NÃO acessam Repositories diretamente

---

### **FASE 8: Configuração (Program.cs)** ⏱️ 30min-1h

#### ✅ Checklist: Configuração de Serviços

**DbContext:**

- [x] `AddDbContext<AppDbContext>` configurado
- [x] Connection string definida
- [x] Provider correto (UseSqlite, UseSqlServer, etc)

**Repositories:**

- [x] `AddScoped<ICategoryRepository, CategoryRepository>`
- [x] `AddScoped<IProductRepository, ProductRepository>`
- [ ] `AddScoped<IItemRepository, ItemRepository>`
- [ ] `AddScoped<IMovementRepository, MovementRepository>`
- [ ] `AddScoped<IUserRepository, UserRepository>`

**Services:**

- [x] `AddScoped<ICategoryService, CategoryService>`
- [x] `AddScoped<IProductService, ProductService>`
- [ ] `AddScoped<IItemService, ItemService>`
- [ ] `AddScoped<IMovementService, MovementService>`
- [ ] `AddScoped<IUserService, UserService>`

**Controllers e Swagger:**

- [x] `AddControllers()` adicionado
- [x] `AddEndpointsApiExplorer()` adicionado
- [x] `AddSwaggerGen()` configurado com informações do projeto

**Pipeline:**

- [x] `UseSwagger()` em Development
- [x] `UseSwaggerUI()` em Development
- [x] `UseHttpsRedirection()` adicionado
- [x] `UseAuthorization()` adicionado
- [x] **`MapControllers()`** adicionado (CRÍTICO)

---

#### ✅ Checklist Final da Fase 8

**Validações:**

- [ ] `dotnet build` compila sem erros
- [ ] `dotnet run` inicia aplicação
- [ ] Swagger acessível em `/swagger`
- [ ] Todos os endpoints visíveis no Swagger
- [ ] Sem erros no console ao iniciar

---

### **FASE 9: Testes e Validações** ⏱️ 3-4h

#### ✅ Checklist: Testes de Sucesso (Happy Path)

**Categoria:**

- [ ] POST /api/categories - Criar categoria
  - [ ] Retorna 201 Created
  - [ ] Id gerado
  - [ ] DataCriacao preenchida
- [ ] GET /api/categories - Listar
  - [ ] Categoria criada aparece na lista
- [ ] PUT /api/categories/{id} - Atualizar
  - [ ] Retorna 200 OK
  - [ ] Dados atualizados

**Produto:**

- [ ] POST /api/products - Criar produto
  - [ ] Com CategoryId válido
  - [ ] SKU único
  - [ ] Retorna 201
  - [ ] Status = Ativo
- [ ] GET /api/products - Listar ativos
  - [ ] Produto criado aparece
- [ ] GET /api/products/sku/{sku} - Buscar por SKU
  - [ ] Retorna produto correto
- [ ] DELETE /api/products/{id} - Desativar
  - [ ] Retorna 204
  - [ ] Produto NÃO aparece mais em GET /api/products
  - [ ] Produto APARECE em GET /api/products/inactive

**Item (Lote):**

- [ ] POST /api/products/{productId}/items - Criar lote
  - [ ] Com DataValidade futura
  - [ ] Retorna 201
  - [ ] Status calculado automaticamente
- [ ] GET /api/products/{productId}/items - Listar lotes do produto
  - [ ] Lote criado aparece
- [ ] POST /api/items/{id}/add-quantity - Adicionar estoque
  - [ ] Quantidade aumenta
  - [ ] Retorna 200
- [ ] GET /api/items/{itemId}/movements - Ver histórico
  - [ ] **Movement de "Entrada" foi criado**
  - [ ] QuantidadeAnterior e QuantidadeNova corretos
  - [ ] UserId registrado
- [ ] POST /api/items/{id}/remove-quantity - Remover estoque
  - [ ] Quantidade diminui
  - [ ] **Movement de "Saida" foi criado**

**Movimentação:**

- [ ] GET /api/items/{itemId}/movements - Histórico do lote
  - [ ] Retorna todas as movimentações
  - [ ] Ordenado por data DESC
- [ ] GET /api/movements?startDate=X&endDate=Y - Por período
  - [ ] Retorna movimentações no período

---

#### ✅ Checklist: Testes de Erro

**Categoria:**

- [ ] POST com Nome vazio → 400 Bad Request
- [ ] PUT de categoria inexistente → 404 Not Found

**Produto:**

- [ ] POST com SKU duplicado → 400 "SKU já existe"
- [ ] POST com CategoryId inexistente → 400
- [ ] PUT de produto inativo → 400 "Produto inativo não pode ser alterado"
- [ ] GET /api/products/sku/{sku-inexistente} → 404

**Item:**

- [ ] POST com DataValidade no passado → 400
- [ ] POST com ProductId inexistente → 404
- [ ] POST /api/items/{id}/remove-quantity com quantidade maior que estoque → 400 "Estoque insuficiente"
- [ ] POST add-quantity com quantidade negativa → 400

**Movement:**

- [ ] GET com startDate > endDate → 400

---

#### ✅ Checklist: Validações de Banco de Dados

**Schema:**

- [ ] 5 tabelas criadas (Categories, Products, Items, Movements, Users)
- [ ] Índice único em Products.SKU existe
- [ ] FKs configuradas corretamente

**Dados:**

- [ ] Produto desativado tem Status = Inativo (0) no banco
- [ ] Produto desativado NÃO é deletado fisicamente
- [ ] Movement registra UserId de quem fez a operação
- [ ] Movement tem Data preenchida automaticamente

**Integridade:**

- [ ] Não consigo inserir Item com ProductId inexistente (FK constraint)
- [ ] Não consigo inserir 2 produtos com mesmo SKU (unique constraint)

---

#### ✅ Checklist: Validações de Negócio

**Product vs Item:**

- [ ] QuantidadeTotal do produto = soma das quantidades de todos os Items
- [ ] Produto pode ter múltiplos Items (lotes)
- [ ] Cada Item representa um lote com validade específica

**Status:**

- [ ] Item com Quantidade = 0 tem Status = "Esgotado"
- [ ] Item com Quantidade > 0 e <= Product.QuantidadeMinima tem Status = "Alerta"
- [ ] Item com Quantidade > Product.QuantidadeMinima tem Status = "Disponivel"

**Histórico:**

- [ ] TODA adição de estoque cria Movement tipo "Entrada"
- [ ] TODA remoção de estoque cria Movement tipo "Saida"
- [ ] Movement registra QuantidadeAnterior e QuantidadeNova
- [ ] Movement NÃO pode ser editado/deletado

**Data de Validade:**

- [ ] GET /api/items/expiring?days=7 retorna itens vencendo em até 7 dias
- [ ] Não retorna itens sem DataValidade
- [ ] Não retorna itens já vencidos (DataValidade < hoje)

---

#### ✅ Checklist Final da Fase 9

**Cobertura de testes:**

- [ ] TODOS os cenários de sucesso testados e funcionando
- [ ] TODOS os cenários de erro retornam status code correto
- [ ] TODAS as validações de banco confirmadas
- [ ] TODAS as regras de negócio validadas
- [ ] Documentado em arquivo TESTES.md (opcional)

---

### **FASE 10: Documentação** ⏱️ 2-3h

#### ✅ Checklist: README.md

**Seção: Descrição do Projeto:**

- [ ] Objetivo do sistema
- [ ] Contexto (lanchonete, controle de estoque alimentício)
- [ ] Diferencial (lotes, validade, rastreabilidade)

**Seção: Tecnologias:**

- [ ] .NET 8 (ou versão usada)
- [ ] Entity Framework Core
- [ ] Provider de banco (SQLite/SQL Server)
- [ ] Swagger/OpenAPI

**Seção: Arquitetura:**

- [ ] Padrão MVC/MSC
- [ ] Diagrama de camadas (texto ou ASCII art)
- [ ] Separação de responsabilidades

**Seção: Modelo de Dados:**

- [ ] Explicação das 5 entidades
- [ ] Diferença entre Product (tipo) e Item (lote)
- [ ] Relacionamentos

**Seção: Pré-requisitos:**

- [ ] .NET SDK (versão mínima)
- [ ] Ferramentas opcionais (VS Code, Rider, etc)

**Seção: Como Executar:**

- [ ] Clone do repositório
- [ ] `dotnet restore`
- [ ] `dotnet ef database update`
- [ ] `dotnet run`
- [ ] URL do Swagger
- [ ] Comandos funcionam quando seguidos passo a passo

**Seção: Endpoints Principais:**

- [ ] Tabela com método, rota, descrição
- [ ] Pelo menos 1 exemplo de request/response

**Seção: Regras de Negócio:**

- [ ] SKU único e imutável
- [ ] Soft delete de produtos
- [ ] Histórico imutável
- [ ] Status automático de itens
- [ ] Product vs Item explicado

**Seção: Próximos Passos (Opcional):**

- [ ] Features futuras
- [ ] Melhorias planejadas

---

#### ✅ Checklist: Documentação no Swagger

**Configuração:**

- [ ] Título e versão definidos em AddSwaggerGen
- [ ] Descrição do projeto
- [ ] Informações de contato (opcional)

**Endpoints:**

- [ ] Todos os 25+ endpoints visíveis
- [ ] Agrupados por controller (Categories, Products, Items, Movements)
- [ ] Comentários XML aparecem nas descrições
- [ ] Exemplos de DTOs visíveis

**Schemas:**

- [ ] Todos os DTOs documentados
- [ ] Propriedades com descrição (se adicionou comentários XML)

---

#### ✅ Checklist: ARQUITETURA.md (Opcional)

**Diagrama de Fluxo:**

- [ ] Request HTTP → Controller → Service → Repository → Database
- [ ] Response: Database → Repository → Service → Controller → HTTP

**Decisões Técnicas:**

- [ ] Por que MVC/MSC
- [ ] Por que separar Product e Item
- [ ] Por que Movement é imutável
- [ ] Por que soft delete

**Padrões Aplicados:**

- [ ] Repository Pattern
- [ ] Service Layer
- [ ] DTO Pattern
- [ ] Dependency Injection

---

#### ✅ Checklist Final da Fase 10

**Validação:**

- [ ] README.md existe e está completo
- [ ] Terceiro consegue clonar e executar seguindo README
- [ ] Swagger documenta todos os endpoints
- [ ] Comentários XML nos controllers
- [ ] Projeto apresentável para portfolio

---

## 📋 CRITÉRIOS DE ACEITE FINAL DO PROJETO

### ✅ Funcionalidades Obrigatórias

- [ ] **CRUD de Categorias** funcionando
- [ ] **CRUD de Produtos** com SKU único e soft delete
- [ ] **CRUD de Items (Lotes)** com data de validade e localização
- [ ] **Controle de estoque** (adicionar/remover quantidade)
- [ ] **Histórico de movimentações** completo e imutável
- [ ] **Auditoria** com registro de UserId
- [ ] **Listagem de produtos inativos** separada
- [ ] **Listagem de itens vencendo** funcional
- [ ] **Cálculo automático de status** dos itens
- [ ] **Cálculo de quantidade total** do produto (soma dos lotes)

---

### ✅ Validações de Negócio

- [ ] SKU é único no sistema
- [ ] Produtos inativos não aparecem em listagem de ativos
- [ ] Produtos inativos NÃO podem ser atualizados
- [ ] Soft delete funciona (produto não é deletado do banco)
- [ ] Estoque nunca fica negativo
- [ ] Data de validade, se informada, deve ser futura
- [ ] Toda alteração de estoque cria registro de Movement
- [ ] Movement é imutável (não pode ser editado/deletado)
- [ ] Status do item atualiza automaticamente após add/remove

---

### ✅ Arquitetura e Código

- [ ] Estrutura de pastas correta (Models, DTOs, Data, Repositories, Services, Controllers)
- [ ] Controllers NÃO acessam Repositories diretamente
- [ ] Services contêm validações de negócio
- [ ] Repositories apenas acessam dados
- [ ] Models NÃO acessam banco de dados
- [ ] DTOs separam modelo de apresentação
- [ ] Injeção de dependências configurada
- [ ] Código compila sem erros
- [ ] Sem warnings críticos

---

### ✅ Banco de Dados

- [ ] 5 tabelas criadas (Categories, Products, Items, Movements, Users)
- [ ] Relacionamentos FK configurados
- [ ] Índice único em SKU
- [ ] Migrations aplicadas
- [ ] Dados persistem corretamente

---

### ✅ API e Testes

- [ ] Todos os endpoints funcionam
- [ ] Status codes corretos (200, 201, 204, 400, 404)
- [ ] Mensagens de erro descritivas
- [ ] Swagger documenta todos os endpoints
- [ ] TODOS os testes de sucesso passam
- [ ] TODOS os testes de erro retornam código correto

---

### ✅ Documentação

- [ ] README.md completo
- [ ] Comandos de execução funcionam
- [ ] Swagger acessível e documentado
- [ ] Terceiro consegue executar projeto

---

## 🎯 PERGUNTAS DE AUTO-AVALIAÇÃO

**Responda SEM consultar código:**

### Conceitos

1. Qual a diferença entre Product e Item?
2. Por que SKU é único mas pode haver múltiplos Items?
3. O que acontece quando executo DELETE /api/products/{id}?

### Fluxo de Dados

4. Desenhe o caminho de POST /api/items/{id}/add-quantity
5. Onde é criado o registro de Movement?
6. Quantas tabelas são afetadas ao adicionar estoque?

### Regras de Negócio

7. Um produto inativo pode ser atualizado?
8. Um Movement pode ser deletado?
9. Como o Status do Item é calculado?

### Arquitetura

10. Controller pode acessar Repository diretamente?
11. Onde ficam as validações de negócio?
12. O que Service retorna: Model ou DTO?

**Mínimo: 10/12 para considerar pronto**

---

## 🚀 PRÓXIMOS PASSOS (Opcional)

Se completou TUDO e ainda tem tempo:

**Features Avançadas:**

- [ ] Paginação em listagens (page, pageSize)
- [ ] Filtros avançados (categoria, faixa de preço, range de validade)
- [ ] Relatório de movimentações por período
- [ ] Endpoint de estatísticas (produtos mais movimentados, etc)

**Qualidade:**

- [ ] Testes unitários com xUnit
- [ ] Testes de integração
- [ ] FluentValidation para validações
- [ ] AutoMapper para mapeamentos

**Infraestrutura:**

- [ ] Job automático de limpeza (produtos > X anos)
- [ ] Notificações de estoque baixo
- [ ] Logs estruturados com Serilog
- [ ] Health checks

---

**Começe pela FASE 1. Valide cada checklist antes de avançar. Go! 🚀**
