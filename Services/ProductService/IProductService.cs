using System.Collections.Generic;
using System.Threading.Tasks;
using controle_estoque_cshap.DTOs.ProductDto;

namespace controle_estoque_cshap.Services.ProductService;

public interface IProductService
{
  Task<IEnumerable<ProductDto>> GetInactiveAsync();
  Task<ProductDto?> GetByIdAsync(int id);
  List<ProductActiveDto> GetActiveProducts();
  Task<ProductDto?> GetBySkuAsync(string sku);

  Task<ProductDto?> CreateAsync(ProductCreateDto dto);
  Task<ProductDto?> UpdateAsync(int id, ProductUpdateDto dto);
  Task<bool> DeleteAsync(int id);
  Task<List<ProductDto>> GetLowStockAsync();




}
