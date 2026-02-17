using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace controle_estoque_cshap.DTOs.UserDto;

public class UserCreateDto : IValidatableObject
{
  public string Name { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string Profile { get; set; } = string.Empty;

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    if (string.IsNullOrWhiteSpace(Name))
    {
      yield return new ValidationResult("Name e obrigatorio.", new[] { nameof(Name) });
    }

    if (string.IsNullOrWhiteSpace(Email))
    {
      yield return new ValidationResult("Email e obrigatorio.", new[] { nameof(Email) });
    }

    if (string.IsNullOrWhiteSpace(Profile))
    {
      yield return new ValidationResult("Profile e obrigatorio.", new[] { nameof(Profile) });
    }
  }
}
