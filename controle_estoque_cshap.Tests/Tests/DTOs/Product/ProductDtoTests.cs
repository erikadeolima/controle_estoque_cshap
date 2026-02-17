using NUnit.Framework;
using controle_estoque_cshap.DTOs.ProductDto;

namespace controle_estoque_cshap.Tests;

public class ProductDtoTests
{
  [Test]
  public void ProductActiveDto_AllowsSettingProperties()
  {
    var dto = new ProductActiveDto { ProductId = 1, Name = "Ativo" };

    Assert.That(dto.ProductId, Is.EqualTo(1));
    Assert.That(dto.Name, Is.EqualTo("Ativo"));
  }
}
