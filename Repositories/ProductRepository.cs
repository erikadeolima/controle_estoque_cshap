using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using controle_estoque_cshap.Data;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Repositories;

public class ProductRepository : IProductRepository
{
  private readonly AppDbContext _context;

  public ProductRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<Product>> GetInactiveAsync() // Enumerable<Product> permite retornar uma lista de produtos inativos
  {
    return await _context.Products
        .AsNoTracking() // Melhora o desempenho para consultas somente leitura
        .Include(p => p.Items)
        .Where(p => p.Status == 0)
        .ToListAsync();
  }

  public async Task<Product?> GetByIdAsync(int id) // Product permite retornar um unico produto por id
  {
    return await _context.Products
        .AsNoTracking() // Melhora o desempenho para consultas somente leitura
        .Include(p => p.Items)
        .FirstOrDefaultAsync(p => p.ProductId == id);
  }
   public async Task<List<Product>> GetActiveAsync()
        {
            return await _context.Products
                .Where(p => p.Status == 1)
                .ToListAsync();
        }
}
