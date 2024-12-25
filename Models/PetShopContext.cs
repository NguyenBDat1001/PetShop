using Microsoft.EntityFrameworkCore;

namespace PetShop.Models
{
    public class PetShopContext : DbContext
    {
        public PetShopContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Banner> Banners { get; set; }

        public DbSet<Feature> Features { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<AdminUser> AdminUsers { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Type> Types { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Member> Members { get; set; }
    }
}