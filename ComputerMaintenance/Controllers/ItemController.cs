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
    public class ItemController : ControllerBase
    {
        private readonly AppContextModel _context;
        private ItemService itemService;

        public ItemController(AppContextModel context)
        {
            _context = context;
            itemService = new ItemService(_context);
        }

        // GET: api/Item
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            var result = await itemService.GetItems();
            return result;
        }

        // GET: api/Item/MaintenanceItem/id
        [HttpGet("MaintenanceItem/{id}")]
        public async Task<ActionResult<IEnumerable<MaintenanceItem>>> GetMaintenanceItems(Guid id)
        {
            var result = await itemService.GetMaintenanceItems(id);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // GET: api/Item/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(Guid id)
        {
            var result = await itemService.GetItem(id);

            if (result == null)
            {
                return NotFound();
            }

            return result;
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
                if (!itemService.ItemExists(id))
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
            await itemService.PostItem(item);

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        // DELETE: api/Item/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> DeleteItem(Guid id)
        {
            var result = await itemService.DeleteItem(id);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // DELETE: api/MaintenanceItem/5/6
        [HttpDelete("{itemId}/{maintenanceId}")]
        public async Task<ActionResult<MaintenanceItem>> DeleteMaintenanceItem(Guid itemId, Guid maintenanceId)
        {
            var result = await itemService.DeleteMaintenanceItem(itemId, maintenanceId);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }
    }
}
