using NUnit.Framework;
using controle_estoque_cshap.DTOs.ItemDto;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Tests;

public class ItemDtoFromModelTests
{
  [Test]
  public void FromModel_UsesEmptyProductName_WhenNull()
  {
    var item = new Item { ItemId = 1, ProductId = 1, Batch = "B", Quantity = 1 };

    var dto = ItemDto.FromModel(item);

    Assert.That(dto.ProductName, Is.EqualTo(string.Empty));
  }
}
