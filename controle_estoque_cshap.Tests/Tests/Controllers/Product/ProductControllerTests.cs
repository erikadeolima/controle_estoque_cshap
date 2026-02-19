using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using controle_estoque_cshap.Controllers;
using controle_estoque_cshap.DTOs.ProductDto;
using controle_estoque_cshap.DTOs.ItemDto;
using controle_estoque_cshap.Services.ProductService;
using controle_estoque_cshap.Services.ItemService;

namespace controle_estoque_cshap.Tests;

public class ProductControllerTests
{
  [Test]
  public async Task GetInactive_ReturnsOk_WhenFound()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    var products = new List<ProductDto> { new() { ProductId = 1, Name = "Inativo", Status = 0 } };
    service.Setup(s => s.GetInactiveAsync()).ReturnsAsync(products);

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.GetInactive();

    Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
  }

  [Test]
  public async Task GetInactive_ReturnsNotFound_WhenNull()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.GetInactiveAsync()).ReturnsAsync((IEnumerable<ProductDto>)null!);

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.GetInactive();

    Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task GetInactive_ReturnsServerError_OnException()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.GetInactiveAsync()).ThrowsAsync(new InvalidOperationException("boom"));

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.GetInactive();

    Assert.That(result.Result, Is.TypeOf<ObjectResult>());
    var error = (ObjectResult)result.Result!;
    Assert.That(error.StatusCode, Is.EqualTo(500));
  }

  [Test]
  public async Task GetById_ReturnsNotFound_WhenMissing()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.GetByIdAsync(10)).ReturnsAsync((ProductDto?)null);

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.GetById(10);

    Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task GetById_ReturnsNotFound_WhenInactive()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.GetByIdAsync(2)).ReturnsAsync(new ProductDto { ProductId = 2, Status = 0 });

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.GetById(2);

    Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task GetById_ReturnsOk_WhenActive()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    var dto = new ProductDto { ProductId = 3, Status = 1 };
    service.Setup(s => s.GetByIdAsync(3)).ReturnsAsync(dto);

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.GetById(3);

    Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
  }

  [Test]
  public async Task GetById_ReturnsServerError_OnException()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.GetByIdAsync(1)).ThrowsAsync(new InvalidOperationException("boom"));

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.GetById(1);

    Assert.That(result.Result, Is.TypeOf<ObjectResult>());
    var error = (ObjectResult)result.Result!;
    Assert.That(error.StatusCode, Is.EqualTo(500));
  }

  [Test]
  public async Task GetBySkuAsync_ReturnsOk_WhenFound()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    var dto = new ProductDto { ProductId = 1, Sku = "SKU123", Status = 1 };
    service.Setup(s => s.GetBySkuAsync("SKU123")).ReturnsAsync(dto);

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.GetBySkuAsync("SKU123");

    Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
  }

  [Test]
  public async Task GetBySkuAsync_ReturnsNotFound_WhenMissing()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.GetBySkuAsync("INVALID")).ReturnsAsync((ProductDto?)null);

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.GetBySkuAsync("INVALID");

    Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task GetBySkuAsync_ReturnsNotFound_WhenInactive()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.GetBySkuAsync("SKU456")).ReturnsAsync(
      new ProductDto { ProductId = 2, Sku = "SKU456", Status = 0 }
    );

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.GetBySkuAsync("SKU456");

    Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task GetBySkuAsync_ReturnsServerError_OnException()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.GetBySkuAsync("SKU789")).ThrowsAsync(new InvalidOperationException("boom"));

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.GetBySkuAsync("SKU789");

    Assert.That(result.Result, Is.TypeOf<ObjectResult>());
    var error = (ObjectResult)result.Result!;
    Assert.That(error.StatusCode, Is.EqualTo(500));
  }

  [Test]
  public async Task Create_ReturnsCreated_WhenSuccess()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    var dto = new ProductCreateDto { Name = "New Product", Sku = "NEW123" };
    var createdDto = new ProductDto { ProductId = 1, Name = "New Product", Sku = "NEW123", Status = 1 };
    service.Setup(s => s.CreateAsync(dto)).ReturnsAsync(createdDto);

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.Create(dto);

    Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
    var createdResult = (CreatedAtActionResult)result.Result!;
    Assert.That(createdResult.ActionName, Is.EqualTo(nameof(ProductController.GetById)));
  }

  [Test]
  public async Task Create_ReturnsBadRequest_WhenServiceReturnsNull()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    var dto = new ProductCreateDto { Name = "New Product", Sku = "NEW456" };
    service.Setup(s => s.CreateAsync(dto)).ReturnsAsync((ProductDto?)null);

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.Create(dto);

    Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
  }

  [Test]
  public async Task Create_ReturnsServerError_OnException()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    var dto = new ProductCreateDto { Name = "New Product", Sku = "NEW789" };
    service.Setup(s => s.CreateAsync(dto)).ThrowsAsync(new InvalidOperationException("boom"));

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.Create(dto);

    Assert.That(result.Result, Is.TypeOf<ObjectResult>());
    var error = (ObjectResult)result.Result!;
    Assert.That(error.StatusCode, Is.EqualTo(500));
  }

  [Test]
  public async Task Create_CreatesItems_WhenItemsProvided()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    var itemDto = new ItemCreateDto { ProductId = 1, Batch = "BATCH001", Quantity = 10 };
    var dto = new ProductCreateDto { Name = "New Product", Sku = "NEW123", Items = new List<ItemCreateDto> { itemDto } };
    var createdDto = new ProductDto { ProductId = 1, Name = "New Product", Sku = "NEW123", Status = 1 };
    service.Setup(s => s.CreateAsync(dto)).ReturnsAsync(createdDto);
    itemService.Setup(i => i.CreateItemAsync(It.IsAny<ItemCreateDto>())).ReturnsAsync(new ItemDto { ItemId = 1 });

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.Create(dto);

    Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
    itemService.Verify(i => i.CreateItemAsync(It.IsAny<ItemCreateDto>()), Times.Once);
  }

  [Test]
  public async Task Update_ReturnsOk_WhenSuccess()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    var updateDto = new ProductUpdateDto { Name = "Updated Product" };
    var updatedDto = new ProductDto { ProductId = 1, Name = "Updated Product", Status = 1 };
    service.Setup(s => s.UpdateAsync(1, updateDto)).ReturnsAsync(updatedDto);

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.Update(1, updateDto);

    Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
  }

  [Test]
  public async Task Update_ReturnsNotFound_WhenProductNotFound()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    var updateDto = new ProductUpdateDto { Name = "Updated Product" };
    service.Setup(s => s.UpdateAsync(999, updateDto)).ReturnsAsync((ProductDto?)null);

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.Update(999, updateDto);

    Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task Update_ReturnsServerError_OnException()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    var updateDto = new ProductUpdateDto { Name = "Updated Product" };
    service.Setup(s => s.UpdateAsync(1, updateDto)).ThrowsAsync(new InvalidOperationException("boom"));

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.Update(1, updateDto);

    Assert.That(result.Result, Is.TypeOf<ObjectResult>());
    var error = (ObjectResult)result.Result!;
    Assert.That(error.StatusCode, Is.EqualTo(500));
  }

  [Test]
  public async Task Delete_ReturnsNoContent_WhenSuccess()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.Delete(1);

    Assert.That(result, Is.TypeOf<NoContentResult>());
  }

  [Test]
  public async Task Delete_ReturnsNotFound_WhenProductNotFound()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.DeleteAsync(999)).ReturnsAsync(false);

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.Delete(999);

    Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task Delete_ReturnsServerError_OnException()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.DeleteAsync(1)).ThrowsAsync(new InvalidOperationException("boom"));

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.Delete(1);

    Assert.That(result, Is.TypeOf<ObjectResult>());
    var error = (ObjectResult)result!;
    Assert.That(error.StatusCode, Is.EqualTo(500));
  }

  [Test]
  public async Task GetLowStock_ReturnsOk_WhenFound()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    var products = new List<ProductDto> { new() { ProductId = 1, Name = "Low Stock", Status = 1 } };
    service.Setup(s => s.GetLowStockAsync()).ReturnsAsync(products);

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.GetLowStock();

    Assert.That(result, Is.TypeOf<OkObjectResult>());
  }

  [Test]
  public async Task GetLowStock_ReturnsNotFound_WhenEmpty()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.GetLowStockAsync()).ReturnsAsync(new List<ProductDto>());

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.GetLowStock();

    Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task GetLowStock_ReturnsServerError_OnException()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.GetLowStockAsync()).ThrowsAsync(new InvalidOperationException("boom"));

    var controller = new ProductController(service.Object, itemService.Object);

    var result = await controller.GetLowStock();

    Assert.That(result, Is.TypeOf<ObjectResult>());
    var error = (ObjectResult)result!;
    Assert.That(error.StatusCode, Is.EqualTo(500));
  }

  [Test]
  public void GetActiveProducts_ReturnsNotFound_WhenEmpty()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.GetActiveProducts()).Returns(new List<ProductActiveDto>());

    var controller = new ProductController(service.Object, itemService.Object);

    var result = controller.GetActiveProducts();

    Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public void GetActiveProducts_ReturnsOk_WhenFound()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.GetActiveProducts()).Returns(new List<ProductActiveDto>
    {
      new() { ProductId = 1, Name = "Ativo" }
    });

    var controller = new ProductController(service.Object, itemService.Object);

    var result = controller.GetActiveProducts();

    Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
  }

  [Test]
  public void GetActiveProducts_ReturnsServerError_OnException()
  {
    var service = new Mock<IProductService>();
    var itemService = new Mock<IItemService>();
    service.Setup(s => s.GetActiveProducts()).Throws(new InvalidOperationException("boom"));

    var controller = new ProductController(service.Object, itemService.Object);

    var result = controller.GetActiveProducts();

    Assert.That(result.Result, Is.TypeOf<ObjectResult>());
    var error = (ObjectResult)result.Result!;
    Assert.That(error.StatusCode, Is.EqualTo(500));
  }
}
