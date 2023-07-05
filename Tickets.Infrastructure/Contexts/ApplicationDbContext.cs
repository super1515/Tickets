using Microsoft.EntityFrameworkCore;
using Tickets.Infrastructure.Models;

namespace Tickets.Infrastructure.Contexts
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
