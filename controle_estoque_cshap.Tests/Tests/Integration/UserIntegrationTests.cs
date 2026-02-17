using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using controle_estoque_cshap.Controllers;
using controle_estoque_cshap.DTOs.UserDto;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.UserRepository;
using controle_estoque_cshap.Services.UserService;

namespace controle_estoque_cshap.Tests;

public class UserIntegrationTests
{
  [Test]
  public async Task CreateUser_GetAll_GetById_FullFlow()
  {
    await using var context = TestDbContextFactory.Create(nameof(CreateUser_GetAll_GetById_FullFlow));
    var repo = new UserRepository(context);
    var service = new UserService(repo);

    // Create users via service
    var createDto = new UserCreateDto
    {
      Name = "Erika",
      Email = "erika@example.com",
      Profile = "Admin"
    };
    var created = await service.CreateAsync(createDto);

    // Verify creation
    Assert.That(created, Is.Not.Null);

    // Get all users
    var allUsers = (await service.GetAllAsync()).ToList();
    Assert.That(allUsers, Has.Count.EqualTo(1));

    // Get by id
    var byId = await service.GetByIdAsync(created!.IdUser);
    Assert.That(byId, Is.Not.Null);
    Assert.That(byId!.Email, Is.EqualTo("erika@example.com"));
  }

  [Test]
  public async Task Controller_CreateUser_FlowAsync()
  {
    await using var context = TestDbContextFactory.Create(nameof(Controller_CreateUser_FlowAsync));
    var repo = new UserRepository(context);
    var service = new UserService(repo);

    // Create via controller (mocked logger)
    var controller = new UserController(service, null!);
    var createDto = new UserCreateDto
    {
      Name = "João",
      Email = "joao@example.com",
      Profile = "User"
    };

    var result = await controller.Create(createDto);

    // Verify response
    Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
    var created = (CreatedAtActionResult)result.Result!;
    Assert.That(created.StatusCode, Is.EqualTo(201));
  }

  [Test]
  public async Task Controller_GetAllUsers_ReturnsOkAsync()
  {
    await using var context = TestDbContextFactory.Create(nameof(Controller_GetAllUsers_ReturnsOkAsync));
    var repo = new UserRepository(context);
    var service = new UserService(repo);

    // Create some users
    await repo.CreateAsync(new User { Name = "Erika", Email = "erika@example.com", Profile = "Admin" });
    await repo.CreateAsync(new User { Name = "João", Email = "joao@example.com", Profile = "User" });

    var controller = new UserController(service, null!);

    var result = await controller.GetAll();

    Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    var ok = (OkObjectResult)result.Result!;
    var users = ((IEnumerable<UserDto>)ok.Value!).ToList();
    Assert.That(users, Has.Count.EqualTo(2));
  }

  [Test]
  public async Task Controller_GetUserById_ReturnsOkAsync()
  {
    await using var context = TestDbContextFactory.Create(nameof(Controller_GetUserById_ReturnsOkAsync));
    var repo = new UserRepository(context);
    var service = new UserService(repo);

    var user = new User { Name = "Erika", Email = "erika@example.com", Profile = "Admin" };
    await repo.CreateAsync(user);

    var controller = new UserController(service, null!);

    var result = await controller.GetById(user.IdUser);

    Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    var ok = (OkObjectResult)result.Result!;
    var retrieved = (UserDto)ok.Value!;
    Assert.That(retrieved.Name, Is.EqualTo("Erika"));
  }

  [Test]
  public async Task ValidatingEmailUniqueness_DuplicateEmailReturnsConflict()
  {
    await using var context = TestDbContextFactory.Create(nameof(ValidatingEmailUniqueness_DuplicateEmailReturnsConflict));
    var repo = new UserRepository(context);
    var service = new UserService(repo);

    // Create first user
    var firstDto = new UserCreateDto
    {
      Name = "Erika",
      Email = "erika@example.com",
      Profile = "Admin"
    };
    await service.CreateAsync(firstDto);

    // Try to create with same email
    var secondDto = new UserCreateDto
    {
      Name = "Erika Silva",
      Email = "erika@example.com",
      Profile = "User"
    };
    var result = await service.CreateAsync(secondDto);

    Assert.That(result, Is.Null);
  }

  [Test]
  public async Task SeveralUsersCanBeCreated_WithDifferentEmails()
  {
    await using var context = TestDbContextFactory.Create(nameof(SeveralUsersCanBeCreated_WithDifferentEmails));
    var repo = new UserRepository(context);
    var service = new UserService(repo);

    var users = new[]
    {
      new UserCreateDto { Name = "Erika", Email = "erika@example.com", Profile = "Admin" },
      new UserCreateDto { Name = "João", Email = "joao@example.com", Profile = "User" },
      new UserCreateDto { Name = "Maria", Email = "maria@example.com", Profile = "User" }
    };

    var created = new List<UserDto>();
    foreach (var dto in users)
    {
      var result = await service.CreateAsync(dto);
      Assert.That(result, Is.Not.Null);
      created.Add(result!);
    }

    var all = (await service.GetAllAsync()).ToList();
    Assert.That(all, Has.Count.EqualTo(3));
  }
}
