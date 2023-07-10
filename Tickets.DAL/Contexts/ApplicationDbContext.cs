using Microsoft.EntityFrameworkCore;
using Tickets.DAL.Models;

namespace Tickets.DAL.Contexts
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
