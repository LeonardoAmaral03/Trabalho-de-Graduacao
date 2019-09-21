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

namespace ComputerMaintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemComputerController : ControllerBase
    {
        private readonly AppContextModel _context;

        public ItemComputerController(AppContextModel context)
        {
            _context = context;
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
            List<ItemComputer> itemComputers = await _context.ItemComputers
                                                        .Include(i => i.Item)
                                                        .Include(c => c.Computer)
                                                        .Where(ic => ic.ComputerId == computerId).ToListAsync();

            if (itemComputers == null)
            {
                return NotFound();
            }

            var computer = await _context.Computers.FindAsync(computerId);

            if (computer == null)
            {
                return NotFound();
            }

            var items = await _context.Items.Where(i =>
                                        !_context.ItemComputers.Any(ic => ic.ComputerId == computerId && ic.ItemId == i.Id)
                                     ).ToListAsync();

            ItemComputerViewModel itemComputerViewModel = new ItemComputerViewModel()
            {
                Computer = computer,
                Items = items,
                ItemComputers = itemComputers
            };

            return itemComputerViewModel;
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
            var registrationDate = DateTime.Now;
            var dateYearsPeriod = registrationDate.AddYears(4).Year;
            var idScheduleItemComputer = Guid.NewGuid();

            ScheduleItemComputer scheduleItemComputer = new ScheduleItemComputer();
            ScheduleMaintenanceItem scheduleMaintenanceItem;
            List<ScheduleMaintenanceItem> scheduleMaintenanceItems = new List<ScheduleMaintenanceItem>();
            Schedule schedule;
            List<Schedule> schedules = new List<Schedule>();

            var listMaintenanceItems = await _context.MaintenanceItems.Where(mi => mi.ItemId == itemComputer.ItemId).ToListAsync();

            itemComputer.RegistrationDate = registrationDate;

            scheduleItemComputer.Id = idScheduleItemComputer;
            scheduleItemComputer.ComputerId = itemComputer.ComputerId;
            scheduleItemComputer.ItemId = itemComputer.ItemId;

            foreach (var maintenanceItem in listMaintenanceItems) {

                scheduleMaintenanceItem = new ScheduleMaintenanceItem();

                scheduleMaintenanceItem.Id = Guid.NewGuid();
                scheduleMaintenanceItem.ScheduleItemComputerId = scheduleItemComputer.Id;
                scheduleMaintenanceItem.MaintenanceId = maintenanceItem.MaintenanceId;

                scheduleMaintenanceItems.Add(scheduleMaintenanceItem);
            }

            foreach (var scheduleMI in scheduleMaintenanceItems)
            {
                var dateAddMonth = registrationDate;

                while (dateAddMonth.Year < dateYearsPeriod)
                {
                    schedule = new Schedule();

                    schedule.Id = Guid.NewGuid();
                    schedule.ScheduleMaintenanceItemId = scheduleMI.Id;
                    schedule.Status = Status.Pending;

                    dateAddMonth = dateAddMonth.AddMonths(listMaintenanceItems.Find(mi => mi.MaintenanceId == scheduleMI.MaintenanceId).Period);

                    schedule.MaintenanceDate = dateAddMonth;

                    schedules.Add(schedule);
                }
            }

            _context.ItemComputers.Add(itemComputer);
            _context.ScheduleItemComputers.Add(scheduleItemComputer);
            _context.ScheduleMaintenanceItems.AddRange(scheduleMaintenanceItems);
            _context.Schedules.AddRange(schedules);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ItemComputerExists(itemComputer.ComputerId))
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

        private bool ItemComputerExists(Guid id)
        {
            return _context.ItemComputers.Any(e => e.ComputerId == id);
        }
    }
}
