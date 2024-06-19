using Microsoft.EntityFrameworkCore;

namespace CoreAndFood.Data.Models
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=LAPTOP-6MJ53BNG\\SQLEXPRESS; database=Food_MVC; integrated security=true");
        }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Başlangıç verisi ekleme
            modelBuilder.Entity<Account>().HasData(new Account
            {
                AccountID = 1,
                UserName = "test",
                Email = "test@gmail.com",
                Password = "test123",
                Role = "Admin"
            });
        }
    }
}
