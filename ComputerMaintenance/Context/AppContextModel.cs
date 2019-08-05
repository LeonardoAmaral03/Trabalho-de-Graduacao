using ComputerMaintenance.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerMaintenance.Context
{
    public class AppContextModel : DbContext
    {
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<ItemComputer> ItemComputers { get; set; }
        public DbSet<MaintenanceItem> MaintenanceItems { get; set; }

        public AppContextModel(DbContextOptions<AppContextModel> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* ItemComputer */
            modelBuilder.Entity<ItemComputer>()
                .HasKey(pt => new { pt.ComputerId, pt.ItemId });

            modelBuilder.Entity<ItemComputer>()
                .HasOne(pt => pt.Computer)
                .WithMany(p => p.ItemComputers)
                .HasForeignKey(pt => pt.ComputerId);

            modelBuilder.Entity<ItemComputer>()
                .HasOne(pt => pt.Item)
                .WithMany(t => t.ItemComputers)
                .HasForeignKey(pt => pt.ItemId);

            /* MaintenanceItem */
            modelBuilder.Entity<MaintenanceItem>()
                .HasKey(pt => new { pt.ItemId, pt.MaintenanceId });

            modelBuilder.Entity<MaintenanceItem>()
                .HasOne(pt => pt.Item)
                .WithMany(p => p.MaintenanceItems)
                .HasForeignKey(pt => pt.ItemId);

            modelBuilder.Entity<MaintenanceItem>()
                .HasOne(pt => pt.Maintenance)
                .WithMany(t => t.MaintenanceItems)
                .HasForeignKey(pt => pt.MaintenanceId);
        }
    }
}
