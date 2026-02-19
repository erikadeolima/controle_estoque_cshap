using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using controle_estoque_cshap.DTOs.CategoryDto;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.CategoryRepository;
using controle_estoque_cshap.Services.CategoryService;

namespace controle_estoque_cshap.Tests;

public class CategoryServiceTests
{
  [Test]
  public async Task GetAllCategoriesAsync_ReturnsMappedDtos()
  {
    var categories = new List<CategoryWithProductCount>
    {
      new(new Category { CategoryId = 1, Name = "Bebidas", Description = "Bebidas frias" }, 3),
      new(new Category { CategoryId = 2, Name = "Lanches", Description = "Lanches quentes" }, 5)
    };

    var repo = new Mock<ICategoryRepository>();
    repo.Setup(r => r.GetAllWithProductCountAsync()).ReturnsAsync(categories);

    var service = new CategoryService(repo.Object);

    var result = (await service.GetAllCategoriesAsync()).ToList();

    Assert.That(result, Has.Count.EqualTo(2));
    Assert.That(result[0].CategoryId, Is.EqualTo(1));
    Assert.That(result[0].Name, Is.EqualTo("Bebidas"));
    Assert.That(result[0].TotalProducts, Is.EqualTo(3));
  }

  [Test]
  public async Task GetCategoryByIdAsync_ReturnsNull_WhenNotFound()
  {
    var repo = new Mock<ICategoryRepository>();
    repo.Setup(r => r.GetByIdWithProductCountAsync(10)).ReturnsAsync((CategoryWithProductCount?)null);

    var service = new CategoryService(repo.Object);

    var result = await service.GetCategoryByIdAsync(10);

    Assert.That(result, Is.Null);
  }

  [Test]
  public async Task CreateCategoryAsync_ReturnsNull_WhenNameExists()
  {
    var repo = new Mock<ICategoryRepository>();
    repo.Setup(r => r.GetCategoryByNameAsync("Pao")).ReturnsAsync(new Category { CategoryId = 1, Name = "Pao" });

    var service = new CategoryService(repo.Object);
    var dto = new CategoryCreateDto { Name = "pao" };

    var result = await service.CreateCategoryAsync(dto);

    Assert.That(result, Is.Null);
    repo.Verify(r => r.CreateCategoryAsync(It.IsAny<Category>()), Times.Never);
  }

  [Test]
  public async Task CreateCategoryAsync_CreatesCategory_WithNormalizedName()
  {
    var repo = new Mock<ICategoryRepository>();
    repo.Setup(r => r.GetCategoryByNameAsync("Pao De Queijo")).ReturnsAsync((Category?)null);

    Category? created = null;
    repo.Setup(r => r.CreateCategoryAsync(It.IsAny<Category>()))
      .Callback<Category>(c => created = c)
      .Returns(Task.CompletedTask);

    var service = new CategoryService(repo.Object);
    var dto = new CategoryCreateDto { Name = "  pao de queijo  ", Description = "Salgados" };

    var result = await service.CreateCategoryAsync(dto);

    Assert.That(result, Is.Not.Null);
    Assert.That(created, Is.Not.Null);
    Assert.That(created!.Name, Is.EqualTo("Pao De Queijo"));
    Assert.That(created.Description, Is.EqualTo("Salgados"));
  }

  [Test]
  public async Task UpdateCategoryAsync_ReturnsNotFound_WhenMissing()
  {
    var repo = new Mock<ICategoryRepository>();
    repo.Setup(r => r.GetCategoryByIdForUpdateAsync(99)).ReturnsAsync((Category?)null);

    var service = new CategoryService(repo.Object);
    var dto = new CategoryUpdateDto { Name = "Lanches" };

    var result = await service.UpdateCategoryAsync(99, dto);

    Assert.That(result, Is.EqualTo(CategoryUpdateResult.NotFound));
  }

  [Test]
  public async Task UpdateCategoryAsync_ReturnsConflict_WhenNameExists()
  {
    var existing = new Category { CategoryId = 1, Name = "Lanches" };
    var repo = new Mock<ICategoryRepository>();
    repo.Setup(r => r.GetCategoryByIdForUpdateAsync(1)).ReturnsAsync(existing);
    repo.Setup(r => r.GetCategoryByNameAsync("Bebidas")).ReturnsAsync(new Category { CategoryId = 2, Name = "Bebidas" });

    var service = new CategoryService(repo.Object);
    var dto = new CategoryUpdateDto { Name = "bebidas" };

    var result = await service.UpdateCategoryAsync(1, dto);

    Assert.That(result, Is.EqualTo(CategoryUpdateResult.Conflict));
  }

  [Test]
  public async Task UpdateCategoryAsync_UpdatesNameAndDescription_WhenValid()
  {
    var existing = new Category { CategoryId = 1, Name = "Lanches", Description = "Antigo" };
    var repo = new Mock<ICategoryRepository>();
    repo.Setup(r => r.GetCategoryByIdForUpdateAsync(1)).ReturnsAsync(existing);
    repo.Setup(r => r.GetCategoryByNameAsync("Lanches Especiais")).ReturnsAsync((Category?)null);
    repo.Setup(r => r.UpdateCategoryAsync()).Returns(Task.CompletedTask);

    var service = new CategoryService(repo.Object);
    var dto = new CategoryUpdateDto { Name = "lanches especiais", Description = "Novo" };

    var result = await service.UpdateCategoryAsync(1, dto);

    Assert.That(result, Is.EqualTo(CategoryUpdateResult.Success));
    Assert.That(existing.Name, Is.EqualTo("Lanches Especiais"));
    Assert.That(existing.Description, Is.EqualTo("Novo"));
    repo.Verify(r => r.UpdateCategoryAsync(), Times.Once);
  }
}
