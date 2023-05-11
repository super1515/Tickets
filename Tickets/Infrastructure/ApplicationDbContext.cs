using Microsoft.EntityFrameworkCore;
using Tickets.Models;

namespace Tickets.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Segments> Segments { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
