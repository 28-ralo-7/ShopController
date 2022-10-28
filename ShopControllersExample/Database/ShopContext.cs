using Microsoft.EntityFrameworkCore;

namespace ShopControllersExample.Database
{
    public class ShopContext:DbContext
    {
        public DbSet<Goods> Goods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStructure> orderStructures { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }

        public ShopContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=ShoppingDB.db");
        }
    }
}
