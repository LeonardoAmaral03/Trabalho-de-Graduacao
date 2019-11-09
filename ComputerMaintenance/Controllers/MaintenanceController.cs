using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerMaintenance.Context;
using ComputerMaintenance.Models;
using ComputerMaintenance.Services;

namespace ComputerMaintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly AppContextModel _context;
        private MaintenanceService maintenanceService;

        public MaintenanceController(AppContextModel context)
        {
            _context = context;
            maintenanceService = new MaintenanceService(_context);
        }

        // GET: api/Maintenance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Maintenance>>> GetMaintenances()
        {
            var result = await maintenanceService.GetMaintenances();
            return result;
        }

        // GET: api/Maintenance/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Maintenance>> GetMaintenance(Guid id)
        {
            var result = await maintenanceService.GetMaintenance(id);

            if (result == null)
            {
                return NotFound();
            }

            return result;
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
                if (!maintenanceService.MaintenanceExists(id))
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
            await maintenanceService.PostMaintenance(maintenance);

            return CreatedAtAction("GetMaintenance", new { id = maintenance.Id }, maintenance);
        }

        // DELETE: api/Maintenance/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Maintenance>> DeleteMaintenance(Guid id)
        {
            var result = await maintenanceService.DeleteMaintenance(id);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }
    }
}
