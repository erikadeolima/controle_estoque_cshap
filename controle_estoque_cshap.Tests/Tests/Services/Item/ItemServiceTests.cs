using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using controle_estoque_cshap.DTOs.ItemDto;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.ItemRepository;
using controle_estoque_cshap.Repositories.ProductRepository;
using controle_estoque_cshap.Services.ItemService;

namespace controle_estoque_cshap.Tests;

public class ItemServiceTests
{
  [Test]
  public async Task GetAllAsync_MapsItems()
  {
    var items = new List<Item>
    {
      new() { ItemId = 1, ProductId = 2, Batch = "B1", Quantity = 3 }
    };

    var itemRepo = new Mock<IItemRepository>();
    var productRepo = new Mock<IProductRepository>();
    itemRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(items);

    var service = new ItemService(itemRepo.Object, productRepo.Object);

    var result = (await service.GetAllAsync()).ToList();

    Assert.That(result, Has.Count.EqualTo(1));
    Assert.That(result[0].ItemId, Is.EqualTo(1));
  }

  [Test]
  public void CreateItemAsync_Throws_WhenBatchMissing()
  {
    var itemRepo = new Mock<IItemRepository>();
    var productRepo = new Mock<IProductRepository>();
    var service = new ItemService(itemRepo.Object, productRepo.Object);

    var dto = new ItemCreateDto { Batch = "", Quantity = 1, Location = "A" };

    Assert.That(async () => await service.CreateItemAsync(dto), Throws.ArgumentException);
  }

  [Test]
  public void CreateItemAsync_Throws_WhenQuantityNegative()
  {
    var itemRepo = new Mock<IItemRepository>();
    var productRepo = new Mock<IProductRepository>();
    var service = new ItemService(itemRepo.Object, productRepo.Object);

    var dto = new ItemCreateDto { Batch = "B1", Quantity = -1, Location = "A" };

    Assert.That(async () => await service.CreateItemAsync(dto), Throws.ArgumentException);
  }

  [Test]
  public void CreateItemAsync_Throws_WhenLocationMissing()
  {
    var itemRepo = new Mock<IItemRepository>();
    var productRepo = new Mock<IProductRepository>();
    var service = new ItemService(itemRepo.Object, productRepo.Object);

    var dto = new ItemCreateDto { Batch = "B1", Quantity = 1, Location = "" };

    Assert.That(async () => await service.CreateItemAsync(dto), Throws.ArgumentException);
  }

  [Test]
  public void CreateItemAsync_Throws_WhenExpirationPast()
  {
    var itemRepo = new Mock<IItemRepository>();
    var productRepo = new Mock<IProductRepository>();
    var service = new ItemService(itemRepo.Object, productRepo.Object);

    var dto = new ItemCreateDto
    {
      Batch = "B1",
      Quantity = 1,
      Location = "A",
      ExpirationDate = DateTime.Now.AddDays(-1)
    };

    Assert.That(async () => await service.CreateItemAsync(dto), Throws.ArgumentException);
  }

  [Test]
  public async Task CreateItemAsync_ReturnsNull_WhenProductMissing()
  {
    var itemRepo = new Mock<IItemRepository>();
    var productRepo = new Mock<IProductRepository>();
    productRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((Product?)null);

    var service = new ItemService(itemRepo.Object, productRepo.Object);
    var dto = new ItemCreateDto { ProductId = 10, Batch = "B1", Quantity = 1, Location = "A" };

    var result = await service.CreateItemAsync(dto);

    Assert.That(result, Is.Null);
  }

  [Test]
  public async Task CreateItemAsync_SetsStatusOutOfStock_WhenQuantityZero()
  {
    var product = new Product { ProductId = 1, MinimumQuantity = 5, Name = "Produto" };
    var itemRepo = new Mock<IItemRepository>();
    var productRepo = new Mock<IProductRepository>();
    productRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

    Item? created = null;
    itemRepo.Setup(r => r.CreateAsync(It.IsAny<Item>()))
      .Callback<Item>(i => { i.ItemId = 11; created = i; })
      .ReturnsAsync((Item i) => i);

    itemRepo.Setup(r => r.GetByIdAsync(11)).ReturnsAsync(() =>
    {
      created!.Product = product;
      return created;
    });

    var service = new ItemService(itemRepo.Object, productRepo.Object);
    var dto = new ItemCreateDto { ProductId = 1, Batch = "B1", Quantity = 0, Location = "A" };

    var result = await service.CreateItemAsync(dto);

    Assert.That(result, Is.Not.Null);
    Assert.That(result!.Status, Is.EqualTo(3));
  }

  [Test]
  public async Task CreateItemAsync_SetsStatusAlert_WhenQuantityBelowMinimum()
  {
    var product = new Product { ProductId = 1, MinimumQuantity = 5, Name = "Produto" };
    var itemRepo = new Mock<IItemRepository>();
    var productRepo = new Mock<IProductRepository>();
    productRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

    Item? created = null;
    itemRepo.Setup(r => r.CreateAsync(It.IsAny<Item>()))
      .Callback<Item>(i => { i.ItemId = 12; created = i; })
      .ReturnsAsync((Item i) => i);

    itemRepo.Setup(r => r.GetByIdAsync(12)).ReturnsAsync(() =>
    {
      created!.Product = product;
      return created;
    });

    var service = new ItemService(itemRepo.Object, productRepo.Object);
    var dto = new ItemCreateDto { ProductId = 1, Batch = "B1", Quantity = 3, Location = "A" };

    var result = await service.CreateItemAsync(dto);

    Assert.That(result, Is.Not.Null);
    Assert.That(result!.Status, Is.EqualTo(2));
  }

  [Test]
  public async Task UpdateItemAsync_ReturnsNull_WhenMissing()
  {
    var itemRepo = new Mock<IItemRepository>();
    var productRepo = new Mock<IProductRepository>();
    itemRepo.Setup(r => r.GetByIdForUpdateAsync(1)).ReturnsAsync((Item?)null);

    var service = new ItemService(itemRepo.Object, productRepo.Object);
    var dto = new ItemUpdateDto { Batch = "B" };

    var result = await service.UpdateItemAsync(1, dto);

    Assert.That(result, Is.Null);
  }

  [Test]
  public void UpdateItemAsync_Throws_WhenInactive()
  {
    var itemRepo = new Mock<IItemRepository>();
    var productRepo = new Mock<IProductRepository>();
    itemRepo.Setup(r => r.GetByIdForUpdateAsync(1)).ReturnsAsync(new Item { Status = 0 });

    var service = new ItemService(itemRepo.Object, productRepo.Object);
    var dto = new ItemUpdateDto { Batch = "B" };

    Assert.That(async () => await service.UpdateItemAsync(1, dto), Throws.InvalidOperationException);
  }

  [Test]
  public async Task UpdateItemAsync_UpdatesFields_WhenValid()
  {
    var item = new Item { ItemId = 1, Status = 1, Batch = "B1", Location = "L1" };
    var itemRepo = new Mock<IItemRepository>();
    var productRepo = new Mock<IProductRepository>();
    itemRepo.Setup(r => r.GetByIdForUpdateAsync(1)).ReturnsAsync(item);
    itemRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(item);
    itemRepo.Setup(r => r.UpdateAsync()).Returns(Task.CompletedTask);

    var service = new ItemService(itemRepo.Object, productRepo.Object);
    var dto = new ItemUpdateDto { Batch = "B2", Location = "L2" };

    var result = await service.UpdateItemAsync(1, dto);

    Assert.That(result, Is.Not.Null);
    Assert.That(item.Batch, Is.EqualTo("B2"));
    Assert.That(item.Location, Is.EqualTo("L2"));
  }

  [Test]
  public async Task DeleteItemAsync_ReturnsNotFound_WhenMissing()
  {
    var itemRepo = new Mock<IItemRepository>();
    var productRepo = new Mock<IProductRepository>();
    itemRepo.Setup(r => r.GetByIdForUpdateAsync(1)).ReturnsAsync((Item?)null);

    var service = new ItemService(itemRepo.Object, productRepo.Object);

    var result = await service.DeleteItemAsync(1);

    Assert.That(result, Is.EqualTo(ItemDeleteResult.NotFound));
  }

  [Test]
  public async Task DeleteItemAsync_ReturnsAlreadyInactive_WhenStatusZero()
  {
    var itemRepo = new Mock<IItemRepository>();
    var productRepo = new Mock<IProductRepository>();
    itemRepo.Setup(r => r.GetByIdForUpdateAsync(1)).ReturnsAsync(new Item { Status = 0 });

    var service = new ItemService(itemRepo.Object, productRepo.Object);

    var result = await service.DeleteItemAsync(1);

    Assert.That(result, Is.EqualTo(ItemDeleteResult.AlreadyInactive));
  }

  [Test]
  public async Task DeleteItemAsync_Deactivates_WhenActive()
  {
    var item = new Item { Status = 1 };
    var itemRepo = new Mock<IItemRepository>();
    var productRepo = new Mock<IProductRepository>();
    itemRepo.Setup(r => r.GetByIdForUpdateAsync(1)).ReturnsAsync(item);
    itemRepo.Setup(r => r.UpdateAsync()).Returns(Task.CompletedTask);

    var service = new ItemService(itemRepo.Object, productRepo.Object);

    var result = await service.DeleteItemAsync(1);

    Assert.That(result, Is.EqualTo(ItemDeleteResult.Deactivated));
    Assert.That(item.Status, Is.EqualTo(0));
  }
}
