using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using controle_estoque_cshap.DTOs.UserDto;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.UserRepository;
using controle_estoque_cshap.Services.UserService;

namespace controle_estoque_cshap.Tests;

public class UserServiceTests
{
  [Test]
  public async Task GetAllAsync_ReturnsMappedDtos()
  {
    var users = new List<User>
    {
      new User { IdUser = 1, Name = "Erika", Email = "erika@example.com", Profile = "Admin" },
      new User { IdUser = 2, Name = "João", Email = "joao@example.com", Profile = "User" }
    };

    var repo = new Mock<IUserRepository>();
    repo.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

    var service = new UserService(repo.Object);

    var result = (await service.GetAllAsync()).ToList();

    Assert.That(result, Has.Count.EqualTo(2));
    Assert.That(result[0].IdUser, Is.EqualTo(1));
    Assert.That(result[0].Name, Is.EqualTo("Erika"));
    Assert.That(result[1].Profile, Is.EqualTo("User"));
  }

  [Test]
  public async Task GetAllAsync_ReturnsEmpty_WhenNoUsers()
  {
    var repo = new Mock<IUserRepository>();
    repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());

    var service = new UserService(repo.Object);

    var result = (await service.GetAllAsync()).ToList();

    Assert.That(result, Is.Empty);
    repo.Verify(r => r.GetAllAsync(), Times.Once);
  }

  [Test]
  public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
  {
    var repo = new Mock<IUserRepository>();
    repo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

    var service = new UserService(repo.Object);

    var result = await service.GetByIdAsync(99);

    Assert.That(result, Is.Null);
    repo.Verify(r => r.GetByIdAsync(99), Times.Once);
  }

  [Test]
  public async Task GetByIdAsync_ReturnsMappedDto_WhenFound()
  {
    var user = new User { IdUser = 1, Name = "Erika", Email = "erika@example.com", Profile = "Admin" };
    var repo = new Mock<IUserRepository>();
    repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

    var service = new UserService(repo.Object);

    var result = await service.GetByIdAsync(1);

    Assert.That(result, Is.Not.Null);
    Assert.That(result!.IdUser, Is.EqualTo(1));
    Assert.That(result.Name, Is.EqualTo("Erika"));
  }

  [Test]
  public void CreateAsync_ThrowsArgumentException_WhenNameEmpty()
  {
    var repo = new Mock<IUserRepository>();
    var service = new UserService(repo.Object);
    var dto = new UserCreateDto { Name = "", Email = "test@example.com", Profile = "Admin" };

    var ex = Assert.ThrowsAsync<System.ArgumentException>(async () => await service.CreateAsync(dto));
    Assert.That(ex!.Message, Contains.Substring("Name e obrigatorio"));
  }

  [Test]
  public void CreateAsync_ThrowsArgumentException_WhenEmailEmpty()
  {
    var repo = new Mock<IUserRepository>();
    var service = new UserService(repo.Object);
    var dto = new UserCreateDto { Name = "Erika", Email = "", Profile = "Admin" };

    var ex = Assert.ThrowsAsync<System.ArgumentException>(async () => await service.CreateAsync(dto));
    Assert.That(ex!.Message, Contains.Substring("Email e obrigatorio"));
  }

  [Test]
  public void CreateAsync_ThrowsArgumentException_WhenEmailInvalid()
  {
    var repo = new Mock<IUserRepository>();
    var service = new UserService(repo.Object);
    var dto = new UserCreateDto { Name = "Erika", Email = "invalid-email", Profile = "Admin" };

    var ex = Assert.ThrowsAsync<System.ArgumentException>(async () => await service.CreateAsync(dto));
    Assert.That(ex!.Message, Contains.Substring("Email invalido"));
  }

  [Test]
  public void CreateAsync_ThrowsArgumentException_WhenProfileEmpty()
  {
    var repo = new Mock<IUserRepository>();
    var service = new UserService(repo.Object);
    var dto = new UserCreateDto { Name = "Erika", Email = "erika@example.com", Profile = "" };

    var ex = Assert.ThrowsAsync<System.ArgumentException>(async () => await service.CreateAsync(dto));
    Assert.That(ex!.Message, Contains.Substring("Profile e obrigatorio"));
  }

  [Test]
  public async Task CreateAsync_ReturnsNull_WhenEmailExists()
  {
    var repo = new Mock<IUserRepository>();
    repo.Setup(r => r.GetByEmailAsync("erika@example.com"))
      .ReturnsAsync(new User { IdUser = 1, Name = "Erika", Email = "erika@example.com", Profile = "Admin" });

    var service = new UserService(repo.Object);
    var dto = new UserCreateDto { Name = "Erika", Email = "erika@example.com", Profile = "Admin" };

    var result = await service.CreateAsync(dto);

    Assert.That(result, Is.Null);
    repo.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Never);
  }

  [Test]
  public async Task CreateAsync_CreatesUser_WithTrimmedData()
  {
    var repo = new Mock<IUserRepository>();
    repo.Setup(r => r.GetByEmailAsync("erika@example.com")).ReturnsAsync((User?)null);

    User? created = null;
    repo.Setup(r => r.CreateAsync(It.IsAny<User>()))
      .Callback<User>(u => created = u)
      .ReturnsAsync((User u) => u);

    var service = new UserService(repo.Object);
    var dto = new UserCreateDto
    {
      Name = "  Erika  ",
      Email = "  erika@example.com  ",
      Profile = "  Admin  "
    };

    var result = await service.CreateAsync(dto);

    Assert.That(result, Is.Not.Null);
    Assert.That(created, Is.Not.Null);
    Assert.That(created!.Name, Is.EqualTo("Erika"));
    Assert.That(created.Email, Is.EqualTo("erika@example.com"));
    Assert.That(created.Profile, Is.EqualTo("Admin"));
    repo.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Once);
  }

  [Test]
  public async Task CreateAsync_CreatesUser_WithValidData()
  {
    var repo = new Mock<IUserRepository>();
    repo.Setup(r => r.GetByEmailAsync("joao@example.com")).ReturnsAsync((User?)null);
    repo.Setup(r => r.CreateAsync(It.IsAny<User>()))
      .ReturnsAsync((User u) => u);

    var service = new UserService(repo.Object);
    var dto = new UserCreateDto { Name = "João", Email = "joao@example.com", Profile = "User" };

    var result = await service.CreateAsync(dto);

    Assert.That(result, Is.Not.Null);
    Assert.That(result!.Name, Is.EqualTo("João"));
    Assert.That(result.Email, Is.EqualTo("joao@example.com"));
    Assert.That(result.Profile, Is.EqualTo("User"));
  }
}
