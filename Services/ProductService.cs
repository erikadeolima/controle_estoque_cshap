using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using controle_estoque_cshap.DTOs;
using controle_estoque_cshap.Repositories;

namespace controle_estoque_cshap.Services;

public class ProductService : IProductService
{
  private readonly IProductRepository _productRepository;

  public ProductService(IProductRepository productRepository)
  {
    _productRepository = productRepository;
  }

  public async Task<IEnumerable<ProductDto>> GetInactiveAsync()
  {
    var products = await _productRepository.GetInactiveAsync();

    return products.Select(p => new ProductDto
    {
      ProductId = p.ProductId,
      Sku = p.Sku,
      Name = p.Name,
      Status = p.Status,
      MinimumQuantity = p.MinimumQuantity,
      CreationDate = p.CreationDate,
      CategoryId = p.CategoryId,
      QuantityTotal = p.Items.Sum(i => i.Quantity)
    });
  }
}
