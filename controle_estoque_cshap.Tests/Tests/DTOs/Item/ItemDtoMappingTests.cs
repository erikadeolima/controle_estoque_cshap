using NUnit.Framework;
using controle_estoque_cshap.DTOs.ItemDto;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Tests;

public class ItemDtoMappingTests
{
  [Test]
  public void FromModel_MapsFields()
  {
    var item = new Item
    {
      ItemId = 1,
      ProductId = 2,
      Batch = "B1",
      Quantity = 3,
      Location = "A",
      Status = 1,
      Product = new Product { Name = "Produto" }
    };

    var dto = ItemDto.FromModel(item);

    Assert.That(dto.ItemId, Is.EqualTo(1));
    Assert.That(dto.ProductId, Is.EqualTo(2));
    Assert.That(dto.ProductName, Is.EqualTo("Produto"));
    Assert.That(dto.Batch, Is.EqualTo("B1"));
    Assert.That(dto.Quantity, Is.EqualTo(3));
    Assert.That(dto.Location, Is.EqualTo("A"));
    Assert.That(dto.Status, Is.EqualTo(1));
  }
}
