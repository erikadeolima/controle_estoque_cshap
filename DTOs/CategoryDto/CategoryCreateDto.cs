using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using controle_estoque_cshap.Utils;

namespace controle_estoque_cshap.DTOs.CategoryDto;

public class CategoryCreateDto : IValidatableObject
{
  [Required(ErrorMessage = "Name e obrigatorio.")]
  public string Name { get; set; } = string.Empty;

  public string? Description { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    if (string.IsNullOrWhiteSpace(Name))
      yield break;

    if (NameValidation.HasLeadingOrTrailingSpaces(Name))
      yield return new ValidationResult("Name nao pode ter espacos no inicio ou no fim.");

    if (!NameValidation.ContainsOnlyLettersAndSpaces(Name))
      yield return new ValidationResult("Name deve conter apenas letras.");
  }
}
