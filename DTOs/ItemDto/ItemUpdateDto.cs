using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace controle_estoque_cshap.DTOs.ItemDto;

public class ItemUpdateDto : IValidatableObject
{
  public string? Batch { get; set; }

  public int? Quantity { get; set; }

  public DateTime? ExpirationDate { get; set; }

  public string? Location { get; set; }

  public sbyte? Status { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    var hasAnyField = !string.IsNullOrWhiteSpace(Batch)
      || Quantity.HasValue
      || ExpirationDate.HasValue
      || Location != null
      || Status.HasValue;

    if (!hasAnyField)
    {
      yield return new ValidationResult("Informe ao menos um campo para atualizar.");
      yield break;
    }

    if (Batch != null && string.IsNullOrWhiteSpace(Batch))
    {
      yield return new ValidationResult("Batch nao pode ser vazio.", new[] { nameof(Batch) });
    }

    if (Quantity.HasValue && Quantity.Value < 0)
    {
      yield return new ValidationResult("Quantity deve ser maior ou igual a zero.", new[] { nameof(Quantity) });
    }

    if (ExpirationDate.HasValue && ExpirationDate.Value <= DateTime.Now)
    {
      yield return new ValidationResult("ExpirationDate deve ser uma data futura.", new[] { nameof(ExpirationDate) });
    }
  }
}
