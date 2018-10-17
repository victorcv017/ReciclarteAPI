using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Curp)
                .IsUnique();

            modelBuilder.Entity<Enterprises>()
                .HasIndex(e => e.RFC)
                .IsUnique();

            modelBuilder.Entity<Enterprises>()
                .HasIndex(e => e.Email)
                .IsUnique();




        }

        public DbSet<Enterprises> Enterprises { get; set; }
    }
}
