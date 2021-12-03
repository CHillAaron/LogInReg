using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogInReg.Models;

namespace LogInReg.Models
{
    public class LoginDbContext : DbContext
    {
        public LoginDbContext(DbContextOptions options) : base(options) { }
        // the "Monsters" table name will come from the DbSet variable name
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql("Host=192.168.1.104;Username=postgres;Password=root;Database=LogInReg");
    }
}
