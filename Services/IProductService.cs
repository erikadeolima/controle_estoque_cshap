using System.Collections.Generic;
using System.Threading.Tasks;
using controle_estoque_cshap.DTOs;

namespace controle_estoque_cshap.Services;

public interface IProductService
{
  Task<IEnumerable<ProductDto>> GetInactiveAsync();
  Task<ProductDto?> GetByIdAsync(int id);
}
