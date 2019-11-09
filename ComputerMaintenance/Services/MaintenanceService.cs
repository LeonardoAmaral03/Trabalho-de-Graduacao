using ComputerMaintenance.Context;
using ComputerMaintenance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerMaintenance.Services
{
    public class MaintenanceService
    {
        private readonly AppContextModel _context;

        public MaintenanceService(AppContextModel context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Maintenance>>> GetMaintenances()
        {
            return await _context.Maintenances.ToListAsync();
        }

        public async Task<ActionResult<Maintenance>> GetMaintenance(Guid id)
        {
            var maintenance = await _context.Maintenances.FindAsync(id);

            if (maintenance == null)
            {
                return null;
            }

            return maintenance;
        }

        public async Task PostMaintenance(Maintenance maintenance)
        {
            _context.Maintenances.Add(maintenance);
            await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<Maintenance>> DeleteMaintenance(Guid id)
        {
            var maintenance = await _context.Maintenances.FindAsync(id);

            if (maintenance == null)
            {
                return null;
            }

            _context.Maintenances.Remove(maintenance);
            await _context.SaveChangesAsync();

            return maintenance;
        }

        public bool MaintenanceExists(Guid id)
        {
            return _context.Maintenances.Any(e => e.Id == id);
        }
    }
}
