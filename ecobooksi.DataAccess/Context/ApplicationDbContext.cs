using ecobooksi.Models.Models;
using ecobooksi.Models.Models.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ecobooksi.DataAccess.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
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
        }

        public DbSet<Category> Categories { get; set; }
    }
}
