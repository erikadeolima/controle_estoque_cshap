using Microsoft.EntityFrameworkCore;
using controle_estoque_cshap.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext com MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, serverVersion)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Endpoint de teste Database First
app.MapGet("/api/test-db", async (AppDbContext db) => 
{
    var totalCategories = await db.Categories.CountAsync();
    var totalProducts = await db.Products.CountAsync();
    var totalUsers = await db.Users.CountAsync();
    
    var firstProduct = await db.Products
        .Include(p => p.Category)
        .FirstOrDefaultAsync();
    
    return Results.Ok(new {
        success = true,
        message = "Database First funcionou!",
        data = new {
            totalCategories,
            totalProducts,
            totalUsers,
            sampleProduct = firstProduct != null ? new {
                firstProduct.Name,
                firstProduct.Sku,
                Category = firstProduct.Category.Name
            } : null
        }
    });
});

app.Run();
