using Microsoft.EntityFrameworkCore;
using SmartEnergyAPI.Models;

namespace SmartEnergyAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<EnergyProduct> EnergyProducts { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<PriceHistory> PriceHistories { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // İlişkiler
            modelBuilder.Entity<Trade>()
                .HasOne(t => t.User)
                .WithMany(u => u.Trades)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Trade>()
                .HasOne(t => t.EnergyProduct)
                .WithMany(e => e.Trades)
                .HasForeignKey(t => t.EnergyProductId);

            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.User)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.EnergyProduct)
                .WithMany()
                .HasForeignKey(p => p.EnergyProductId);

            modelBuilder.Entity<PriceHistory>()
                .HasOne(ph => ph.EnergyProduct)
                .WithMany(e => e.PriceHistories)
                .HasForeignKey(ph => ph.EnergyProductId);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId);

            // Decimal ayarları
            modelBuilder.Entity<User>()
                .Property(u => u.Balance)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Trade>()
                .Property(t => t.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Trade>()
                .Property(t => t.Total)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Trade>()
                .Property(t => t.Quantity)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Portfolio>()
                .Property(p => p.AverageBuyPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Portfolio>()
                .Property(p => p.Quantity)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<EnergyProduct>()
                .Property(e => e.CurrentPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<EnergyProduct>()
                .Property(e => e.PreviousPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PriceHistory>()
                .Property(ph => ph.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
