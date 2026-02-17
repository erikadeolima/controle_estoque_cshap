using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using controle_estoque_cshap.Controllers;
using controle_estoque_cshap.DTOs.UserDto;
using controle_estoque_cshap.Services.UserService;

namespace controle_estoque_cshap.Tests;

public class UserControllerTests
{
  [Test]
  public async Task GetAll_ReturnsOk_WithUsers()
  {
    // Arrange
    var service = new Mock<IUserService>();
    var logger = new Mock<ILogger<UserController>>();
    var users = new List<UserDto>
    {
      new() { IdUser = 1, Name = "Erika", Email = "erika@example.com", Profile = "Admin" },
      new() { IdUser = 2, Name = "João", Email = "joao@example.com", Profile = "User" }
    };
    service.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

    var controller = new UserController(service.Object, logger.Object);

    // Act
    var result = await controller.GetAll();

    // Assert
    Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    var ok = (OkObjectResult)result.Result!;
    Assert.That(ok.Value, Is.EqualTo(users));
    service.Verify(s => s.GetAllAsync(), Times.Once);
  }

  [Test]
  public async Task GetAll_ReturnsInternalServerError_OnException()
  {
    // Arrange
    var service = new Mock<IUserService>();
    var logger = new Mock<ILogger<UserController>>();
    service.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("Database error"));

    var controller = new UserController(service.Object, logger.Object);

    // Act
    var result = await controller.GetAll();

    // Assert
    Assert.That(result.Result, Is.TypeOf<ObjectResult>());
    var error = (ObjectResult)result.Result!;
    Assert.That(error.StatusCode, Is.EqualTo(500));
  }

  [Test]
  public async Task GetById_ReturnsOk_WhenFound()
  {
    // Arrange
    var service = new Mock<IUserService>();
    var logger = new Mock<ILogger<UserController>>();
    var userId = 1;
    var user = new UserDto { IdUser = userId, Name = "Erika", Email = "erika@example.com", Profile = "Admin" };
    service.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync(user);

    var controller = new UserController(service.Object, logger.Object);

    // Act
    var result = await controller.GetById(userId);

    // Assert
    Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    var ok = (OkObjectResult)result.Result!;
    Assert.That(ok.Value, Is.EqualTo(user));
    service.Verify(s => s.GetByIdAsync(userId), Times.Once);
  }

  [Test]
  public async Task GetById_ReturnsNotFound_WhenMissing()
  {
    // Arrange
    var service = new Mock<IUserService>();
    var logger = new Mock<ILogger<UserController>>();
    var userId = 999;
    service.Setup(s => s.GetByIdAsync(userId)).ReturnsAsync((UserDto?)null);

    var controller = new UserController(service.Object, logger.Object);

    // Act
    var result = await controller.GetById(userId);

    // Assert
    Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
    var notFound = (NotFoundObjectResult)result.Result!;
    Assert.That(notFound.StatusCode, Is.EqualTo(404));
    service.Verify(s => s.GetByIdAsync(userId), Times.Once);
  }

  [Test]
  public async Task GetById_ReturnsInternalServerError_OnException()
  {
    // Arrange
    var service = new Mock<IUserService>();
    var logger = new Mock<ILogger<UserController>>();
    var userId = 1;
    service.Setup(s => s.GetByIdAsync(userId)).ThrowsAsync(new Exception("Database error"));

    var controller = new UserController(service.Object, logger.Object);

    // Act
    var result = await controller.GetById(userId);

    // Assert
    Assert.That(result.Result, Is.TypeOf<ObjectResult>());
    var error = (ObjectResult)result.Result!;
    Assert.That(error.StatusCode, Is.EqualTo(500));
  }

  [Test]
  public async Task Create_ReturnsCreatedAtAction_WhenSuccess()
  {
    // Arrange
    var service = new Mock<IUserService>();
    var logger = new Mock<ILogger<UserController>>();
    var createDto = new UserCreateDto { Name = "Erika", Email = "erika@example.com", Profile = "Admin" };
    var createdUser = new UserDto { IdUser = 1, Name = "Erika", Email = "erika@example.com", Profile = "Admin" };
    service.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(createdUser);

    var controller = new UserController(service.Object, logger.Object);

    // Act
    var result = await controller.Create(createDto);

    // Assert
    Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
    var created = (CreatedAtActionResult)result.Result!;
    Assert.That(created.ActionName, Is.EqualTo(nameof(UserController.GetById)));
    Assert.That(created.StatusCode, Is.EqualTo(201));
    Assert.That(created.Value, Is.EqualTo(createdUser));
    service.Verify(s => s.CreateAsync(createDto), Times.Once);
  }

  [Test]
  public async Task Create_ReturnsConflict_WhenEmailExists()
  {
    // Arrange
    var service = new Mock<IUserService>();
    var logger = new Mock<ILogger<UserController>>();
    var createDto = new UserCreateDto { Name = "Erika", Email = "erika@example.com", Profile = "Admin" };
    service.Setup(s => s.CreateAsync(createDto)).ReturnsAsync((UserDto?)null);

    var controller = new UserController(service.Object, logger.Object);

    // Act
    var result = await controller.Create(createDto);

    // Assert
    Assert.That(result.Result, Is.TypeOf<ConflictObjectResult>());
    var conflict = (ConflictObjectResult)result.Result!;
    Assert.That(conflict.StatusCode, Is.EqualTo(409));
    service.Verify(s => s.CreateAsync(createDto), Times.Once);
  }

  [Test]
  public async Task Create_ReturnsBadRequest_WithInvalidData()
  {
    // Arrange
    var service = new Mock<IUserService>();
    var logger = new Mock<ILogger<UserController>>();
    var createDto = new UserCreateDto { Name = "", Email = "", Profile = "" };
    var invalidException = new ArgumentException("Name e obrigatorio.");
    service.Setup(s => s.CreateAsync(createDto)).ThrowsAsync(invalidException);

    var controller = new UserController(service.Object, logger.Object);

    // Act
    var result = await controller.Create(createDto);

    // Assert
    Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
    var badRequest = (BadRequestObjectResult)result.Result!;
    Assert.That(badRequest.StatusCode, Is.EqualTo(400));
    service.Verify(s => s.CreateAsync(createDto), Times.Once);
  }

  [Test]
  public async Task Create_ReturnsInternalServerError_OnException()
  {
    // Arrange
    var service = new Mock<IUserService>();
    var logger = new Mock<ILogger<UserController>>();
    var createDto = new UserCreateDto { Name = "Erika", Email = "erika@example.com", Profile = "Admin" };
    service.Setup(s => s.CreateAsync(createDto)).ThrowsAsync(new Exception("Database error"));

    var controller = new UserController(service.Object, logger.Object);

    // Act
    var result = await controller.Create(createDto);

    // Assert
    Assert.That(result.Result, Is.TypeOf<ObjectResult>());
    var error = (ObjectResult)result.Result!;
    Assert.That(error.StatusCode, Is.EqualTo(500));
    service.Verify(s => s.CreateAsync(createDto), Times.Once);
  }
}
