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
    public class ItemController : ControllerBase
    {
        private readonly AppContextModel _context;

        public ItemController(AppContextModel context)
        {
            _context = context;
        }

        // GET: api/Item
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return await _context.Items.ToListAsync();
        }

        // GET: api/Item/MaintenanceItem/id
        [HttpGet("MaintenanceItem/{id}")]
        public async Task<ActionResult<IEnumerable<MaintenanceItem>>> GetIMaintenanceItems(Guid id)
        {
            List<MaintenanceItem> maintenanceItems = await _context.MaintenanceItems
                                                        .Include(m => m.Maintenance)
                                                        .Include(i => i.Item)
                                                        .Where(mi => mi.ItemId == id).ToListAsync();

            if (maintenanceItems == null)
            {
                return NotFound();
            }

            return maintenanceItems;
        }

        // GET: api/Item/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(Guid id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Item/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(Guid id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        // POST: api/Item
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        // DELETE: api/Item/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItem(Guid id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        // DELETE: api/MaintenanceItem/5/6
        [HttpDelete("{itemId}/{maintenanceId}")]
        public async Task<ActionResult<MaintenanceItem>> DeleteMaintenanceItem(Guid itemId, Guid maintenanceId)
        {
            var maintenanceItem = await _context.MaintenanceItems.FindAsync(itemId, maintenanceId);
            if (maintenanceItem == null)
            {
                return NotFound();
            }

            _context.MaintenanceItems.Remove(maintenanceItem);
            await _context.SaveChangesAsync();

            return maintenanceItem;
        }

        private bool ItemExists(Guid id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
