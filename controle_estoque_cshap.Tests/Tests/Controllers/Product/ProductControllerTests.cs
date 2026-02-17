using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using controle_estoque_cshap.Controllers;
using controle_estoque_cshap.DTOs.ProductDto;
using controle_estoque_cshap.Services.ProductService;

namespace controle_estoque_cshap.Tests;

public class ProductControllerTests
{
  [Test]
  public async Task GetInactive_ReturnsOk_WhenFound()
  {
    var service = new Mock<IProductService>();
    var products = new List<ProductDto> { new() { ProductId = 1, Name = "Inativo", Status = 0 } };
    service.Setup(s => s.GetInactiveAsync()).ReturnsAsync(products);

    var controller = new ProductController(service.Object);

    var result = await controller.GetInactive();

    Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
  }

  [Test]
  public async Task GetInactive_ReturnsNotFound_WhenNull()
  {
    var service = new Mock<IProductService>();
    service.Setup(s => s.GetInactiveAsync()).ReturnsAsync((IEnumerable<ProductDto>)null!);

    var controller = new ProductController(service.Object);

    var result = await controller.GetInactive();

    Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task GetInactive_ReturnsServerError_OnException()
  {
    var service = new Mock<IProductService>();
    service.Setup(s => s.GetInactiveAsync()).ThrowsAsync(new InvalidOperationException("boom"));

    var controller = new ProductController(service.Object);

    var result = await controller.GetInactive();

    Assert.That(result.Result, Is.TypeOf<ObjectResult>());
    var error = (ObjectResult)result.Result!;
    Assert.That(error.StatusCode, Is.EqualTo(500));
  }

  [Test]
  public async Task GetById_ReturnsNotFound_WhenMissing()
  {
    var service = new Mock<IProductService>();
    service.Setup(s => s.GetByIdAsync(10)).ReturnsAsync((ProductDto?)null);

    var controller = new ProductController(service.Object);

    var result = await controller.GetById(10);

    Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task GetById_ReturnsNotFound_WhenInactive()
  {
    var service = new Mock<IProductService>();
    service.Setup(s => s.GetByIdAsync(2)).ReturnsAsync(new ProductDto { ProductId = 2, Status = 0 });

    var controller = new ProductController(service.Object);

    var result = await controller.GetById(2);

    Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public async Task GetById_ReturnsOk_WhenActive()
  {
    var service = new Mock<IProductService>();
    var dto = new ProductDto { ProductId = 3, Status = 1 };
    service.Setup(s => s.GetByIdAsync(3)).ReturnsAsync(dto);

    var controller = new ProductController(service.Object);

    var result = await controller.GetById(3);

    Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
  }

  [Test]
  public void GetActiveProducts_ReturnsNotFound_WhenEmpty()
  {
    var service = new Mock<IProductService>();
    service.Setup(s => s.GetActiveProducts()).Returns(new List<ProductActiveDto>());

    var controller = new ProductController(service.Object);

    var result = controller.GetActiveProducts();

    Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
  }

  [Test]
  public void GetActiveProducts_ReturnsOk_WhenFound()
  {
    var service = new Mock<IProductService>();
    service.Setup(s => s.GetActiveProducts()).Returns(new List<ProductActiveDto>
    {
      new() { ProductId = 1, Name = "Ativo" }
    });

    var controller = new ProductController(service.Object);

    var result = controller.GetActiveProducts();

    Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
  }

  [Test]
  public void GetActiveProducts_ReturnsServerError_OnException()
  {
    var service = new Mock<IProductService>();
    service.Setup(s => s.GetActiveProducts()).Throws(new InvalidOperationException("boom"));

    var controller = new ProductController(service.Object);

    var result = controller.GetActiveProducts();

    Assert.That(result.Result, Is.TypeOf<ObjectResult>());
    var error = (ObjectResult)result.Result!;
    Assert.That(error.StatusCode, Is.EqualTo(500));
  }
}
