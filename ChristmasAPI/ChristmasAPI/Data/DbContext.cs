using ChristmasAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChristmasAPI.Data
{
    public class ChristmasDbContext : DbContext
    {
        public ChristmasDbContext(DbContextOptions<ChristmasDbContext> options)
            : base(options)
        {
        }

        // DbSet for LightModel
        public DbSet<LightModel> Lights { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LightModel>()
                .HasKey(l => l.Id);

            modelBuilder.Entity<LightModel>()
                .Property(l => l.X)
                .IsRequired();

            modelBuilder.Entity<LightModel>()
                .Property(l => l.Y)
                .IsRequired();

            modelBuilder.Entity<LightModel>()
                .Property(l => l.Radius)
                .IsRequired();

            modelBuilder.Entity<LightModel>()
                .Property(l => l.Color)
                .HasMaxLength(50); // Adjust length as needed

            modelBuilder.Entity<LightModel>()
                .Property(l => l.Effects)
                .HasMaxLength(100); // Adjust length as needed

            modelBuilder.Entity<LightModel>()
                .Property(l => l.Desc)
                .HasMaxLength(200); // Adjust length as needed

            modelBuilder.Entity<LightModel>()
                .Property(l => l.Ct)
                .HasMaxLength(50); // Adjust length as needed
        }
    }
}