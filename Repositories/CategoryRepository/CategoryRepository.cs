using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using controle_estoque_cshap.Data;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Repositories.CategoryRepository;

public class CategoryRepository : ICategoryRepository
{
  private readonly AppDbContext _context;

  public CategoryRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
  {
    return await _context.Categories
        .AsNoTracking() // Melhora o desempenho para consultas somente leitura
        .Include(c => c.Products) // Inclui os produtos relacionados à categoria
        .ToListAsync();
  }

  public async Task<Category?> GetCategoryByIdAsync(int id)
  {
    return await _context.Categories
        .AsNoTracking() // Melhora o desempenho para consultas somente leitura
        .Include(c => c.Products) // Inclui os produtos relacionados à categoria
        .FirstOrDefaultAsync(c => c.CategoryId == id);
  }

  public async Task<Category?> GetCategoryByIdForUpdateAsync(int id)
  {
    return await _context.Categories
        .FirstOrDefaultAsync(c => c.CategoryId == id);
  }

  public async Task<Category?> GetCategoryByNameAsync(string name)
  {
    var normalizedName = name.Trim().ToLower();
    return await _context.Categories
        .AsNoTracking()
      .FirstOrDefaultAsync(c => c.Name.ToLower().Trim() == normalizedName);
  }

  public async Task CreateCategoryAsync(Category category)
  {
    _context.Categories.Add(category);
    await _context.SaveChangesAsync();
  }

  public async Task UpdateCategoryAsync()
  {
    await _context.SaveChangesAsync();
  }
}