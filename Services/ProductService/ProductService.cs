using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using controle_estoque_cshap.DTOs.ProductDto;
using controle_estoque_cshap.Repositories.ProductRepository;

namespace controle_estoque_cshap.Services.ProductService;

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

  public async Task<ProductDto?> GetByIdAsync(int id)
  {
    var product = await _productRepository.GetByIdAsync(id);

    if (product == null)
      return null;

    return new ProductDto
    {
      ProductId = product.ProductId,
      Sku = product.Sku,
      Name = product.Name,
      Status = product.Status,
      MinimumQuantity = product.MinimumQuantity,
      CreationDate = product.CreationDate,
      CategoryId = product.CategoryId,
      QuantityTotal = product.Items.Sum(i => i.Quantity)
    };
  }
  public List<ProductActiveDto> GetActiveProducts()
    {
        var products = _productRepository.GetActiveAsync().Result;

        return products.Select(p => new ProductActiveDto
        {
            ProductId = p.ProductId,
            Name = p.Name
        }).ToList();
    }
}
