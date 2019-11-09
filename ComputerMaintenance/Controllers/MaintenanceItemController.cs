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
    public class MaintenanceItemController : ControllerBase
    {
        private readonly AppContextModel _context;
        private MaintenanceItemService maintenanceItemService;

        public MaintenanceItemController(AppContextModel context)
        {
            _context = context;
            maintenanceItemService = new MaintenanceItemService(_context);
        }

        //// GET: api/Maintenances
        //[HttpGet("Maintenances")]
        //public async Task<ActionResult<List<Maintenance>>> GetMaintenances()
        //{
        //    return await _context.Maintenances.ToListAsync();
        //}

        //// GET: api/Item/5
        //[HttpGet("Item/{itemId}")]
        //public async Task<ActionResult<Item>> GetItem(Guid itemId)
        //{
        //    var item = await _context.Items.FindAsync(itemId);

        //    if (item == null)
        //    {
        //        return NotFound();
        //    }

        //    return item;
        //}


        // GET: api/Item/MaintenanceItems/itemId
        [HttpGet("{itemId}")]
        public async Task<ActionResult<MaintenanceItemViewModel>> GetMaintenanceItems(Guid itemId)
        {
            var result = await maintenanceItemService.GetMaintenanceItems(itemId);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // GET: api/Item/MaintenanceItems/itemId
        [HttpGet("{itemId}/{maintenanceId}")]
        public async Task<ActionResult<MaintenanceItemEditViewModel>> GetMaintenanceItem(Guid itemId, Guid maintenanceId)
        {
            var result = await maintenanceItemService.GetMaintenanceItem(itemId, maintenanceId);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        //// GET: api/MaintenanceItem/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<MaintenanceItem>> GetMaintenanceItem(Guid id)
        //{
        //    var maintenanceItem = await _context.MaintenanceItems.FindAsync(id);

        //    if (maintenanceItem == null)
        //    {
        //        return NotFound();
        //    }

        //    return maintenanceItem;
        //}

        // PUT: api/MaintenanceItem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaintenanceItem(Guid id, MaintenanceItem maintenanceItem)
        {
            if (id != maintenanceItem.ItemId)
            {
                return BadRequest();
            }

            await maintenanceItemService.PutMaintenanceItem(maintenanceItem);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!maintenanceItemService.MaintenanceItemExists(id))
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
            await maintenanceItemService.PostMaintenanceItem(maintenanceItem);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (maintenanceItemService.MaintenanceItemExists(maintenanceItem.ItemId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMaintenanceItems", new { itemId = maintenanceItem.ItemId }, maintenanceItem);
        }
    }
}
