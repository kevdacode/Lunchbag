using Lunchbag.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lunchbag.API.DbContexts
{
    public class LunchbagContext : DbContext
    {

        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public LunchbagContext(DbContextOptions<LunchbagContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>()
                .HasData(
                    new Category() { Id = 1, Name = "Snacks" },
                    new Category() { Id = 2, Name = "Drinks" },
                    new Category() { Id = 3, Name = "Sweets" },
                    new Category() { Id = 4, Name = "Vegan" },
                    new Category() { Id = 5, Name = "Special" }
                );

            modelBuilder.Entity<Product>()
                .HasData(
                    // Snacks
                    new Product() { Id = 1, Name = "Brezel", CategoryId = 1, Price = 1.50m },
                    new Product() { Id = 2, Name = "Wurstsemmel", CategoryId = 1, Price = 3.20m },
                    new Product() { Id = 3, Name = "Burger", CategoryId = 1, Price = 2.80m },
                    new Product() { Id = 4, Name = "Kartoffelchips", CategoryId = 1, Price = 1.80m },

                    // Drinks
                    new Product() { Id = 5, Name = "Apfelschorle", CategoryId = 2, Price = 2.00m },
                    new Product() { Id = 6, Name = "Radler", CategoryId = 2, Price = 3.50m },
                    new Product() { Id = 7, Name = "Limonade", CategoryId = 2, Price = 3.00m },
                    new Product() { Id = 8, Name = "Kaffee", CategoryId = 2, Price = 2.20m },

                    // Sweets
                    new Product() { Id = 9, Name = "Schwarzwälder Kirschtorte", CategoryId = 3, Price = 4.50m },
                    new Product() { Id = 10, Name = "Lebkuchen", CategoryId = 3, Price = 2.00m },
                    new Product() { Id = 11, Name = "Muffin", CategoryId = 3, Price = 3.00m },
                    new Product() { Id = 12, Name = "Donut", CategoryId = 3, Price = 1.50m },

                    // Vegan
                    new Product() { Id = 13, Name = "Vegane Linsensuppe", CategoryId = 4, Price = 3.80m },
                    new Product() { Id = 14, Name = "Tofu-Schnitzel", CategoryId = 4, Price = 5.00m },
                    new Product() { Id = 15, Name = "Gemüseburger", CategoryId = 4, Price = 4.20m },
                    new Product() { Id = 16, Name = "Quinoa-Salat", CategoryId = 4, Price = 3.90m },

                    // Special
                    new Product() { Id = 17, Name = "Sauerbraten", CategoryId = 5, Price = 8.90m },
                    new Product() { Id = 18, Name = "Weißwurst mit Brezel", CategoryId = 5, Price = 6.50m },
                    new Product() { Id = 19, Name = "Schweinshaxe", CategoryId = 5, Price = 9.50m },
                    new Product() { Id = 20, Name = "Kartoffelsalat", CategoryId = 5, Price = 3.50m }
                );

            base.OnModelCreating(modelBuilder);
        }

    }
}
