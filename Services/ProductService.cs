using System.Collections.Generic;
using System.Linq;

public class ProductService
{
    private readonly ProductRepository _repository;

    public ProductService(ProductRepository repository)
    {
        _repository = repository;
    }

    public List<ProductActiveDto> GetActiveProducts()
    {
        var products = _repository.GetActive();

        return products.Select(p => new ProductActiveDto
        {
            ProductId = p.ProductId,
            Name = p.Name
        }).ToList();
    }
}
