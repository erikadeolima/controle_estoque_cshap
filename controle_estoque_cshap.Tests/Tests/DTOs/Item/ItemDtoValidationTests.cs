using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;
using controle_estoque_cshap.DTOs.ItemDto;

namespace controle_estoque_cshap.Tests;

public class ItemDtoValidationTests
{
  [Test]
  public void ItemCreateDto_ReturnsError_WhenBatchMissing()
  {
    var dto = new ItemCreateDto { Batch = "", Quantity = 1, Location = "A" };

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "Batch e obrigatorio."), Is.True);
  }

  [Test]
  public void ItemCreateDto_ReturnsError_WhenQuantityNegative()
  {
    var dto = new ItemCreateDto { Batch = "B", Quantity = -1, Location = "A" };

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "Quantity deve ser maior ou igual a zero."), Is.True);
  }

  [Test]
  public void ItemCreateDto_ReturnsError_WhenLocationMissing()
  {
    var dto = new ItemCreateDto { Batch = "B", Quantity = 1, Location = "" };

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "Location e obrigatoria."), Is.True);
  }

  [Test]
  public void ItemCreateDto_ReturnsError_WhenExpirationPast()
  {
    var dto = new ItemCreateDto { Batch = "B", Quantity = 1, Location = "A", ExpirationDate = System.DateTime.Now.AddDays(-1) };

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "ExpirationDate deve ser uma data futura."), Is.True);
  }

  [Test]
  public void ItemUpdateDto_ReturnsError_WhenNoFields()
  {
    var dto = new ItemUpdateDto();

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "Informe ao menos um campo para atualizar."), Is.True);
  }

  [Test]
  public void ItemUpdateDto_ReturnsError_WhenBatchEmpty()
  {
    var dto = new ItemUpdateDto { Batch = "", Location = "A" };

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "Batch nao pode ser vazio."), Is.True);
  }

  [Test]
  public void ItemUpdateDto_ReturnsError_WhenLocationEmpty()
  {
    var dto = new ItemUpdateDto { Location = "" };

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "Location nao pode ser vazia."), Is.True);
  }

  [Test]
  public void ItemUpdateDto_ReturnsError_WhenExpirationPast()
  {
    var dto = new ItemUpdateDto { ExpirationDate = System.DateTime.Now.AddDays(-1) };

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "ExpirationDate deve ser uma data futura."), Is.True);
  }

  private static IList<ValidationResult> Validate(object instance)
  {
    var results = new List<ValidationResult>();
    var context = new ValidationContext(instance);
    Validator.TryValidateObject(instance, context, results, true);
    return results;
  }
}
