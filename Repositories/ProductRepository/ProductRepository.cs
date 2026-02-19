using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using controle_estoque_cshap.Data;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Repositories.ProductRepository;

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
  public async Task<Product?> GetBySkuAsync(string sku)
  {
    return await _context.Products
        .AsNoTracking()
        .Include(p => p.Items)
        .FirstOrDefaultAsync(p => p.Sku == sku);
  }
  public async Task CreateAsync(Product product)
  {
    await _context.Products.AddAsync(product);
    await _context.SaveChangesAsync();
  }
  public async Task UpdateAsync(Product product)
  {
    _context.Products.Update(product);
    await _context.SaveChangesAsync();
  }

public async Task<List<Product>> GetLowStockAsync()
{
    return await _context.Products
        .AsNoTracking()
        .Include(p => p.Items)
        .Where(p =>
            p.Status == 1 &&
            p.MinimumQuantity != null &&
            p.Items.Sum(i => i.Quantity) < p.MinimumQuantity
        )
        .ToListAsync();
}


   
}
  