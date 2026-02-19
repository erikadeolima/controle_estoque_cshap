using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Assert = Xunit.Assert;
using controle_estoque_cshap.Data;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.MovementRepository;

namespace controle_estoque_cshap.Tests.Repositories;

public class MovementRepositoryTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetByItemAsync_ShouldReturnOnlyMovementsFromItem()
    {
        // Arrange
        var context = CreateContext();

        var user = new User
        {
            IdUser = 1,
            Name = "Usuário Teste",
            Email = "teste@testando.com",
            Profile = "Admin"
        };

        var product = new Product
        {
            ProductId = 1,
            Name = "Produto Teste",
            Sku = "SKU-01",
            CategoryId = 1,
            MinimumQuantity = 1,
            Status = 1
        };

        var item1 = new Item
        {
            ItemId = 1,
            Quantity = 10,
            ProductId = 1,
            Product = product,
            Batch = "Lote-01"
        };

        var item2 = new Item
        {
            ItemId = 2,
            Quantity = 5,
            ProductId = 1,
            Product = product,
            Batch = "Lote-02"
        };

        context.Users.Add(user);
        context.Products.Add(product);
        context.Items.AddRange(item1, item2);

        var movements = new List<Movement>
        {
            new Movement
            {
                MovementId = 1,
                ItemId = 1,
                Item = item1,
                UserId = 1,
                User = user,
                QuantityMoved = 5,
                Type = "IN",
                Date = DateTime.Now
            },
            new Movement
            {
                MovementId = 2,
                ItemId = 2,
                Item = item2,
                UserId = 1,
                User = user,
                QuantityMoved = 3,
                Type = "OUT",
                Date = DateTime.Now
            }
        };

        context.Movements.AddRange(movements);
        await context.SaveChangesAsync();

        var repository = new MovementRepository(context);

        // Act
        var result = await repository.GetByItemAsync(1);

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].ItemId);
    }

    [Fact]
    public async Task GetByPeriodAsync_ShouldReturnOnlyMovementsInPeriod()
    {
        // Arrange
        var context = CreateContext();

        var user = new User
        {
            IdUser = 1,
            Name = "Usuário Teste",
            Email = "teste@testando.com",
            Profile = "Admin"
        };

        var product = new Product
        {
            ProductId = 1,
            Name = "Produto Teste",
            Sku = "SKU-01",
            CategoryId = 1,
            MinimumQuantity = 1,
            Status = 1
        };

        var item = new Item
        {
            ItemId = 1,
            Quantity = 10,
            ProductId = 1,
            Product = product,
            Batch = "Lote-01"
        };

        context.Users.Add(user);
        context.Products.Add(product);
        context.Items.Add(item);

        var start = new DateTime(2025, 1, 10);
        var end = new DateTime(2025, 1, 20);

        var movements = new List<Movement>
        {
            new Movement
            {
                MovementId = 1,
                ItemId = 1,
                Item = item,
                UserId = 1,
                User = user,
                QuantityMoved = 5,
                Type = "IN",
                Date = new DateTime(2025, 1, 15)
            },
            new Movement
            {
                MovementId = 2,
                ItemId = 1,
                Item = item,
                UserId = 1,
                User = user,
                QuantityMoved = 3,
                Type = "OUT",
                Date = new DateTime(2025, 1, 25)
            }
        };

        context.Movements.AddRange(movements);
        await context.SaveChangesAsync();

        var repository = new MovementRepository(context);

        // Act
        var result = await repository.GetByPeriodAsync(start, end);

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result[0].MovementId);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistMovement()
    {
        // Arrange
        var context = CreateContext();

        var user = new User
        {
            IdUser = 1,
            Name = "Usuário Teste",
            Email = "teste@testando.com",
            Profile = "Admin"
        };

        var product = new Product
        {
            ProductId = 1,
            Name = "Produto Teste",
            Sku = "SKU-01",
            CategoryId = 1,
            MinimumQuantity = 1,
            Status = 1
        };

        var item = new Item
        {
            ItemId = 1,
            Quantity = 10,
            ProductId = 1,
            Product = product,
            Batch = "Lote-01"
        };

        context.Users.Add(user);
        context.Products.Add(product);
        context.Items.Add(item);
        await context.SaveChangesAsync();

        var repository = new MovementRepository(context);

        var movement = new Movement
        {
            ItemId = 1,
            UserId = 1,
            QuantityMoved = 2,
            Type = "IN",
            Date = new DateTime(2025, 2, 1),
            PreviousQuantity = 10,
            NewQuantity = 12
        };

        // Act
        await repository.AddAsync(movement);

        // Assert
        var saved = await context.Movements.SingleAsync();
        Assert.Equal(1, saved.ItemId);
        Assert.Equal(1, saved.UserId);
        Assert.Equal(2, saved.QuantityMoved);
        Assert.Equal("IN", saved.Type);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnMovementWithIncludes()
    {
        // Arrange
        var context = CreateContext();

        var user = new User
        {
            IdUser = 1,
            Name = "Usuário Teste",
            Email = "teste@testando.com",
            Profile = "Admin"
        };

        var product = new Product
        {
            ProductId = 1,
            Name = "Produto Teste",
            Sku = "SKU-01",
            CategoryId = 1,
            MinimumQuantity = 1,
            Status = 1
        };

        var item = new Item
        {
            ItemId = 1,
            Quantity = 10,
            ProductId = 1,
            Product = product,
            Batch = "Lote-01"
        };

        context.Users.Add(user);
        context.Products.Add(product);
        context.Items.Add(item);
        await context.SaveChangesAsync();

        var repository = new MovementRepository(context);

        var movement = new Movement
        {
            ItemId = 1,
            UserId = 1,
            QuantityMoved = 3,
            Type = "OUT",
            Date = new DateTime(2025, 2, 2),
            PreviousQuantity = 10,
            NewQuantity = 7
        };

        // Act
        var created = await repository.CreateAsync(movement);

        // Assert
        Assert.True(created.MovementId > 0);
        Assert.Equal(1, created.ItemId);
        Assert.Equal(1, created.UserId);
        Assert.NotNull(created.Item);
        Assert.NotNull(created.User);
        Assert.NotNull(created.Item.Product);
    }

}
