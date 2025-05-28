using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrestigeAuction.Models;
using PrestigeAuction.Repository;

namespace PrestigeAuction.Data
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<CountDownTarget> CountDownTargets { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<AuctionOrder> AuctionOrders { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Bid>().ToTable("Bids");
            modelBuilder.Entity<CountDownTarget>().ToTable("CountDownTargets");
            modelBuilder.Entity<ProductImage>().ToTable("ProductImages");
            modelBuilder.Entity<AuctionOrder>().ToTable("AuctionOrders");
            modelBuilder.Entity<ChatMessage>().ToTable("ChatMessages");

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Other", DisplayOrder = 1,},
                new Category { Id = 2, Name = "Computer", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Laptop", DisplayOrder = 3 }
                );
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Title = "Oppo", Description = "Oppo a37", SKU="1234_oppo",StartingPrice=100,CategoryId=2 },
                new Product { Id = 2, Title = "Redmi", Description = "Redmi 9c", SKU= "1234_redmi",StartingPrice=200, CategoryId = 2},
                new Product { Id = 3, Title = "Techno", Description = "Techno spark go", SKU= "1234_techno",StartingPrice =150, CategoryId = 1}
                );
        }
    }
}
 