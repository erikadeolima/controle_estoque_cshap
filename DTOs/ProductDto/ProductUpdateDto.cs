namespace controle_estoque_cshap.DTOs.ProductDto
{
    public class ProductUpdateDto
{
    public string? Name {get; set;} = string.Empty;
    public int? CategoryId {get; set;}
    public int? MinimumQuantity {get; set;}
}
}
