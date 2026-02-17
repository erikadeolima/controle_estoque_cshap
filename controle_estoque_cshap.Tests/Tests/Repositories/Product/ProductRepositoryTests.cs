using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.ProductRepository;

namespace controle_estoque_cshap.Tests;

public class ProductRepositoryTests
{
  [Test]
  public async Task GetInactiveAsync_ReturnsOnlyInactive()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetInactiveAsync_ReturnsOnlyInactive));
    context.Products.Add(new Product { Name = "Ativo", Sku = "A1", Status = 1, CategoryId = 1 });
    context.Products.Add(new Product { Name = "Inativo", Sku = "I1", Status = 0, CategoryId = 1 });
    await context.SaveChangesAsync();

    var repo = new ProductRepository(context);

    var result = (await repo.GetInactiveAsync()).ToList();

    Assert.That(result, Has.Count.EqualTo(1));
    Assert.That(result[0].Status, Is.EqualTo(0));
  }

  [Test]
  public async Task GetByIdAsync_ReturnsProduct_WhenFound()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetByIdAsync_ReturnsProduct_WhenFound));
    var product = new Product { Name = "Cafe", Sku = "C1", Status = 1, CategoryId = 1 };
    context.Products.Add(product);
    await context.SaveChangesAsync();

    var repo = new ProductRepository(context);

    var result = await repo.GetByIdAsync(product.ProductId);

    Assert.That(result, Is.Not.Null);
    Assert.That(result!.Sku, Is.EqualTo("C1"));
  }

  [Test]
  public async Task GetActiveAsync_ReturnsOnlyActive()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetActiveAsync_ReturnsOnlyActive));
    context.Products.Add(new Product { Name = "Ativo", Sku = "A1", Status = 1, CategoryId = 1 });
    context.Products.Add(new Product { Name = "Inativo", Sku = "I1", Status = 0, CategoryId = 1 });
    await context.SaveChangesAsync();

    var repo = new ProductRepository(context);

    var result = await repo.GetActiveAsync();

    Assert.That(result, Has.Count.EqualTo(1));
    Assert.That(result[0].Status, Is.EqualTo(1));
  }
}
