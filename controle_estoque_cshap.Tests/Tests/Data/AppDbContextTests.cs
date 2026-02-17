using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NUnit.Framework;
using controle_estoque_cshap.Data;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Tests;

public class AppDbContextTests
{
  [Test]
  public void Model_HasExpectedTablesAndColumns()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(nameof(Model_HasExpectedTablesAndColumns))
      .Options;

    using var context = new AppDbContext(options);
    var model = context.Model;

    var category = model.FindEntityType(typeof(Category));
    Assert.That(category, Is.Not.Null);
    Assert.That(category!.GetTableName(), Is.EqualTo("category"));
    var categoryId = category.FindProperty(nameof(Category.CategoryId));
    Assert.That(categoryId!.GetColumnName(StoreObjectIdentifier.Table("category", null)), Is.EqualTo("category_id"));

    var product = model.FindEntityType(typeof(Product));
    Assert.That(product, Is.Not.Null);
    Assert.That(product!.GetTableName(), Is.EqualTo("product"));

    var item = model.FindEntityType(typeof(Item));
    Assert.That(item, Is.Not.Null);
    Assert.That(item!.GetTableName(), Is.EqualTo("item"));

    var movement = model.FindEntityType(typeof(Movement));
    Assert.That(movement, Is.Not.Null);
    Assert.That(movement!.GetTableName(), Is.EqualTo("movement"));

    var user = model.FindEntityType(typeof(User));
    Assert.That(user, Is.Not.Null);
    Assert.That(user!.GetTableName(), Is.EqualTo("user"));
  }

  [Test]
  public void DbSets_AreAvailable()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(nameof(DbSets_AreAvailable))
      .Options;

    using var context = new AppDbContext(options);

    Assert.That(context.Categories, Is.Not.Null);
    Assert.That(context.Products, Is.Not.Null);
    Assert.That(context.Items, Is.Not.Null);
    Assert.That(context.Movements, Is.Not.Null);
    Assert.That(context.Users, Is.Not.Null);
    Assert.That(context.EfmigrationsHistories, Is.Not.Null);
  }

  [Test]
  public void Model_DefinesExpectedKeys()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(nameof(Model_DefinesExpectedKeys))
      .Options;

    using var context = new AppDbContext(options);
    var model = context.Model;

    var categoryKey = model.FindEntityType(typeof(Category))!.FindPrimaryKey();
    Assert.That(categoryKey!.Properties.Single().Name, Is.EqualTo(nameof(Category.CategoryId)));

    var productKey = model.FindEntityType(typeof(Product))!.FindPrimaryKey();
    Assert.That(productKey!.Properties.Single().Name, Is.EqualTo(nameof(Product.ProductId)));

    var itemKey = model.FindEntityType(typeof(Item))!.FindPrimaryKey();
    Assert.That(itemKey!.Properties.Single().Name, Is.EqualTo(nameof(Item.ItemId)));

    var movementKey = model.FindEntityType(typeof(Movement))!.FindPrimaryKey();
    Assert.That(movementKey!.Properties.Single().Name, Is.EqualTo(nameof(Movement.MovementId)));

    var userKey = model.FindEntityType(typeof(User))!.FindPrimaryKey();
    Assert.That(userKey!.Properties.Single().Name, Is.EqualTo(nameof(User.IdUser)));
  }
}
