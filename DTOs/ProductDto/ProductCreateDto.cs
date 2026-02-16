using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using controle_estoque_cshap.DTOs.ItemDto;

namespace controle_estoque_cshap.DTOs.ProductDto
{
    public class ProductCreateDto
    {
        public string Sku { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int MinimumQuantity { get; set; }
        public List<ItemCreateDto> Items { get; set; } = new List<ItemCreateDto>();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Sku))
            {
                yield return new ValidationResult("Sku e obrigatorio.", new[] { nameof(Sku) });
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Name e obrigatorio.", new[] { nameof(Name) });
            }

            if (CategoryId <= 0)
            {
                yield return new ValidationResult("CategoryId deve ser maior que zero.", new[] { nameof(CategoryId) });
            }

            if (MinimumQuantity < 0)
            {
                yield return new ValidationResult("MinimumQuantity deve ser maior ou igual a zero.", new[] { nameof(MinimumQuantity) });
            }
        } 
    }
}
