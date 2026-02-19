using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using controle_estoque_cshap.Controllers;
using controle_estoque_cshap.DTOs.MovementDto;
using controle_estoque_cshap.Services.MovementService;

namespace controle_estoque_cshap.Tests.Controllers.Movement;

public class MovementControllerTests
{
    private readonly Mock<IMovementService> _movementServiceMock;
    private readonly MovementController _controller;

    public MovementControllerTests()
    {
        _movementServiceMock = new Mock<IMovementService>();
        _controller = new MovementController(_movementServiceMock.Object);
    }

    [Fact]
    public async Task GetByItem_ReturnsOk_WhenMovementsExist()
    {
        // Arrange
        var movements = new List<MovementDto>
        {
            new MovementDto
            {
                MovementId = 1,
                ItemId = 1,
                ProductName = "Produto Teste",
                UserName = "Usuário Teste",
                QuantityMoved = 5,
                PreviousQuantity = 10,
                NewQuantity = 15,
                Type = "IN",
                Date = DateTime.Now
            }
        };

        _movementServiceMock
            .Setup(s => s.GetByItemAsync(1))
            .ReturnsAsync(movements);

        // Act
        var result = await _controller.GetByItem(1);

        // Assert
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var value = Xunit.Assert.IsType<List<MovementDto>>(okResult.Value);
        Xunit.Assert.Single(value); 
    }

    [Fact]
    public async Task GetByItem_ReturnsNotFound_WhenNoMovementsExist()
    {
        // Arrange
        _movementServiceMock
            .Setup(s => s.GetByItemAsync(1))
            .ReturnsAsync(new List<MovementDto>());

        // Act
        var result = await _controller.GetByItem(1);

        // Assert
        Xunit.Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByPeriod_ReturnsOk_WhenMovementsExist()
    {
        // Arrange
        var start = DateTime.Today.AddDays(-5);
        var end = DateTime.Today;

        var movements = new List<MovementDto>
        {
            new MovementDto
            {
                MovementId = 1,
                ItemId = 1,
                ProductName = "Produto",
                UserName = "Usuário",
                QuantityMoved = 2,
                PreviousQuantity = 3,
                NewQuantity = 5,
                Type = "OUT",
                Date = DateTime.Now
            }
        };

        _movementServiceMock
            .Setup(s => s.GetByPeriodAsync(start, end))
            .ReturnsAsync(movements);

        // Act
        var result = await _controller.GetByPeriod(start, end);

        // Assert
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        var value = Xunit.Assert.IsType<List<MovementDto>>(okResult.Value);
        Xunit.Assert.Single(value);
    }

    [Fact]
    public async Task GetByPeriod_ReturnsNotFound_WhenNoMovementsExist()
    {
        // Arrange
        var start = DateTime.Today.AddDays(-5);
        var end = DateTime.Today;

        _movementServiceMock
            .Setup(s => s.GetByPeriodAsync(start, end))
            .ReturnsAsync(new List<MovementDto>());

        // Act
        var result = await _controller.GetByPeriod(start, end);

        // Assert
        Xunit.Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsOk_WhenSuccess()
    {
        // Arrange
        var dto = new CreateMovementDto
        {
            ItemId = 1,
            Quantity = 3,
            Type = "IN",
            UserId = 1
        };

        var resultDto = new MovementDto
        {
            MovementId = 1,
            ItemId = 1,
            QuantityMoved = 3,
            Type = "IN",
            Date = DateTime.Now,
            ProductName = "Produto",
            UserName = "Usuário"
        };

        _movementServiceMock
            .Setup(s => s.CreateAsync(dto))
            .ReturnsAsync(resultDto);

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
        Xunit.Assert.IsType<MovementDto>(okResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenExceptionIsThrown()
    {
        // Arrange
        var dto = new CreateMovementDto
        {
            ItemId = 1,
            Quantity = 3,
            Type = "IN",
            UserId = 1
        };

        _movementServiceMock
            .Setup(s => s.CreateAsync(dto))
            .ThrowsAsync(new Exception("Erro"));

        // Act
        var result = await _controller.Create(dto);

        // Assert
        Xunit.Assert.IsType<BadRequestObjectResult>(result);
    }
}
