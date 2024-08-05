using DemoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Context
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Markalar> Markalar { get; set; }
        public DbSet<UrunGruplari> UrunGruplari { get; set; }
        public DbSet<UrunFiyati> UrunFiyati { get; set; }
        public DbSet<UrunUretici> UrunUretici { get; set; }
    }
}
