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
    public class MaintenanceItemController : ControllerBase
    {
        private readonly AppContextModel _context;

        public MaintenanceItemController(AppContextModel context)
        {
            _context = context;
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
            List<MaintenanceItem> maintenanceItems = await _context.MaintenanceItems
                                                        .Include(m => m.Maintenance)
                                                        .Include(i => i.Item)
                                                        .Where(mi => mi.ItemId == itemId).ToListAsync();

            if (maintenanceItems == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(itemId);

            if (item == null)
            {
                return NotFound();
            }

            var maintenances = await _context.Maintenances.Where(m =>
                                        !_context.MaintenanceItems.Any(mi => mi.ItemId == itemId && mi.MaintenanceId == m.Id)
                                     ).ToListAsync();

            MaintenanceItemViewModel maintenanceItemViewModel = new MaintenanceItemViewModel()
            {
                Item = item,
                Maintenances = maintenances,
                MaintenanceItems = maintenanceItems
            };

            return maintenanceItemViewModel;
        }

        // GET: api/Item/MaintenanceItems/itemId
        [HttpGet("{itemId}/{maintenanceId}")]
        public async Task<ActionResult<MaintenanceItemEditViewModel>> GetMaintenanceItem(Guid itemId, Guid maintenanceId)
        {
            MaintenanceItem maintenanceItem = await _context.MaintenanceItems
                                                        .Include(m => m.Maintenance)
                                                        .Include(i => i.Item)
                                                        .Where(mi => mi.ItemId == itemId && mi.MaintenanceId == maintenanceId)
                                                        .FirstOrDefaultAsync();

            if (maintenanceItem == null)
            {
                return NotFound();
            }

            //var item = await _context.Items.FindAsync(itemId);

            //if (item == null)
            //{
            //    return NotFound();
            //}

            var maintenance = await _context.Maintenances.FindAsync(maintenanceId);

            MaintenanceItemEditViewModel maintenanceItemEditViewModel = new MaintenanceItemEditViewModel()
            {
                Maintenance = maintenance,
                MaintenanceItem = maintenanceItem
            };

            return maintenanceItemEditViewModel;
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

            _context.Entry(maintenanceItem).State = EntityState.Modified;

            var scheduleItemComputersId = await (from sic in _context.ScheduleItemComputers
                                                 where sic.ItemId == maintenanceItem.ItemId
                                                 select new
                                                 {
                                                     sic.Id
                                                 }).ToListAsync();

            if (scheduleItemComputersId.Count() > 0)
            {
                List<ScheduleMaintenanceItem> scheduleMaintenanceItemsList = new List<ScheduleMaintenanceItem>();

                foreach (var sicId in scheduleItemComputersId)
                {
                    var smiList = await _context.ScheduleMaintenanceItems.Where(smi => smi.ScheduleItemComputerId == sicId.Id
                                                                          && smi.MaintenanceId == maintenanceItem.MaintenanceId)
                                                                         .ToListAsync();
                    scheduleMaintenanceItemsList.AddRange(smiList);
                }

                if (scheduleMaintenanceItemsList.Count > 0)
                {
                    //List<Schedule> schedulesList = new List<Schedule>();

                    foreach (var smiObj in scheduleMaintenanceItemsList)
                    {
                        var schedulesList = await _context.Schedules.Where(s => s.ScheduleMaintenanceItemId == smiObj.Id).ToListAsync();

                        //schedulesList.AddRange(sList);


                        if (schedulesList.Count > 0)
                        {
                            var scheduleStatusLastDate = DateTime.Now;
                            var scheduleStatusPending = schedulesList.Where(sl => sl.Status == Status.Pending).ToList();

                            if (schedulesList.Where(sl => sl.Status != Status.Pending).Any())
                            {
                                scheduleStatusLastDate = schedulesList.Where(sl => sl.Status != Status.Pending)
                                                                          .OrderByDescending(sl => sl.MaintenanceDate)
                                                                          .Select(sl => sl.MaintenanceDate)
                                                                          .FirstOrDefault();
                            }

                            var dateYearsPeriod = scheduleStatusLastDate.AddYears(4).Year;
                            var dateAddMonth = scheduleStatusLastDate;

                            Schedule schedule;
                            List<Schedule> schedules = new List<Schedule>();

                            while (dateAddMonth.Year < dateYearsPeriod)
                            {
                                schedule = new Schedule();

                                schedule.Id = Guid.NewGuid();
                                schedule.ScheduleMaintenanceItemId = smiObj.Id;
                                schedule.Status = Status.Pending;

                                dateAddMonth = dateAddMonth.AddMonths(maintenanceItem.Period);

                                schedule.MaintenanceDate = dateAddMonth;

                                schedules.Add(schedule);
                            }

                            if (scheduleStatusPending.Count > 0)
                            {
                                _context.Schedules.RemoveRange(scheduleStatusPending);
                            }

                            _context.Schedules.AddRange(schedules);
                        }
                    }
                }
            }

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
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateException)
            //{
            //    if (MaintenanceItemExists(maintenanceItem.ItemId))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            List<ScheduleItemComputer> scheduleItemComputerList = await (from sic in _context.ScheduleItemComputers
                                                                         where sic.ItemId == maintenanceItem.ItemId
                                                                         select new ScheduleItemComputer
                                                                         {
                                                                             Id = sic.Id,
                                                                         }).ToListAsync();

            if (scheduleItemComputerList.Count() > 0)
            {
                ScheduleMaintenanceItem scheduleMaintenanceItem;
                List<ScheduleMaintenanceItem> scheduleMaintenanceItems = new List<ScheduleMaintenanceItem>();
                Schedule schedule;
                List<Schedule> schedules = new List<Schedule>();

                var registrationDate = DateTime.Now;
                var dateYearsPeriod = registrationDate.AddYears(4).Year;

                foreach (var scheduleItemComputer in scheduleItemComputerList)
                {
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

                        dateAddMonth = dateAddMonth.AddMonths(maintenanceItem.Period);

                        schedule.MaintenanceDate = dateAddMonth;

                        schedules.Add(schedule);
                    }
                }

                _context.ScheduleMaintenanceItems.AddRange(scheduleMaintenanceItems);
                _context.Schedules.AddRange(schedules);
            }

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

            return CreatedAtAction("GetMaintenanceItems", new { itemId = maintenanceItem.ItemId }, maintenanceItem);
        }

        private bool MaintenanceItemExists(Guid id)
        {
            return _context.MaintenanceItems.Any(e => e.ItemId == id);
        }
    }
}
