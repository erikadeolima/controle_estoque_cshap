using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using controle_estoque_cshap.Data;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.ItemRepository;

namespace controle_estoque_cshap.Tests;

public class ItemRepositoryTests
{
  private static Product AddProduct(AppDbContext context, string name, string sku)
  {
    var category = new Category { Name = "Categoria" };
    var product = new Product { Name = name, Sku = sku, Category = category };
    context.Categories.Add(category);
    context.Products.Add(product);
    return product;
  }

  [Test]
  public async Task GetAllAsync_IncludesProduct()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetAllAsync_IncludesProduct));
    var product = AddProduct(context, "Produto", "P1");
    var item = new Item { Batch = "B1", Quantity = 1, Product = product };
    context.Items.Add(item);
    await context.SaveChangesAsync();

    var repo = new ItemRepository(context);

    var result = (await repo.GetAllAsync()).ToList();

    Assert.That(result, Has.Count.EqualTo(1));
    Assert.That(result[0].Product, Is.Not.Null);
  }

  [Test]
  public async Task GetByIdAsync_ReturnsItem_WhenFound()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetByIdAsync_ReturnsItem_WhenFound));
    var product = AddProduct(context, "Produto", "P1");
    var item = new Item { Batch = "B1", Quantity = 1, Product = product };
    context.Items.Add(item);
    await context.SaveChangesAsync();

    var repo = new ItemRepository(context);

    var result = await repo.GetByIdAsync(item.ItemId);

    Assert.That(result, Is.Not.Null);
    Assert.That(result!.Batch, Is.EqualTo("B1"));
  }

  [Test]
  public async Task GetByIdForUpdateAsync_ReturnsTrackedEntity()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetByIdForUpdateAsync_ReturnsTrackedEntity));
    var product = AddProduct(context, "Produto", "P1");
    var item = new Item { Batch = "B1", Quantity = 1, Product = product };
    context.Items.Add(item);
    await context.SaveChangesAsync();

    var repo = new ItemRepository(context);

    var result = await repo.GetByIdForUpdateAsync(item.ItemId);

    Assert.That(result, Is.Not.Null);
    Assert.That(context.ChangeTracker.Entries<Item>().Any(e => e.Entity.ItemId == item.ItemId), Is.True);
  }

  [Test]
  public async Task GetByProductIdAsync_FiltersByProduct()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetByProductIdAsync_FiltersByProduct));
    var product1 = AddProduct(context, "Produto 1", "P1");
    var product2 = AddProduct(context, "Produto 2", "P2");
    context.Items.Add(new Item { Batch = "B1", Quantity = 1, Product = product1 });
    context.Items.Add(new Item { Batch = "B2", Quantity = 1, Product = product2 });
    await context.SaveChangesAsync();

    var repo = new ItemRepository(context);

    var result = (await repo.GetByProductIdAsync(1)).ToList();

    Assert.That(result, Has.Count.EqualTo(1));
    Assert.That(result[0].Batch, Is.EqualTo("B1"));
  }

  [Test]
  public async Task GetExpiringItemsAsync_FiltersByDate()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetExpiringItemsAsync_FiltersByDate));
    var product = AddProduct(context, "Produto", "P1");
    context.Items.Add(new Item { Batch = "B1", Quantity = 1, Product = product, ExpirationDate = DateTime.UtcNow.Date.AddDays(3) });
    context.Items.Add(new Item { Batch = "B2", Quantity = 1, Product = product, ExpirationDate = DateTime.UtcNow.Date.AddDays(10) });
    await context.SaveChangesAsync();

    var repo = new ItemRepository(context);

    var result = (await repo.GetExpiringItemsAsync(7)).ToList();

    Assert.That(result, Has.Count.EqualTo(1));
    Assert.That(result[0].Batch, Is.EqualTo("B1"));
  }

  [Test]
  public async Task CreateAsync_PersistsItem()
  {
    await using var context = TestDbContextFactory.Create(nameof(CreateAsync_PersistsItem));
    var repo = new ItemRepository(context);
    var product = AddProduct(context, "Produto", "P1");

    await repo.CreateAsync(new Item { Batch = "B1", Quantity = 1, Product = product });

    Assert.That(context.Items.Count(), Is.EqualTo(1));
  }

  [Test]
  public async Task UpdateAsync_SavesChanges()
  {
    await using var context = TestDbContextFactory.Create(nameof(UpdateAsync_SavesChanges));
    var product = AddProduct(context, "Produto", "P1");
    var item = new Item { Batch = "B1", Quantity = 1, Product = product };
    context.Items.Add(item);
    await context.SaveChangesAsync();

    item.Batch = "B2";
    var repo = new ItemRepository(context);

    await repo.UpdateAsync(item);

    var updated = context.Items.First();
    Assert.That(updated.Batch, Is.EqualTo("B2"));
  }

  [Test]
  public async Task DeleteAsync_RemovesItem()
  {
    await using var context = TestDbContextFactory.Create(nameof(DeleteAsync_RemovesItem));
    var product = AddProduct(context, "Produto", "P1");
    var item = new Item { Batch = "B1", Quantity = 1, Product = product };
    context.Items.Add(item);
    await context.SaveChangesAsync();

    var repo = new ItemRepository(context);

    await repo.DeleteAsync(item);

    Assert.That(context.Items.Count(), Is.EqualTo(0));
  }
}
