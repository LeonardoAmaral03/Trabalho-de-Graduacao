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
    }
}
