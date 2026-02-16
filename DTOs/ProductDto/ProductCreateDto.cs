using System;
using System.Collections.Generic;
using controle_estoque_cshap.DTOs.ItemDto;

namespace controle_estoque_cshap.DTOs.ProductDto
{
    public class ProductCreateDto
    {
        public string Sku { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int? MinimumQuantity { get; set; }

        // 👇 aqui entra o que você descreveu no áudio
        public List<ItemCreateDto>? Items { get; set; }
    }
}
