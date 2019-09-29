using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerMaintenance.Context;
using ComputerMaintenance.Models;
using ComputerMaintenance.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerMaintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        private readonly AppContextModel _context;

        public ComputerController(AppContextModel context)
        {
            _context = context;
        }

        // GET: api/Computer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Computer>>> GetComputers()
        {
            return await _context.Computers.ToListAsync();
        }

        // GET: api/Item/ItemComputer/id
        [HttpGet("ItemComputer/{id}")]
        public async Task<ActionResult<IEnumerable<ItemComputer>>> GetItemComputers(Guid id)
        {
            List<ItemComputer> itemComputers = await _context.ItemComputers
                                                        .Include(i => i.Item)
                                                        .Include(c => c.Computer)
                                                        .Where(ic => ic.ComputerId == id).ToListAsync();

            if (itemComputers == null)
            {
                return NotFound();
            }

            return itemComputers;
        }

        // GET: api/Computer/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Computer>> GetComputer(Guid id)
        {
            var computer = await _context.Computers.FindAsync(id);

            if(computer == null)
            {
                return NotFound();
            }

            return computer;
        }

        // GET: api/Item/ComputerSchedule/id
        [HttpGet("ComputerSchedule/{id}")]
        public async Task<ActionResult<IEnumerable<ComputerScheduleViewModel>>> GetComputerSchedule(Guid id)
        {
            List<ScheduleMaintenanceItem> scheduleMaintenanceItemComputers = new List<ScheduleMaintenanceItem>();
            List<ComputerScheduleViewModel> computerScheduleViewModelsGroup = new List<ComputerScheduleViewModel>();

            List<ScheduleItemComputer> scheduleItemComputers = await _context.ScheduleItemComputers
                                                                    .Where(sic => sic.ComputerId == id)
                                                                    .ToListAsync();

            if (scheduleItemComputers == null)
            {
                return NotFound();
            }

            foreach (var scheduleItemComputer in scheduleItemComputers)
            {
                List<ScheduleMaintenanceItem> scheduleMaintenanceItems = await _context.ScheduleMaintenanceItems
                                                                .Include(m => m.Maintenance)
                                                                .Include(sic => sic.ScheduleItemComputer)
                                                                    .ThenInclude(i => i.Item)
                                                                .Where(smi => smi.ScheduleItemComputerId == scheduleItemComputer.Id)
                                                                .ToListAsync();

                scheduleMaintenanceItemComputers.AddRange(scheduleMaintenanceItems);
            }

            if (scheduleMaintenanceItemComputers == null)
            {
                return NotFound();
            }

            foreach (var scheduleMaintenanceItemComputer in scheduleMaintenanceItemComputers)
            {
                List<ComputerScheduleViewModel> computerScheduleViewModels = await _context.Schedules
                                                    .Where(s => s.ScheduleMaintenanceItemId == scheduleMaintenanceItemComputer.Id)
                                                    .Select( schedule => new ComputerScheduleViewModel {
                                                        MaintenanceDate = schedule.MaintenanceDate,
                                                        MaintenanceName = scheduleMaintenanceItemComputer.Maintenance.Name,
                                                        ItemName = scheduleMaintenanceItemComputer.ScheduleItemComputer.Item.Name,
                                                        Status = schedule.Status
                                                    })
                                                    .ToListAsync();

                computerScheduleViewModelsGroup.AddRange(computerScheduleViewModels);
            }

            return computerScheduleViewModelsGroup;
        }

        // PUT: api/Computer/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComputer(Guid id, Computer computer)
        {
            if (id != computer.Id)
            {
                return BadRequest();
            }

            _context.Entry(computer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComputerExists(id))
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

        // POST: api/Computer
        [HttpPost]
        public async Task<ActionResult<Computer>> PostComputer(Computer computer)
        {
            _context.Computers.Add(computer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComputer", new { id = computer.Id }, computer);
        }        

        // DELETE: api/Computer/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Computer>> DeleteComputer(Guid id)
        {
            var computer = await _context.Computers.FindAsync(id);
            if (computer == null)
            {
                return NotFound();
            }

            _context.Computers.Remove(computer);
            await _context.SaveChangesAsync();

            return computer;
        }

        // DELETE: api/MaintenanceItem/5/6
        [HttpDelete("{computerId}/{itemId}")]
        public async Task<ActionResult<ItemComputer>> DeleteItemComputer(Guid computerId, Guid itemId)
        {
            var itemComputer = await _context.ItemComputers.FindAsync(computerId, itemId);
            if (itemComputer == null)
            {
                return NotFound();
            }

            _context.ItemComputers.Remove(itemComputer);
            await _context.SaveChangesAsync();

            return itemComputer;
        }

        private bool ComputerExists(Guid id)
        {
            return _context.Computers.Any(e => e.Id == id);
        }
    }
}