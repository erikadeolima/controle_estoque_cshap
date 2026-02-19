using System.Collections.Generic;
using System.Threading.Tasks;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Repositories.ProductRepository;

public interface IProductRepository
{
  Task<IEnumerable<Product>> GetInactiveAsync();
  Task<Product?> GetByIdAsync(int id);
  Task<List<Product>> GetActiveAsync();
  Task<Product?> GetBySkuAsync(string sku);
  Task<List<Product>> GetLowStockAsync();
  Task CreateAsync(Product product);
  Task UpdateAsync(Product product);

  
  

}
