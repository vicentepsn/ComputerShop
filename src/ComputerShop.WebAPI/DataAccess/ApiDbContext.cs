using ComputerShop.WebAPI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComputerShop.WebAPI.DataAccess
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LaptopConfiguration>()
                .HasKey(_ => new { _.LaptopId, _.ConfigurationId });

            modelBuilder.Entity<LaptopConfiguration>()
                .HasOne(_ => _.Laptop)
                .WithMany(_ => _.LaptopConfigurations)
                .HasForeignKey(_ => _.LaptopId);

            modelBuilder.Entity<LaptopConfiguration>()
                .HasOne(_ => _.Configuration)
                .WithMany(_ => _.LaptopConfigurations)
                .HasForeignKey(_ => _.ConfigurationId);

            modelBuilder.Entity<BasketItemConfiguration>()
                .HasKey(_ => new { _.BasketItemId, _.ConfigurationId });

            modelBuilder.Entity<BasketItemConfiguration>()
                .HasOne(_ => _.BasketItem)
                .WithMany(_ => _.BasketItemConfigurations)
                .HasForeignKey(_ => _.BasketItemId);

            modelBuilder.Entity<BasketItemConfiguration>()
                .HasOne(_ => _.Configuration)
                .WithMany(_ => _.BasketItemConfigurations)
                .HasForeignKey(_ => _.ConfigurationId);
        }

        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<Laptop> Laptops { get; set; }
        public DbSet<LaptopConfiguration> LaptopsConfigurations { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

    }
}
