using System.Collections.Generic;
using System.Threading.Tasks;
using controle_estoque_cshap.DTOs.CategoryDto;

namespace controle_estoque_cshap.Services.CategoryService;

public interface ICategoryService
{
  Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
  Task<CategoryDto?> GetCategoryByIdAsync(int id);
  Task<CategoryDto?> CreateCategoryAsync(CategoryCreateDto dto);
  Task<CategoryUpdateResult> UpdateCategoryAsync(int id, CategoryUpdateDto dto);
}
