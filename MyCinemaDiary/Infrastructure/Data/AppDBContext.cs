using Npgsql;
using Microsoft.EntityFrameworkCore;
using MyCinemaDiary.Domain.Entities;

namespace MyCinemaDiary.Infrastructure.Data
{
    // To initaliaze the database, run the following commands in the Package Manager Console:
    // > dotnet ef migrations add InitialCreate --output-dir ./Infrastructure/Data/Migrations
    // > dotnet ef database update

    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiaryEntry>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<DiaryEntry>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        //    var host = "192.168.1.131";
        //    var username = "obscure";
        //    var password = "secure";
        //    var database = "UCD";

        //    optionsBuilder.UseNpgsql($"Host={host};Port=5432;Database={database};User ID={username};Password={password};");
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DiaryEntry> DiaryEntries { get; set; }
    }
}
