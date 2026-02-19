using System;
using NUnit.Framework;
using controle_estoque_cshap.DTOs.ItemDto;

namespace controle_estoque_cshap.Tests;

public class ExpirationReportDtoTests
{
  [Test]
  public void ExpirationReportDto_CanSetAndGetAllProperties()
  {
    var expirationDate = DateTime.Now.AddDays(10);
    var dto = new ExpirationReportDto
    {
      ItemId = 1,
      Batch = "B001",
      Quantity = 50,
      Location = "Rack A1",
      ExpirationDate = expirationDate,
      ProductName = "Suco de Laranja",
      ProductSku = "SKU123",
      CategoryName = "Bebidas",
      Status = "PRÓXIMO A VENCER",
      DaysUntilExpiration = 10
    };

    Assert.That(dto.ItemId, Is.EqualTo(1));
    Assert.That(dto.Batch, Is.EqualTo("B001"));
    Assert.That(dto.Quantity, Is.EqualTo(50));
    Assert.That(dto.Location, Is.EqualTo("Rack A1"));
    Assert.That(dto.ExpirationDate, Is.EqualTo(expirationDate));
    Assert.That(dto.ProductName, Is.EqualTo("Suco de Laranja"));
    Assert.That(dto.ProductSku, Is.EqualTo("SKU123"));
    Assert.That(dto.CategoryName, Is.EqualTo("Bebidas"));
    Assert.That(dto.Status, Is.EqualTo("PRÓXIMO A VENCER"));
    Assert.That(dto.DaysUntilExpiration, Is.EqualTo(10));
  }
}
