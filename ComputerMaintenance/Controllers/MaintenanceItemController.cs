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
    public class MaintenanceItemController : ControllerBase
    {
        private readonly AppContextModel _context;

        public MaintenanceItemController(AppContextModel context)
        {
            _context = context;
        }

        // GET: api/MaintenanceItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceItem>>> GetMaintenanceItems()
        {
            return await _context.MaintenanceItems.ToListAsync();
        }

        // GET: api/MaintenanceItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MaintenanceItem>> GetMaintenanceItem(Guid id)
        {
            var maintenanceItem = await _context.MaintenanceItems.FindAsync(id);

            if (maintenanceItem == null)
            {
                return NotFound();
            }

            return maintenanceItem;
        }

        // PUT: api/MaintenanceItem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaintenanceItem(Guid id, MaintenanceItem maintenanceItem)
        {
            if (id != maintenanceItem.ItemId)
            {
                return BadRequest();
            }

            _context.Entry(maintenanceItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceItemExists(id))
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

        // POST: api/MaintenanceItem
        [HttpPost]
        public async Task<ActionResult<MaintenanceItem>> PostMaintenanceItem(MaintenanceItem maintenanceItem)
        {
            _context.MaintenanceItems.Add(maintenanceItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MaintenanceItemExists(maintenanceItem.ItemId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMaintenanceItem", new { id = maintenanceItem.ItemId }, maintenanceItem);
        }

        // DELETE: api/MaintenanceItem/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MaintenanceItem>> DeleteMaintenanceItem(Guid id)
        {
            var maintenanceItem = await _context.MaintenanceItems.FindAsync(id);
            if (maintenanceItem == null)
            {
                return NotFound();
            }

            _context.MaintenanceItems.Remove(maintenanceItem);
            await _context.SaveChangesAsync();

            return maintenanceItem;
        }

        private bool MaintenanceItemExists(Guid id)
        {
            return _context.MaintenanceItems.Any(e => e.ItemId == id);
        }
    }
}
