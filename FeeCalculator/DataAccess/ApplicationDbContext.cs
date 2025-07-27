using Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<FeeRule> FeeRules => Set<FeeRule>();
        public DbSet<FeeCalculationResult> FeeCalculationResults => Set<FeeCalculationResult>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Amount)
                    .HasPrecision(18, 4); // 18 total digits, 4 after decimal
            });

            modelBuilder.Entity<FeeCalculationResult>(entity =>
            {
                entity.Property(e => e.CalculatedFee)
                    .HasPrecision(18, 4); // 18 total digits, 4 after decimal
            });

            // Optional: Add more configurations if needed (e.g., indexes, relationships)
        }
    }
}
