using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.CategoryRepository;

namespace controle_estoque_cshap.Tests;

public class CategoryRepositoryTests
{
  [Test]
  public async Task GetAllWithProductCountAsync_ReturnsCounts()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetAllWithProductCountAsync_ReturnsCounts));
    var category = new Category { Name = "Bebidas" };
    context.Categories.Add(category);
    context.Products.Add(new Product { Name = "Suco", Sku = "SKU1", Category = category });
    context.Products.Add(new Product { Name = "Cafe", Sku = "SKU2", Category = category });
    await context.SaveChangesAsync();

    var repo = new CategoryRepository(context);

    var result = (await repo.GetAllWithProductCountAsync()).ToList();

    Assert.That(result, Has.Count.EqualTo(1));
    Assert.That(result[0].TotalProducts, Is.EqualTo(2));
  }

  [Test]
  public async Task GetByIdWithProductCountAsync_ReturnsNull_WhenMissing()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetByIdWithProductCountAsync_ReturnsNull_WhenMissing));
    var repo = new CategoryRepository(context);

    var result = await repo.GetByIdWithProductCountAsync(99);

    Assert.That(result, Is.Null);
  }

  [Test]
  public async Task GetCategoryByNameAsync_TrimsName()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetCategoryByNameAsync_TrimsName));
    context.Categories.Add(new Category { Name = "Lanches" });
    await context.SaveChangesAsync();

    var repo = new CategoryRepository(context);

    var result = await repo.GetCategoryByNameAsync(" Lanches ");

    Assert.That(result, Is.Not.Null);
  }

  [Test]
  public async Task CreateCategoryAsync_PersistsCategory()
  {
    await using var context = TestDbContextFactory.Create(nameof(CreateCategoryAsync_PersistsCategory));
    var repo = new CategoryRepository(context);

    await repo.CreateCategoryAsync(new Category { Name = "Sobremesas" });

    Assert.That(context.Categories.Count(), Is.EqualTo(1));
  }

  [Test]
  public async Task UpdateCategoryAsync_SavesChanges()
  {
    await using var context = TestDbContextFactory.Create(nameof(UpdateCategoryAsync_SavesChanges));
    var category = new Category { Name = "Bebidas" };
    context.Categories.Add(category);
    await context.SaveChangesAsync();

    category.Name = "Bebidas Geladas";
    var repo = new CategoryRepository(context);

    await repo.UpdateCategoryAsync();

    var updated = context.Categories.First();
    Assert.That(updated.Name, Is.EqualTo("Bebidas Geladas"));
  }

  [Test]
  public async Task GetCategoryByIdForUpdateAsync_ReturnsCategoryForUpdate()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetCategoryByIdForUpdateAsync_ReturnsCategoryForUpdate));
    var category = new Category { Name = "Bebidas" };
    context.Categories.Add(category);
    await context.SaveChangesAsync();

    var repo = new CategoryRepository(context);
    var result = await repo.GetCategoryByIdForUpdateAsync(category.CategoryId);

    Assert.That(result, Is.Not.Null);
    Assert.That(result.Name, Is.EqualTo("Bebidas"));
  }
}
