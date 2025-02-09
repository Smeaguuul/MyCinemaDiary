using Npgsql;
using Microsoft.EntityFrameworkCore;
using MyCinemaDiary.Domain.Entities;

namespace MyCinemaDiary.Infrastructure.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
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
    }
}
