using DemoApi.Models;
using System.Data.Entity;

namespace DemoApi.Context
{
    public class DemoDbContext:DbContext
    {
        //public DemoDbContext(DbContextOptions<DemoDbContext> options): base(options)
        //{

        //}

        public DbSet<User> Users { get; set; }
    }
}
