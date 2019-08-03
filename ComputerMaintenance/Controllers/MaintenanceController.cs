using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerMaintenance.Context;
using ComputerMaintenance.Models;

namespace ComputerMaintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly AppContextModel _context;

        public MaintenanceController(AppContextModel context)
        {
            _context = context;
        }

        // GET: api/Maintenance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Maintenance>>> GetMaintenances()
        {
            return await _context.Maintenances.ToListAsync();
        }

        // GET: api/Maintenance/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Maintenance>> GetMaintenance(Guid id)
        {
            var maintenance = await _context.Maintenances.FindAsync(id);

            if (maintenance == null)
            {
                return NotFound();
            }

            return maintenance;
        }

        // PUT: api/Maintenance/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaintenance(Guid id, Maintenance maintenance)
        {
            if (id != maintenance.Id)
            {
                return BadRequest();
            }

            _context.Entry(maintenance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Maintenance
        [HttpPost]
        public async Task<ActionResult<Maintenance>> PostMaintenance(Maintenance maintenance)
        {
            _context.Maintenances.Add(maintenance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaintenance", new { id = maintenance.Id }, maintenance);
        }

        // DELETE: api/Maintenance/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Maintenance>> DeleteMaintenance(Guid id)
        {
            var maintenance = await _context.Maintenances.FindAsync(id);
            if (maintenance == null)
            {
                return NotFound();
            }

            _context.Maintenances.Remove(maintenance);
            await _context.SaveChangesAsync();

            return maintenance;
        }

        private bool MaintenanceExists(Guid id)
        {
            return _context.Maintenances.Any(e => e.Id == id);
        }
    }
}
