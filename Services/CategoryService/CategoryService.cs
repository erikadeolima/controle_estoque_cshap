using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using controle_estoque_cshap.DTOs.CategoryDto;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.CategoryRepository;
using controle_estoque_cshap.Utils;

namespace controle_estoque_cshap.Services.CategoryService;

public class CategoryService : ICategoryService
{
  private readonly ICategoryRepository _categoryRepository;

  public CategoryService(ICategoryRepository categoryRepository)
  {
    _categoryRepository = categoryRepository;
  }


  public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
  {
    var categories = await _categoryRepository.GetAllCategoriesAsync();

    return categories.Select(c => new CategoryDto
    {
      CategoryId = c.CategoryId,
      Name = c.Name,
      Description = c.Description,
      CreationDate = c.CreationDate,
      TotalProducts = c.Products.Count
    });
  }

  public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
  {
    var category = await _categoryRepository.GetCategoryByIdAsync(id);

    if (category == null)
      return null;

    return new CategoryDto
    {
      CategoryId = category.CategoryId,
      Name = category.Name,
      Description = category.Description,
      CreationDate = category.CreationDate,
      TotalProducts = category.Products.Count
    };
  }

  public async Task<CategoryDto?> CreateCategoryAsync(CategoryCreateDto dto)
  {
    var normalizedName = NameValidation.NormalizeTitleCase(dto.Name);
    var existing = await _categoryRepository.GetCategoryByNameAsync(normalizedName);
    if (existing != null)
      return null;

    var category = new Category
    {
      Name = normalizedName,
      Description = dto.Description
    };

    await _categoryRepository.CreateCategoryAsync(category);

    return new CategoryDto
    {
      CategoryId = category.CategoryId,
      Name = category.Name,
      Description = category.Description,
      CreationDate = category.CreationDate,
      TotalProducts = 0
    };
  }

  public async Task<CategoryUpdateResult> UpdateCategoryAsync(int id, CategoryUpdateDto dto)
  {
    var existing = await _categoryRepository.GetCategoryByIdForUpdateAsync(id);
    if (existing == null)
      return CategoryUpdateResult.NotFound;

    // Nao altere a CreationDate: ela representa quando a categoria foi criada.
    // Em atualizacoes, mantenha esse valor para preservar o historico.
    if (!string.IsNullOrWhiteSpace(dto.Name))
    {
      var normalizedName = NameValidation.NormalizeTitleCase(dto.Name);
      var nameExists = await _categoryRepository.GetCategoryByNameAsync(normalizedName);
      if (nameExists != null && nameExists.CategoryId != existing.CategoryId)
        return CategoryUpdateResult.Conflict;

      existing.Name = normalizedName;
    }

    if (dto.Description != null)
      existing.Description = dto.Description;

    await _categoryRepository.UpdateCategoryAsync();
    return CategoryUpdateResult.Success;
  }
}
