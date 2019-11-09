using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComputerMaintenance.Context;
using ComputerMaintenance.Models;
using ComputerMaintenance.Models.ViewModels;
using static ComputerMaintenance.Models.Enum.EnumStatus;
using ComputerMaintenance.Services;

namespace ComputerMaintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemComputerController : ControllerBase
    {
        private readonly AppContextModel _context;
        private ItemComputerService itemComputerService;

        public ItemComputerController(AppContextModel context)
        {
            _context = context;
            itemComputerService = new ItemComputerService(_context);
        }

        //// GET: api/ItemComputer
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ItemComputer>>> GetItemComputers()
        //{
        //    return await _context.ItemComputers.ToListAsync();
        //}

        //// GET: api/ItemComputer/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ItemComputer>> GetItemComputer(Guid id)
        //{
        //    var itemComputer = await _context.ItemComputers.FindAsync(id);

        //    if (itemComputer == null)
        //    {
        //        return NotFound();
        //    }

        //    return itemComputer;
        //}

        // GET: api/Item/MaintenanceItems/itemId
        [HttpGet("{computerId}")]
        public async Task<ActionResult<ItemComputerViewModel>> GetItemComputers(Guid computerId)
        {
            var result = await itemComputerService.GetItemComputers(computerId);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        //// PUT: api/ItemComputer/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutItemComputer(Guid id, ItemComputer itemComputer)
        //{
        //    if (id != itemComputer.ComputerId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(itemComputer).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ItemComputerExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/ItemComputer
        [HttpPost]
        public async Task<ActionResult<ItemComputer>> PostItemComputer(ItemComputer itemComputer)
        {
            await itemComputerService.PostItemComputer(itemComputer);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (itemComputerService.ItemComputerExists(itemComputer.ComputerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetItemComputers", new { computerId = itemComputer.ComputerId }, itemComputer);
        }

        //// DELETE: api/ItemComputer/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<ItemComputer>> DeleteItemComputer(Guid id)
        //{
        //    var itemComputer = await _context.ItemComputers.FindAsync(id);
        //    if (itemComputer == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.ItemComputers.Remove(itemComputer);
        //    await _context.SaveChangesAsync();

        //    return itemComputer;
        //}
    }
}
