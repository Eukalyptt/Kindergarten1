using Kindergarten.Models;
using Microsoft.EntityFrameworkCore;


namespace Kindergarten.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Group> Groups { get; set; }
    }
}
