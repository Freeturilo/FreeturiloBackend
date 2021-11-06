using Microsoft.EntityFrameworkCore;

namespace FreeturiloWebApi.Models
{
    public class FreeturiloContext : DbContext
    {
        public FreeturiloContext(DbContextOptions<FreeturiloContext> options) : base(options) { }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
    }
}
