using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using controle_estoque_cshap.DTOs.ProductDto;
using controle_estoque_cshap.Repositories.ProductRepository;
using controle_estoque_cshap.Models;


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
  public async Task<ProductDto?> GetBySkuAsync(string sku)
    {
        var product = await _productRepository.GetBySkuAsync(sku);

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
  public async Task<ProductDto?> CreateAsync(ProductCreateDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.Sku))
        throw new ArgumentException("Sku é obrigatório");

    if (string.IsNullOrWhiteSpace(dto.Name))
        throw new ArgumentException("Name é obrigatório");

    var product = new Product
    {
        Sku = dto.Sku.Trim(),
        Name = dto.Name.Trim(),
        CategoryId = dto.CategoryId,
        MinimumQuantity = dto.MinimumQuantity,
        Status = 1,
        CreationDate = DateTime.Now
    };

    await _productRepository.CreateAsync(product);

    var created = await _productRepository.GetByIdAsync(product.ProductId);

    if (created == null)
        return null;

    return new ProductDto
    {
        ProductId = created.ProductId,
        Sku = created.Sku,
        Name = created.Name,
        Status = created.Status,
        MinimumQuantity = created.MinimumQuantity,
        CreationDate = created.CreationDate,
        CategoryId = created.CategoryId,
        QuantityTotal = created.Items.Sum(i => i.Quantity)
    };
}
  public async Task<ProductDto?> UpdateAsync(int id, ProductUpdateDto dto)
{
    var product = await _productRepository.GetByIdAsync(id);

    if (product == null || product.Status == 0)
        return null;

    if (dto.Name != null)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Name não pode ser vazio.");

        product.Name = dto.Name.Trim();
    }

    if (dto.CategoryId.HasValue)
        product.CategoryId = dto.CategoryId.Value;

    if (dto.MinimumQuantity.HasValue)
        product.MinimumQuantity = dto.MinimumQuantity.Value;

    await _productRepository.UpdateAsync(product);

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
public async Task<bool> DeleteAsync(int id)
{
    var product = await _productRepository.GetByIdAsync(id);

    if (product == null || product.Status == 0)
        return false;

    product.Status = 0;

    await _productRepository.UpdateAsync(product);

    return true;
}


}
