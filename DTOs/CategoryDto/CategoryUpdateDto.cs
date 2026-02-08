using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using controle_estoque_cshap.Utils;

namespace controle_estoque_cshap.DTOs.CategoryDto;

public class CategoryUpdateDto : IValidatableObject
{
  public string? Name { get; set; }

  public string? Description { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    var hasName = !string.IsNullOrWhiteSpace(Name);
    var hasDescription = !string.IsNullOrWhiteSpace(Description);
    if (!hasName && !hasDescription)
      yield return new ValidationResult("Informe Name ou Description para atualizar.");

    if (hasName)
    {
      if (NameValidation.HasLeadingOrTrailingSpaces(Name!))
        yield return new ValidationResult("Name nao pode ter espacos no inicio ou no fim.");

      if (!NameValidation.ContainsOnlyLettersAndSpaces(Name!))
        yield return new ValidationResult("Name deve conter apenas letras e espacos.");
    }
  }
}
