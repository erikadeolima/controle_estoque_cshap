using NUnit.Framework;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Tests;

public class ModelTests
{
  [Test]
  public void Category_AllowsPropertyAccess()
  {
    var category = new Category { CategoryId = 1, Name = "Bebidas", Description = "Desc" };

    Assert.That(category.CategoryId, Is.EqualTo(1));
    Assert.That(category.Name, Is.EqualTo("Bebidas"));
    Assert.That(category.Description, Is.EqualTo("Desc"));
    Assert.That(category.Products, Is.Not.Null);
  }

  [Test]
  public void Product_AllowsPropertyAccess()
  {
    var product = new Product { ProductId = 1, Sku = "SKU", Name = "Produto", CategoryId = 2 };

    Assert.That(product.ProductId, Is.EqualTo(1));
    Assert.That(product.Sku, Is.EqualTo("SKU"));
    Assert.That(product.Name, Is.EqualTo("Produto"));
    Assert.That(product.Items, Is.Not.Null);
  }

  [Test]
  public void Item_AllowsPropertyAccess()
  {
    var item = new Item { ItemId = 1, Batch = "B1", Quantity = 2, ProductId = 3 };

    Assert.That(item.ItemId, Is.EqualTo(1));
    Assert.That(item.Batch, Is.EqualTo("B1"));
    Assert.That(item.Quantity, Is.EqualTo(2));
    Assert.That(item.Movements, Is.Not.Null);
  }

  [Test]
  public void Movement_AllowsPropertyAccess()
  {
    var movement = new Movement { MovementId = 1, Type = "Entrada", QuantityMoved = 2, ItemId = 3, UserId = 4 };

    Assert.That(movement.MovementId, Is.EqualTo(1));
    Assert.That(movement.Type, Is.EqualTo("Entrada"));
    Assert.That(movement.QuantityMoved, Is.EqualTo(2));
  }

  [Test]
  public void User_AllowsPropertyAccess()
  {
    var user = new User { IdUser = 1, Name = "Ana", Email = "ana@example.com", Profile = "Admin" };

    Assert.That(user.IdUser, Is.EqualTo(1));
    Assert.That(user.Name, Is.EqualTo("Ana"));
    Assert.That(user.Email, Is.EqualTo("ana@example.com"));
    Assert.That(user.Profile, Is.EqualTo("Admin"));
    Assert.That(user.Movements, Is.Not.Null);
  }
}
