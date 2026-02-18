using Microsoft.EntityFrameworkCore;
using controle_estoque_cshap.Data;

using controle_estoque_cshap.Repositories.ProductRepository;
using controle_estoque_cshap.Services.ProductService;

using controle_estoque_cshap.Repositories.CategoryRepository;
using controle_estoque_cshap.Services.CategoryService;

using controle_estoque_cshap.Repositories.ItemRepository;
using controle_estoque_cshap.Services.ItemService;

using controle_estoque_cshap.Repositories.UserRepository;
using controle_estoque_cshap.Services.UserService;

using controle_estoque_cshap.Repositories.MovementRepository;
using controle_estoque_cshap.Services.MovementService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ======================
// Dependency Injection
// ======================

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IItemService, ItemService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IMovementRepository, MovementRepository>();
builder.Services.AddScoped<IMovementService, MovementService>();


// ======================
// DbContext
// ======================

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, serverVersion)
);

var app = builder.Build();


// ======================
// Pipeline
// ======================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}
