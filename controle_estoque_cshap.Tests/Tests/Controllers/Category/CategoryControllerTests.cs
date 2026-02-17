using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using controle_estoque_cshap.Controllers;
using controle_estoque_cshap.DTOs.CategoryDto;
using controle_estoque_cshap.Services.CategoryService;

namespace controle_estoque_cshap.Tests;

public class CategoryControllerTests
{
  [Test]
  public async Task GetAll_ReturnsOk_WithCategories()
  {
    var service = new Mock<ICategoryService>();
    var logger = new Mock<ILogger<CategoryController>>();
    var categories = new List<CategoryDto> { new() { CategoryId = 1, Name = "Bebidas" } };
    service.Setup(s => s.GetAllCategoriesAsync()).ReturnsAsync(categories);

    var controller = new CategoryController(service.Object, logger.Object);

    var result = await controller.GetAll();

    Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    var ok = (OkObjectResult)result.Result!;
    Assert.That(ok.Value, Is.EqualTo(categories));
  }

  [Test]
  public async Task GetById_ReturnsNotFound_WhenMissing()
  {
    var service = new Mock<ICategoryService>();
    var logger = new Mock<ILogger<CategoryController>>();
    service.Setup(s => s.GetCategoryByIdAsync(10)).ReturnsAsync((CategoryDto?)null);

    var controller = new CategoryController(service.Object, logger.Object);

    var result = await controller.GetById(10);

    Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task GetById_ReturnsOk_WhenFound()
  {
    var service = new Mock<ICategoryService>();
    var logger = new Mock<ILogger<CategoryController>>();
    var dto = new CategoryDto { CategoryId = 2, Name = "Lanches" };
    service.Setup(s => s.GetCategoryByIdAsync(2)).ReturnsAsync(dto);

    var controller = new CategoryController(service.Object, logger.Object);

    var result = await controller.GetById(2);

    Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    var ok = (OkObjectResult)result.Result!;
    Assert.That(ok.Value, Is.EqualTo(dto));
  }

  [Test]
  public async Task Create_ReturnsCreated_WhenSuccess()
  {
    var service = new Mock<ICategoryService>();
    var logger = new Mock<ILogger<CategoryController>>();
    var dto = new CategoryCreateDto { Name = "Bebidas" };
    var created = new CategoryDto { CategoryId = 1, Name = "Bebidas" };
    service.Setup(s => s.CreateCategoryAsync(dto)).ReturnsAsync(created);

    var controller = new CategoryController(service.Object, logger.Object);

    var result = await controller.Create(dto);

    Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
    var createdResult = (CreatedAtActionResult)result.Result!;
    Assert.That(createdResult.Value, Is.EqualTo(created));
  }

  [Test]
  public async Task Create_ReturnsConflict_WhenNameExists()
  {
    var service = new Mock<ICategoryService>();
    var logger = new Mock<ILogger<CategoryController>>();
    var dto = new CategoryCreateDto { Name = "Bebidas" };
    service.Setup(s => s.CreateCategoryAsync(dto)).ReturnsAsync((CategoryDto?)null);

    var controller = new CategoryController(service.Object, logger.Object);

    var result = await controller.Create(dto);

    Assert.That(result.Result, Is.TypeOf<ConflictObjectResult>());
  }

  [Test]
  public async Task Update_ReturnsNoContent_WhenSuccess()
  {
    var service = new Mock<ICategoryService>();
    var logger = new Mock<ILogger<CategoryController>>();
    var dto = new CategoryUpdateDto { Name = "Lanches" };
    service.Setup(s => s.UpdateCategoryAsync(1, dto)).ReturnsAsync(CategoryUpdateResult.Success);

    var controller = new CategoryController(service.Object, logger.Object);

    var result = await controller.Update(1, dto);

    Assert.That(result, Is.TypeOf<NoContentResult>());
  }

  [Test]
  public async Task Update_ReturnsNotFound_WhenMissing()
  {
    var service = new Mock<ICategoryService>();
    var logger = new Mock<ILogger<CategoryController>>();
    var dto = new CategoryUpdateDto { Name = "Lanches" };
    service.Setup(s => s.UpdateCategoryAsync(1, dto)).ReturnsAsync(CategoryUpdateResult.NotFound);

    var controller = new CategoryController(service.Object, logger.Object);

    var result = await controller.Update(1, dto);

    Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task Update_ReturnsConflict_WhenNameExists()
  {
    var service = new Mock<ICategoryService>();
    var logger = new Mock<ILogger<CategoryController>>();
    var dto = new CategoryUpdateDto { Name = "Bebidas" };
    service.Setup(s => s.UpdateCategoryAsync(1, dto)).ReturnsAsync(CategoryUpdateResult.Conflict);

    var controller = new CategoryController(service.Object, logger.Object);

    var result = await controller.Update(1, dto);

    Assert.That(result, Is.TypeOf<ConflictObjectResult>());
  }

  [Test]
  public async Task GetAll_ReturnsServerError_OnException()
  {
    var service = new Mock<ICategoryService>();
    var logger = new Mock<ILogger<CategoryController>>();
    service.Setup(s => s.GetAllCategoriesAsync()).ThrowsAsync(new InvalidOperationException("boom"));

    var controller = new CategoryController(service.Object, logger.Object);

    var result = await controller.GetAll();

    Assert.That(result.Result, Is.TypeOf<ObjectResult>());
    var error = (ObjectResult)result.Result!;
    Assert.That(error.StatusCode, Is.EqualTo(500));
  }
}
