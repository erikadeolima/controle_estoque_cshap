using System.Collections.Generic;
using System.Threading.Tasks;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Repositories;

public interface IProductRepository
{
  Task<IEnumerable<Product>> GetInactiveAsync();
}
