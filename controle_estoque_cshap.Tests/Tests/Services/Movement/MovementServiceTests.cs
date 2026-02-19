using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Assert = Xunit.Assert;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.DTOs.MovementDto;
using controle_estoque_cshap.Repositories.MovementRepository;
using controle_estoque_cshap.Repositories.ItemRepository;
using controle_estoque_cshap.Services.MovementService;


namespace controle_estoque_cshap.Tests.Services;

public class MovementServiceTests
{
    [Fact]
    public async Task GetByItemAsync_ShouldReturnMovementsOfItem()
    {
        // Arrange
        var mockMovementRepository = new Mock<IMovementRepository>();
        var mockItemRepository = new Mock<IItemRepository>();

        var movements = new List<Movement>
        {
            new Movement
            {
                MovementId = 1,
                Date = DateTime.Now,
                Type = "IN",
                QuantityMoved = 10,
                PreviousQuantity = 5,
                NewQuantity = 15,
                ItemId = 1,
                Item = new Item
                {
                    ItemId = 1,
                    Product = new Product
                    {
                        Name = "Produto Teste"
                    }
                },
                UserId = 1,
                User = new User
                {
                    Name = "Usuário Teste"
                }
            }
        };

        mockMovementRepository
            .Setup(r => r.GetByItemAsync(1))
            .ReturnsAsync(movements);

        var service = new MovementService(
            mockMovementRepository.Object,
            mockItemRepository.Object
        );

        // Act
        var result = await service.GetByItemAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        var movement = result[0];
        Assert.Equal(1, movement.ItemId);
        Assert.Equal("Produto Teste", movement.ProductName);
        Assert.Equal("Usuário Teste", movement.UserName);
    }
}

