using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using controle_estoque_cshap.Data;
using controle_estoque_cshap.DTOs.ItemDto;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Services.ItemService;
using controle_estoque_cshap.Repositories.ItemRepository;
using controle_estoque_cshap.Repositories.ProductRepository;
using ItemModel = controle_estoque_cshap.Models.Item;

namespace controle_estoque_cshap.Tests.Tests.Services.Item;

[TestFixture]
public class ItemServiceReportTests
{
  private IItemRepository _itemRepository = null!;
  private IProductRepository _productRepository = null!;
  private IItemService _service = null!;
  private AppDbContext _context = null!;

  [SetUp]
  public void Setup()
  {
    _context = TestDbContextFactory.Create($"{nameof(ItemServiceReportTests)}-{Guid.NewGuid()}");
    _itemRepository = new ItemRepository(_context);
    _productRepository = new ProductRepository(_context);
    _service = new ItemService(_itemRepository, _productRepository);
  }

  [TearDown]
  public async Task Teardown()
  {
    if (_context != null)
    {
      _context.Database.EnsureDeleted();
      await _context.DisposeAsync();
    }
  }

  [Test]
  public async Task GenerateExpirationReportCsvAsync_ReturnsValidCsv()
  {
    // Arrange
    var category = new Category { Name = "Alimentos", Description = "Produtos alimentícios" };
    await _context.Categories.AddAsync(category);
    await _context.SaveChangesAsync();

    var product = new Product
    {
      Sku = "TEST-001",
      Name = "Produto Teste",
      Status = 1,
      MinimumQuantity = 5,
      CategoryId = category.CategoryId
    };
    await _context.Products.AddAsync(product);
    await _context.SaveChangesAsync();

    var expiringDate = DateTime.UtcNow.AddDays(3);
    var item = new ItemModel
    {
      Batch = "LOTE-001",
      Quantity = 10,
      Location = "Prateleira A",
      ExpirationDate = expiringDate,
      ProductId = product.ProductId,
      Status = 1
    };
    await _context.Items.AddAsync(item);
    await _context.SaveChangesAsync();

    // Act
    var csv = await _service.GenerateExpirationReportCsvAsync(7);

    // Assert
    Assert.IsNotNull(csv);
    Assert.IsNotEmpty(csv);
    Assert.That(csv, Does.Contain("Item ID"));
    Assert.That(csv, Does.Contain(item.ItemId.ToString()));
    Assert.That(csv, Does.Contain("LOTE-001"));
    Assert.That(csv, Does.Contain("Produto Teste"));
  }

  [Test]
  public async Task GenerateExpirationReportCsvAsync_ReturnsEmpty_WhenNoExpiringItems()
  {
    // Arrange
    var category = new Category { Name = "Alimentos" };
    await _context.Categories.AddAsync(category);
    await _context.SaveChangesAsync();

    var product = new Product
    {
      Sku = "TEST-002",
      Name = "Produto Futuro",
      Status = 1,
      MinimumQuantity = 5,
      CategoryId = category.CategoryId
    };
    await _context.Products.AddAsync(product);
    await _context.SaveChangesAsync();

    var futureDate = DateTime.UtcNow.AddDays(30);
    var item = new ItemModel
    {
      Batch = "LOTE-002",
      Quantity = 10,
      Location = "Prateleira B",
      ExpirationDate = futureDate,
      ProductId = product.ProductId,
      Status = 1
    };
    await _context.Items.AddAsync(item);
    await _context.SaveChangesAsync();

    // Act
    var csv = await _service.GenerateExpirationReportCsvAsync(7);

    // Assert
    Assert.IsEmpty(csv);
  }

  [Test]
  public async Task GenerateExpiredItemsReportCsvAsync_ReturnsValidCsv()
  {
    // Arrange
    var category = new Category { Name = "Bebidas" };
    await _context.Categories.AddAsync(category);
    await _context.SaveChangesAsync();

    var product = new Product
    {
      Sku = "TEST-003",
      Name = "Bebida Vencida",
      Status = 1,
      MinimumQuantity = 5,
      CategoryId = category.CategoryId
    };
    await _context.Products.AddAsync(product);
    await _context.SaveChangesAsync();

    var expiredDate = DateTime.UtcNow.AddDays(-5);
    var item = new ItemModel
    {
      Batch = "LOTE-003",
      Quantity = 5,
      Location = "Prateleira C",
      ExpirationDate = expiredDate,
      ProductId = product.ProductId,
      Status = 1
    };
    await _context.Items.AddAsync(item);
    await _context.SaveChangesAsync();

    // Act
    var csv = await _service.GenerateExpiredItemsReportCsvAsync();

    // Assert
    Assert.IsNotNull(csv);
    Assert.IsNotEmpty(csv);
    Assert.That(csv, Does.Contain("Item ID"));
    Assert.That(csv, Does.Contain("Dias Vencido"));
    Assert.That(csv, Does.Contain(item.ItemId.ToString()));
  }

  [Test]
  public async Task GenerateExpiredItemsReportCsvAsync_ReturnsEmpty_WhenNoExpiredItems()
  {
    // Arrange
    var category = new Category { Name = "Alimentos" };
    await _context.Categories.AddAsync(category);
    await _context.SaveChangesAsync();

    var product = new Product
    {
      Sku = "TEST-004",
      Name = "Produto Válido",
      Status = 1,
      MinimumQuantity = 5,
      CategoryId = category.CategoryId
    };
    await _context.Products.AddAsync(product);
    await _context.SaveChangesAsync();

    var futureDate = DateTime.UtcNow.AddDays(10);
    var item = new ItemModel
    {
      Batch = "LOTE-004",
      Quantity = 10,
      Location = "Prateleira D",
      ExpirationDate = futureDate,
      ProductId = product.ProductId,
      Status = 1
    };
    await _context.Items.AddAsync(item);
    await _context.SaveChangesAsync();

    // Act
    var csv = await _service.GenerateExpiredItemsReportCsvAsync();

    // Assert
    Assert.IsEmpty(csv);
  }

  [Test]
  public async Task GenerateExpirationReportCsvAsync_FiltersOnlyItemsWithinWarningPeriod()
  {
    // Arrange
    var category = new Category { Name = "Comida" };
    await _context.Categories.AddAsync(category);
    await _context.SaveChangesAsync();

    var product = new Product
    {
      Sku = "TEST-005",
      Name = "Alimento",
      Status = 1,
      MinimumQuantity = 5,
      CategoryId = category.CategoryId
    };
    await _context.Products.AddAsync(product);
    await _context.SaveChangesAsync();

    // Item expirando em 3 dias (deve ser incluído)
    var soon = new ItemModel
    {
      Batch = "SOON",
      Quantity = 10,
      Location = "A",
      ExpirationDate = DateTime.UtcNow.AddDays(3),
      ProductId = product.ProductId,
      Status = 1
    };

    // Item expirando em 15 dias (não deve ser incluído com warning de 7 dias)
    var later = new ItemModel
    {
      Batch = "LATER",
      Quantity = 10,
      Location = "B",
      ExpirationDate = DateTime.UtcNow.AddDays(15),
      ProductId = product.ProductId,
      Status = 1
    };

    await _context.Items.AddAsync(soon);
    await _context.Items.AddAsync(later);
    await _context.SaveChangesAsync();

    // Act
    var csv = await _service.GenerateExpirationReportCsvAsync(7);

    // Assert
    Assert.That(csv, Does.Contain("SOON"));
    Assert.That(csv, Does.Not.Contain("LATER"));
  }

  [Test]
  public async Task GenerateExpirationReportCsvAsync_EscapesQuotesInCsv()
  {
    // Arrange
    var category = new Category { Name = "Alimentos \"Especiais\"" };
    await _context.Categories.AddAsync(category);
    await _context.SaveChangesAsync();

    var product = new Product
    {
      Sku = "TEST-006",
      Name = "Produto \"Premium\"",
      Status = 1,
      MinimumQuantity = 5,
      CategoryId = category.CategoryId
    };
    await _context.Products.AddAsync(product);
    await _context.SaveChangesAsync();

    var item = new ItemModel
    {
      Batch = "LOTE \"ESPECIAL\"",
      Quantity = 10,
      Location = "Prateleira \"A\"",
      ExpirationDate = DateTime.UtcNow.AddDays(3),
      ProductId = product.ProductId,
      Status = 1
    };
    await _context.Items.AddAsync(item);
    await _context.SaveChangesAsync();

    // Act
    var csv = await _service.GenerateExpirationReportCsvAsync(7);

    // Assert
    Assert.IsNotEmpty(csv);
    Assert.That(csv, Does.Contain("\"\"ESPECIAL\"\""));
  }
}
