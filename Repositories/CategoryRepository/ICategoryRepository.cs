using System.Collections.Generic;
using System.Threading.Tasks;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Repositories.CategoryRepository;

public interface ICategoryRepository
{
  Task<IEnumerable<Category>> GetAllCategoriesAsync();
  Task<Category?> GetCategoryByIdAsync(int id);
  Task<Category?> GetCategoryByIdForUpdateAsync(int id);
  Task<Category?> GetCategoryByNameAsync(string name);

  Task CreateCategoryAsync(Category category);

  Task UpdateCategoryAsync();
}
