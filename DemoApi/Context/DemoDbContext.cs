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
    }
}
