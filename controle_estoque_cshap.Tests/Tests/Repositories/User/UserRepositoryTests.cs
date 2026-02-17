using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.UserRepository;

namespace controle_estoque_cshap.Tests;

public class UserRepositoryTests
{
  [Test]
  public async Task GetAllAsync_ReturnsAllUsers()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetAllAsync_ReturnsAllUsers));
    context.Users.Add(new User { Name = "Erika", Email = "erika@example.com", Profile = "Admin" });
    context.Users.Add(new User { Name = "João", Email = "joao@example.com", Profile = "User" });
    await context.SaveChangesAsync();

    var repo = new UserRepository(context);

    var result = (await repo.GetAllAsync()).ToList();

    Assert.That(result, Has.Count.EqualTo(2));
  }

  [Test]
  public async Task GetAllAsync_ReturnsEmpty_WhenNoUsers()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetAllAsync_ReturnsEmpty_WhenNoUsers));
    var repo = new UserRepository(context);

    var result = (await repo.GetAllAsync()).ToList();

    Assert.That(result, Is.Empty);
  }

  [Test]
  public async Task GetByIdAsync_ReturnsNull_WhenMissing()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetByIdAsync_ReturnsNull_WhenMissing));
    var repo = new UserRepository(context);

    var result = await repo.GetByIdAsync(99);

    Assert.That(result, Is.Null);
  }

  [Test]
  public async Task GetByIdAsync_ReturnsUser_WhenFound()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetByIdAsync_ReturnsUser_WhenFound));
    var user = new User { Name = "Erika", Email = "erika@example.com", Profile = "Admin" };
    context.Users.Add(user);
    await context.SaveChangesAsync();

    var repo = new UserRepository(context);
    var result = await repo.GetByIdAsync(user.IdUser);

    Assert.That(result, Is.Not.Null);
    Assert.That(result!.Name, Is.EqualTo("Erika"));
  }

  [Test]
  public async Task GetByEmailAsync_ReturnsNull_WhenMissing()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetByEmailAsync_ReturnsNull_WhenMissing));
    var repo = new UserRepository(context);

    var result = await repo.GetByEmailAsync("notfound@example.com");

    Assert.That(result, Is.Null);
  }

  [Test]
  public async Task GetByEmailAsync_ReturnsUser_WhenFound()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetByEmailAsync_ReturnsUser_WhenFound));
    var user = new User { Name = "Erika", Email = "erika@example.com", Profile = "Admin" };
    context.Users.Add(user);
    await context.SaveChangesAsync();

    var repo = new UserRepository(context);
    var result = await repo.GetByEmailAsync("erika@example.com");

    Assert.That(result, Is.Not.Null);
    Assert.That(result!.Name, Is.EqualTo("Erika"));
  }

  [Test]
  public async Task GetByEmailAsync_TrimsEmail()
  {
    await using var context = TestDbContextFactory.Create(nameof(GetByEmailAsync_TrimsEmail));
    var user = new User { Name = "Erika", Email = "erika@example.com", Profile = "Admin" };
    context.Users.Add(user);
    await context.SaveChangesAsync();

    var repo = new UserRepository(context);
    var result = await repo.GetByEmailAsync("  erika@example.com  ");

    Assert.That(result, Is.Not.Null);
  }

  [Test]
  public async Task CreateAsync_PersistsUser()
  {
    await using var context = TestDbContextFactory.Create(nameof(CreateAsync_PersistsUser));
    var repo = new UserRepository(context);
    var user = new User { Name = "Erika", Email = "erika@example.com", Profile = "Admin" };

    var created = await repo.CreateAsync(user);

    Assert.That(context.Users.Count(), Is.EqualTo(1));
    Assert.That(created.IdUser, Is.GreaterThan(0));
  }

  [Test]
  public async Task CreateAsync_AssignsId()
  {
    await using var context = TestDbContextFactory.Create(nameof(CreateAsync_AssignsId));
    var repo = new UserRepository(context);
    var user = new User { Name = "João", Email = "joao@example.com", Profile = "User" };

    var created = await repo.CreateAsync(user);

    Assert.That(created.IdUser, Is.GreaterThan(0));
    Assert.That(created.Name, Is.EqualTo("João"));
  }

  [Test]
  public async Task CreateAsync_MultipleUsers()
  {
    await using var context = TestDbContextFactory.Create(nameof(CreateAsync_MultipleUsers));
    var repo = new UserRepository(context);

    await repo.CreateAsync(new User { Name = "Erika", Email = "erika@example.com", Profile = "Admin" });
    await repo.CreateAsync(new User { Name = "João", Email = "joao@example.com", Profile = "User" });

    Assert.That(context.Users.Count(), Is.EqualTo(2));
  }
}
