using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;
using controle_estoque_cshap.DTOs.CategoryDto;

namespace controle_estoque_cshap.Tests;

public class CategoryDtoValidationTests
{
  [Test]
  public void CategoryCreateDto_ReturnsError_WhenNameMissing()
  {
    var dto = new CategoryCreateDto { Name = "" };

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "Name e obrigatorio."), Is.True);
  }

  [Test]
  public void CategoryCreateDto_ReturnsError_WhenNameHasSpaces()
  {
    var dto = new CategoryCreateDto { Name = " Lanches" };

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "Name nao pode ter espacos no inicio ou no fim."), Is.True);
  }

  [Test]
  public void CategoryCreateDto_ReturnsError_WhenNameHasNumbers()
  {
    var dto = new CategoryCreateDto { Name = "Lanche1" };

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "Name deve conter apenas letras e espacos."), Is.True);
  }

  [Test]
  public void CategoryUpdateDto_ReturnsError_WhenNoFieldsProvided()
  {
    var dto = new CategoryUpdateDto();

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "Informe Name ou Description para atualizar."), Is.True);
  }

  [Test]
  public void CategoryUpdateDto_ReturnsError_WhenNameHasSpaces()
  {
    var dto = new CategoryUpdateDto { Name = " Lanches" };

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "Name nao pode ter espacos no inicio ou no fim."), Is.True);
  }

  [Test]
  public void CategoryUpdateDto_ReturnsError_WhenNameHasNumbers()
  {
    var dto = new CategoryUpdateDto { Name = "Lanche1" };

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "Name deve conter apenas letras e espacos."), Is.True);
  }

  [Test]
  public void CategoryUpdateDto_PassesValidation_WhenValidNameProvided()
  {
    var dto = new CategoryUpdateDto { Name = "Lanches" };

    var results = Validate(dto);

    Assert.That(results, Is.Empty);
  }

  private static IList<ValidationResult> Validate(object instance)
  {
    var results = new List<ValidationResult>();
    var context = new ValidationContext(instance);
    Validator.TryValidateObject(instance, context, results, true);
    return results;
  }
}
