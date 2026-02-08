using System.Collections.Generic;
using System.Threading.Tasks;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Repositories.CategoryRepository;

public interface ICategoryRepository
{
  Task<Category?> GetCategoryByIdForUpdateAsync(int id);
  Task<IEnumerable<CategoryWithProductCount>> GetAllWithProductCountAsync();
  Task<CategoryWithProductCount?> GetByIdWithProductCountAsync(int id);
  Task<Category?> GetCategoryByNameAsync(string name);

  Task CreateCategoryAsync(Category category);

  Task UpdateCategoryAsync();
}
