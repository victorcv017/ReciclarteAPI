using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
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

            modelBuilder.Entity<MaterialsPerCenter>()
            .HasKey(mc => new { mc.CenterId, mc.MaterialId});

            modelBuilder.Entity<MaterialsPerCenter>()
                .HasOne(mc => mc.Center)
                .WithMany(c => c.MaterialsPerCenters)
                .HasForeignKey(mc => mc.CenterId);

            modelBuilder.Entity<MaterialsPerCenter>()
                .HasOne(mc => mc.Material)
                .WithMany(m => m.MaterialsPerCenters)
                .HasForeignKey(mc => mc.MaterialId);

            base.OnModelCreating(modelBuilder);



        }

        public DbSet<Addresses> Addresses { get; set; }
        public DbSet<Centers> Centers { get; set; }
        public DbSet<Enterprises> Enterprises { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<Materials> Materials { get; set; }
        public DbSet<MaterialsPerCenter> MaterialsPerCenter { get; set; }
        public DbSet<Offices> Offices { get; set; }
        public DbSet<Purchases> Purchases { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public new DbSet<Users> Users { get; set; }
    }
}
