using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;
using controle_estoque_cshap.DTOs.UserDto;

namespace controle_estoque_cshap.Tests;

public class UserDtoValidationTests
{
  [Test]
  public void UserCreateDto_ReturnsError_WhenNameMissing()
  {
    var dto = new UserCreateDto { Email = "test@example.com", Profile = "Admin" };

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "Name e obrigatorio."), Is.True);
  }

  [Test]
  public void UserCreateDto_ReturnsError_WhenEmailMissing()
  {
    var dto = new UserCreateDto { Name = "Erika", Profile = "Admin" };

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "Email e obrigatorio."), Is.True);
  }

  [Test]
  public void UserCreateDto_ReturnsError_WhenProfileMissing()
  {
    var dto = new UserCreateDto { Name = "Erika", Email = "erika@example.com" };

    var results = Validate(dto);

    Assert.That(results.Any(r => r.ErrorMessage == "Profile e obrigatorio."), Is.True);
  }

  [Test]
  public void UserCreateDto_PassesValidation_WhenAllFieldsProvided()
  {
    var dto = new UserCreateDto
    {
      Name = "Erika",
      Email = "erika@example.com",
      Profile = "Admin"
    };

    var results = Validate(dto);

    Assert.That(results, Is.Empty);
  }

  [Test]
  public void UserCreateDto_ReturnsError_WhenMultipleFieldsMissing()
  {
    var dto = new UserCreateDto();

    var results = Validate(dto);

    Assert.That(results, Has.Count.EqualTo(3));
  }

  private static IList<ValidationResult> Validate(object instance)
  {
    var results = new List<ValidationResult>();
    var context = new ValidationContext(instance);
    Validator.TryValidateObject(instance, context, results, true);
    return results;
  }
}
