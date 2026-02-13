using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace controle_estoque_cshap.DTOs.ItemDto;

public class ItemCreateDto : IValidatableObject
{
  public int ProductId { get; set; }

  public string Batch { get; set; } = string.Empty;

  public int Quantity { get; set; }

  public DateTime? ExpirationDate { get; set; }

  public string? Location { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    if (string.IsNullOrWhiteSpace(Batch))
    {
      yield return new ValidationResult("Batch e obrigatorio.", new[] { nameof(Batch) });
    }

    if (Quantity < 0)
    {
      yield return new ValidationResult("Quantity deve ser maior ou igual a zero.", new[] { nameof(Quantity) });
    }

    if (string.IsNullOrWhiteSpace(Location))
    {
      yield return new ValidationResult("Location e obrigatoria.", new[] { nameof(Location) });
    }

    if (ExpirationDate.HasValue && ExpirationDate.Value <= DateTime.Now)
    {
      yield return new ValidationResult("ExpirationDate deve ser uma data futura.", new[] { nameof(ExpirationDate) });
    }
  }
}