using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using controle_estoque_cshap.DTOs.ProductDto;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.ProductRepository;
using controle_estoque_cshap.Services.ProductService;

namespace controle_estoque_cshap.Tests;

public class ProductServiceTests
{
  [Test]
  public async Task GetInactiveAsync_MapsProductsAndTotals()
  {
    var products = new List<Product>
    {
      new()
      {
        ProductId = 1,
        Sku = "SKU-1",
        Name = "Produto 1",
        Status = 0,
        MinimumQuantity = 5,
        CategoryId = 2,
        Items = new List<Item> { new() { Quantity = 2 }, new() { Quantity = 3 } }
      }
    };

    var repo = new Mock<IProductRepository>();
    repo.Setup(r => r.GetInactiveAsync()).ReturnsAsync(products);

    var service = new ProductService(repo.Object);

    var result = (await service.GetInactiveAsync()).ToList();

    Assert.That(result, Has.Count.EqualTo(1));
    Assert.That(result[0].ProductId, Is.EqualTo(1));
    Assert.That(result[0].QuantityTotal, Is.EqualTo(5));
  }

  [Test]
  public async Task GetByIdAsync_ReturnsNull_WhenMissing()
  {
    var repo = new Mock<IProductRepository>();
    repo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((Product?)null);

    var service = new ProductService(repo.Object);

    var result = await service.GetByIdAsync(10);

    Assert.That(result, Is.Null);
  }

  [Test]
  public async Task GetByIdAsync_MapsProduct_WhenFound()
  {
    var product = new Product
    {
      ProductId = 3,
      Sku = "SKU-3",
      Name = "Produto 3",
      Status = 1,
      MinimumQuantity = 1,
      CategoryId = 4,
      Items = new List<Item> { new() { Quantity = 7 } }
    };

    var repo = new Mock<IProductRepository>();
    repo.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(product);

    var service = new ProductService(repo.Object);

    var result = await service.GetByIdAsync(3);

    Assert.That(result, Is.Not.Null);
    Assert.That(result!.ProductId, Is.EqualTo(3));
    Assert.That(result.QuantityTotal, Is.EqualTo(7));
  }

  [Test]
  public void GetActiveProducts_MapsToActiveDto()
  {
    var products = new List<Product>
    {
      new() { ProductId = 1, Name = "Ativo 1" },
      new() { ProductId = 2, Name = "Ativo 2" }
    };

    var repo = new Mock<IProductRepository>();
    repo.Setup(r => r.GetActiveAsync()).ReturnsAsync(products);

    var service = new ProductService(repo.Object);

    var result = service.GetActiveProducts();

    Assert.That(result, Has.Count.EqualTo(2));
    Assert.That(result[0].ProductId, Is.EqualTo(1));
    Assert.That(result[0].Name, Is.EqualTo("Ativo 1"));
  }
}
