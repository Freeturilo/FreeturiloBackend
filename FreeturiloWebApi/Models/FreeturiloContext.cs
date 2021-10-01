using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Models
{
    public class FreeturiloContext : DbContext
    {
        public FreeturiloContext(DbContextOptions<FreeturiloContext> options): base(options)
        {

        }
        public DbSet<Station> Stations { get; set; }
    }
}
