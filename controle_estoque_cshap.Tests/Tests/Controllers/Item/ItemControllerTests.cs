using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using controle_estoque_cshap.Controllers;
using controle_estoque_cshap.DTOs.ItemDto;
using controle_estoque_cshap.Services.ItemService;

namespace controle_estoque_cshap.Tests;

public class ItemControllerTests
{
  [Test]
  public async Task GetAll_ReturnsOk_WhenSuccess()
  {
    var service = new Mock<IItemService>();
    var logger = new Mock<ILogger<ItemController>>();
    service.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ItemDto>());

    var controller = new ItemController(service.Object, logger.Object);

    var result = await controller.GetAll();

    Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
  }

  [Test]
  public async Task GetById_ReturnsNotFound_WhenMissing()
  {
    var service = new Mock<IItemService>();
    var logger = new Mock<ILogger<ItemController>>();
    service.Setup(s => s.GetItemByIdAsync(10)).ReturnsAsync((ItemDto?)null);

    var controller = new ItemController(service.Object, logger.Object);

    var result = await controller.GetById(10);

    Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task GetExpiring_ReturnsBadRequest_WhenDaysNegative()
  {
    var service = new Mock<IItemService>();
    var logger = new Mock<ILogger<ItemController>>();

    var controller = new ItemController(service.Object, logger.Object);

    var result = await controller.GetExpiring(-1);

    Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
  }

  [Test]
  public async Task Create_ReturnsBadRequest_WhenProductIdInvalid()
  {
    var service = new Mock<IItemService>();
    var logger = new Mock<ILogger<ItemController>>();

    var controller = new ItemController(service.Object, logger.Object);

    var result = await controller.Create(0, new ItemCreateDto());

    Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
  }

  [Test]
  public async Task Create_ReturnsNotFound_WhenProductMissing()
  {
    var service = new Mock<IItemService>();
    var logger = new Mock<ILogger<ItemController>>();
    service.Setup(s => s.CreateItemAsync(It.IsAny<ItemCreateDto>())).ReturnsAsync((ItemDto?)null);

    var controller = new ItemController(service.Object, logger.Object);

    var result = await controller.Create(1, new ItemCreateDto { Batch = "B1", Quantity = 1, Location = "A" });

    Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task Create_ReturnsBadRequest_OnArgumentException()
  {
    var service = new Mock<IItemService>();
    var logger = new Mock<ILogger<ItemController>>();
    service.Setup(s => s.CreateItemAsync(It.IsAny<ItemCreateDto>()))
      .ThrowsAsync(new ArgumentException("invalid"));

    var controller = new ItemController(service.Object, logger.Object);

    var result = await controller.Create(1, new ItemCreateDto { Batch = "B1", Quantity = 1, Location = "A" });

    Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
  }

  [Test]
  public async Task Create_ReturnsCreated_WhenSuccess()
  {
    var service = new Mock<IItemService>();
    var logger = new Mock<ILogger<ItemController>>();
    var created = new ItemDto { ItemId = 1 };
    service.Setup(s => s.CreateItemAsync(It.IsAny<ItemCreateDto>())).ReturnsAsync(created);

    var controller = new ItemController(service.Object, logger.Object);

    var result = await controller.Create(1, new ItemCreateDto { Batch = "B1", Quantity = 1, Location = "A" });

    Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
  }

  [Test]
  public async Task Update_ReturnsNotFound_WhenMissing()
  {
    var service = new Mock<IItemService>();
    var logger = new Mock<ILogger<ItemController>>();
    service.Setup(s => s.UpdateItemAsync(1, It.IsAny<ItemUpdateDto>())).ReturnsAsync((ItemDto?)null);

    var controller = new ItemController(service.Object, logger.Object);

    var result = await controller.Update(1, new ItemUpdateDto { Batch = "B" });

    Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task Update_ReturnsConflict_WhenInactive()
  {
    var service = new Mock<IItemService>();
    var logger = new Mock<ILogger<ItemController>>();
    service.Setup(s => s.UpdateItemAsync(1, It.IsAny<ItemUpdateDto>()))
      .ThrowsAsync(new InvalidOperationException("inativo"));

    var controller = new ItemController(service.Object, logger.Object);

    var result = await controller.Update(1, new ItemUpdateDto { Batch = "B" });

    Assert.That(result, Is.TypeOf<ConflictObjectResult>());
  }

  [Test]
  public async Task Update_ReturnsBadRequest_OnArgumentException()
  {
    var service = new Mock<IItemService>();
    var logger = new Mock<ILogger<ItemController>>();
    service.Setup(s => s.UpdateItemAsync(1, It.IsAny<ItemUpdateDto>()))
      .ThrowsAsync(new ArgumentException("invalid"));

    var controller = new ItemController(service.Object, logger.Object);

    var result = await controller.Update(1, new ItemUpdateDto { Batch = "B" });

    Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
  }

  [Test]
  public async Task Update_ReturnsNoContent_WhenSuccess()
  {
    var service = new Mock<IItemService>();
    var logger = new Mock<ILogger<ItemController>>();
    service.Setup(s => s.UpdateItemAsync(1, It.IsAny<ItemUpdateDto>())).ReturnsAsync(new ItemDto());

    var controller = new ItemController(service.Object, logger.Object);

    var result = await controller.Update(1, new ItemUpdateDto { Batch = "B" });

    Assert.That(result, Is.TypeOf<NoContentResult>());
  }

  [Test]
  public async Task Inactivate_ReturnsNotFound_WhenMissing()
  {
    var service = new Mock<IItemService>();
    var logger = new Mock<ILogger<ItemController>>();
    service.Setup(s => s.DeleteItemAsync(1)).ReturnsAsync(ItemDeleteResult.NotFound);

    var controller = new ItemController(service.Object, logger.Object);

    var result = await controller.Inactivate(1);

    Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task Inactivate_ReturnsConflict_WhenAlreadyInactive()
  {
    var service = new Mock<IItemService>();
    var logger = new Mock<ILogger<ItemController>>();
    service.Setup(s => s.DeleteItemAsync(1)).ReturnsAsync(ItemDeleteResult.AlreadyInactive);

    var controller = new ItemController(service.Object, logger.Object);

    var result = await controller.Inactivate(1);

    Assert.That(result, Is.TypeOf<ConflictObjectResult>());
  }

  [Test]
  public async Task Inactivate_ReturnsNoContent_WhenSuccess()
  {
    var service = new Mock<IItemService>();
    var logger = new Mock<ILogger<ItemController>>();
    service.Setup(s => s.DeleteItemAsync(1)).ReturnsAsync(ItemDeleteResult.Deactivated);

    var controller = new ItemController(service.Object, logger.Object);

    var result = await controller.Inactivate(1);

    Assert.That(result, Is.TypeOf<NoContentResult>());
  }
}
