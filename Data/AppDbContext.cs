using Microsoft.EntityFrameworkCore;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Data;

public class AppDbContext : DbContext
{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Movement> Movements { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                base.OnModelCreating(modelBuilder);

                // Configuração de Category
                modelBuilder.Entity<Category>(entity =>
                {
                        entity.HasKey(e => e.Id);

                        entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(255);

                        entity.Property(e => e.Descricao)
                    .HasMaxLength(200);

                        entity.Property(e => e.DataCriacao)
                    .IsRequired();

                        entity.HasMany(e => e.Products)
                    .WithOne(p => p.Category)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
                });

                // Configuração de Product
                modelBuilder.Entity<Product>(entity =>
                {
                        entity.HasKey(e => e.Id);

                        entity.Property(e => e.SKU)
                    .IsRequired()
                    .HasMaxLength(45);

                        entity.HasIndex(e => e.SKU)
                    .IsUnique();

                        entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(200);

                        entity.Property(e => e.Status)
                    .IsRequired()
                    .HasConversion<int>();

                        entity.Property(e => e.QuantidadeMinima)
                    .IsRequired()
                    .HasDefaultValue(0);

                        entity.Property(e => e.DataCriacao)
                    .IsRequired();

                        entity.HasMany(e => e.Items)
                    .WithOne(i => i.Product)
                    .HasForeignKey(i => i.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
                });

                // Configuração de Item
                modelBuilder.Entity<Item>(entity =>
                {
                        entity.HasKey(e => e.Id);

                        entity.Property(e => e.Batch)
                    .HasMaxLength(55);

                        entity.Property(e => e.DataValidade);

                        entity.Property(e => e.Quantidade)
                    .IsRequired();

                        entity.Property(e => e.Localizacao)
                    .HasMaxLength(100);

                        entity.Property(e => e.Status)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(20);

                        entity.Property(e => e.DataCriacao)
                    .IsRequired();

                        entity.HasMany(e => e.Movements)
                    .WithOne(m => m.Item)
                    .HasForeignKey(m => m.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);
                });

                // Configuração de Movement
                modelBuilder.Entity<Movement>(entity =>
                {
                        entity.HasKey(e => e.Id);

                        entity.Property(e => e.Data)
                    .IsRequired();

                        entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(45);

                        entity.Property(e => e.QuantidadeMovimentada)
                    .IsRequired();

                        entity.Property(e => e.QuantidadeAnterior)
                    .IsRequired();

                        entity.Property(e => e.QuantidadeNova)
                    .IsRequired();

                        entity.HasOne(e => e.User)
                    .WithMany(u => u.Movements)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                });

                // Configuração de User
                modelBuilder.Entity<User>(entity =>
                {
                        entity.HasKey(e => e.Id);

                        entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(200);

                        entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                        entity.Property(e => e.Perfil)
                    .IsRequired()
                    .HasMaxLength(50);
                });
        }
}
