using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Repositories.CategoryRepository;

public record CategoryWithProductCount(Category Category, int TotalProducts);
