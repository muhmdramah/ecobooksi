using ecobooksi.Models.Models;
using ecobooksi.Models.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ecobooksi.DataAccess.Context
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // the keys of identity tables are mapped here, so i must write this line
            base.OnModelCreating(builder);

            builder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");

                entity.Property(prop => prop.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(prop => prop.DisplayOrder)
                    .IsRequired();

                entity.HasData(new List<Category>
                {
                    new Category{
                        CategoryId = 1,
                        CategoryName = "Cat One",
                        DisplayOrder = 1
                    },
                    new Category{
                        CategoryId = 2,
                        CategoryName = "Cat Two",
                        DisplayOrder = 2
                    },
                    new Category{
                        CategoryId = 3,
                        CategoryName = "Cat Three",
                        DisplayOrder = 3
                    }
                });
            });

            builder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.Property(prop => prop.Title)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(prop => prop.Description)
                    .IsRequired(false);
                entity.Property(prop => prop.ISBN)
                    .IsRequired();
                entity.Property(prop => prop.Author)
                    .IsRequired();
                entity.Property(prop => prop.ListPrice)
                    .IsRequired();
                entity.Property(prop => prop.Price)
                    .IsRequired();
                entity.Property(prop => prop.PriceFifty)
                    .IsRequired();
                entity.Property(prop => prop.PriceHundred)
                    .IsRequired();

                entity.HasData(new List<Product>
                {
                    new Product{
                        ProductId = 1,
                        Title = "Book One",
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        ISBN = "1234567890",
                        Author = "John Doe",
                        ListPrice = 99.00,
                        Price = 90.00,
                        PriceFifty = 85.00,
                        PriceHundred = 80.00,
                        CategoryId = 1,
                        ImageURL = ""
                    },
                    new Product{
                        ProductId = 2,
                        Title = "Book Two",
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        ISBN = "1234567891",
                        Author = "Jane Doe",
                        ListPrice = 120.00,
                        Price = 100.00,
                        PriceFifty = 90.00,
                        PriceHundred = 80.00,
                        CategoryId = 1,
                        ImageURL = ""
                    },
                    new Product{
                        ProductId = 3,
                        Title = "Book Three",
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        ISBN = "1234567892",
                        Author = "John Smith",
                        ListPrice = 150.00,
                        Price = 130.00,
                        PriceFifty = 120.00,
                        PriceHundred = 100.00,
                        CategoryId = 1,
                        ImageURL = ""
                    }
                });
            });
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
    }
}
