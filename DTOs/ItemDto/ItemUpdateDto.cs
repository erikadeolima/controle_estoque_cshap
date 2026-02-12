using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace controle_estoque_cshap.DTOs.ItemDto;

public class ItemUpdateDto : IValidatableObject
{
  public string? Batch { get; set; }

  public DateTime? ExpirationDate { get; set; }

  public string? Location { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    var hasAnyField = !string.IsNullOrWhiteSpace(Batch)
      || ExpirationDate.HasValue
      || Location != null;

    if (!hasAnyField)
    {
      yield return new ValidationResult("Informe ao menos um campo para atualizar.");
      yield break;
    }

    if (Batch != null && string.IsNullOrWhiteSpace(Batch))
    {
      yield return new ValidationResult("Batch nao pode ser vazio.", new[] { nameof(Batch) });
    }

    if (ExpirationDate.HasValue && ExpirationDate.Value <= DateTime.Now)
    {
      yield return new ValidationResult("ExpirationDate deve ser uma data futura.", new[] { nameof(ExpirationDate) });
    }

    if (Location != null && string.IsNullOrWhiteSpace(Location))
    {
      yield return new ValidationResult("Location nao pode ser vazia.", new[] { nameof(Location) });
    }
  }
}
