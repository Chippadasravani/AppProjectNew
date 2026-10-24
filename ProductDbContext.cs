using System.Data.Entity;
using AppProjectNew.Models;

namespace AppProjectNew.Data
{
    public class ProductDbContext : DbContext
    {
        static ProductDbContext()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ProductDbContext>());
        }

        public ProductDbContext()
            : base("name=ProductDbContext")
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var product = modelBuilder.Entity<Product>();

            product.ToTable("Products");
            product.Property(item => item.Name).IsRequired().HasMaxLength(120);
            product.Property(item => item.Sku).IsRequired().HasMaxLength(40);
            product.Property(item => item.Description).HasMaxLength(1000);
            product.Property(item => item.Price).HasPrecision(18, 2);
            product.Property(item => item.CreatedUtc).IsRequired();
            product.HasIndex(item => item.Sku).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}