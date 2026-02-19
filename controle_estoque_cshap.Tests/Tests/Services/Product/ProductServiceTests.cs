using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

using controle_estoque_cshap.DTOs.ProductDto;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.ProductRepository;
using controle_estoque_cshap.Services.ProductService;


namespace controle_estoque_cshap.Tests.Services;

public class ProductServiceTests
{
    private Mock<IProductRepository> _mockRepository;
    private ProductService _service;

    public ProductServiceTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _service = new ProductService(_mockRepository.Object);
    }

    [Fact]
    public void GetActiveProducts_ShouldReturnOnlyActiveProducts()
    {
        // Arrange
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

        _mockRepository
            .Setup(r => r.GetActiveAsync())
            .ReturnsAsync(products);

        // Act
        var result = _service.GetActiveProducts();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetInactiveAsync_ShouldReturnInactiveProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { ProductId = 1, Name = "Inativo", Sku = "SKU3", Status = 0, MinimumQuantity = 2, Items = new List<Item>() }
        };

        _mockRepository.Setup(r => r.GetInactiveAsync()).ReturnsAsync(products);

        // Act
        var result = await _service.GetInactiveAsync();

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProductDto_WhenFound()
    {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Produto", Sku = "SKU1", Status = 1, MinimumQuantity = 5, Items = new List<Item>() };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.ProductId);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetBySkuAsync_ShouldReturnProductDto_WhenFound()
    {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Produto", Sku = "SKU1", Status = 1, MinimumQuantity = 5, Items = new List<Item>() };
        _mockRepository.Setup(r => r.GetBySkuAsync("SKU1")).ReturnsAsync(product);

        // Act
        var result = await _service.GetBySkuAsync("SKU1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("SKU1", result.Sku);
    }

    [Fact]
    public async Task GetBySkuAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetBySkuAsync("INVALID")).ReturnsAsync((Product)null);

        // Act
        var result = await _service.GetBySkuAsync("INVALID");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenSkuIsEmpty()
    {
        // Arrange
        var dto = new ProductCreateDto { Sku = "", Name = "Produto", MinimumQuantity = 5, CategoryId = 1 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var dto = new ProductCreateDto { Sku = "SKU1", Name = "", MinimumQuantity = 5, CategoryId = 1 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenMinimumQuantityIsZero()
    {
        // Arrange
        var dto = new ProductCreateDto { Sku = "SKU1", Name = "Produto", MinimumQuantity = 0, CategoryId = 1 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateProduct_WhenValid()
    {
        // Arrange
        var dto = new ProductCreateDto { Sku = "SKU1", Name = "Produto", MinimumQuantity = 5, CategoryId = 1 };
        var product = new Product { ProductId = 1, Sku = "SKU1", Name = "Produto", Status = 1, MinimumQuantity = 5, Items = new List<Item>() };
        
        _mockRepository.Setup(r => r.GetBySkuAsync(It.IsAny<string>())).ReturnsAsync((Product)null);
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(product);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("SKU1", result.Sku);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenProductNotFound()
    {
        // Arrange
        var dto = new ProductUpdateDto { Name = "Novo Nome" };
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product)null);

        // Act
        var result = await _service.UpdateAsync(999, dto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Produto", Sku = "SKU1", Status = 1, MinimumQuantity = 5, Items = new List<Item>() };
        var dto = new ProductUpdateDto { Name = "" };
        
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(1, dto));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProduct_WhenValid()
    {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Produto", Sku = "SKU1", Status = 1, MinimumQuantity = 5, Items = new List<Item>() };
        var dto = new ProductUpdateDto { Name = "Novo Nome", CategoryId = 2 };
        
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Novo Nome", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenProductNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product)null);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenProductDeleted()
    {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Produto", Sku = "SKU1", Status = 1, MinimumQuantity = 5, Items = new List<Item>() };
        
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetLowStockAsync_ShouldReturnProductsWithLowStock()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { ProductId = 1, Name = "Baixo Estoque", Sku = "SKU1", Status = 1, MinimumQuantity = 10, Items = new List<Item>() }
        };
        
        _mockRepository.Setup(r => r.GetLowStockAsync()).ReturnsAsync(products);

        // Act
        var result = await _service.GetLowStockAsync();

        // Assert
        Assert.Single(result);
    }
}
