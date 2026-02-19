using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.ProductRepository;
using controle_estoque_cshap.Services.ProductService;


namespace controle_estoque_cshap.Tests.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task GetActiveProducts_ShouldReturnOnlyActiveProducts()
    {
        // Arrange
        var mockRepository = new Mock<IProductRepository>();

        var products = new List<Product>
        {
            new Product
            {
                ProductId = 1,
                Name = "Produto 1",
                Sku = "SKU1",
                Status = 1,
                MinimumQuantity = 5,
                Items = new List<Item>()
            },
            new Product
            {
                ProductId = 2,
                Name = "Produto 2",
                Sku = "SKU2",
                Status = 1,
                MinimumQuantity = 3,
                Items = new List<Item>()
            }
        };

        mockRepository
            .Setup(r => r.GetActiveAsync())
            .ReturnsAsync(products);

        var service = new ProductService(mockRepository.Object);

        // Act
        var result = service.GetActiveProducts();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        
    }
}
