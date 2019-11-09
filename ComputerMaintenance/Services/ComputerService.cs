using ComputerMaintenance.Context;
using ComputerMaintenance.Models;
using ComputerMaintenance.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerMaintenance.Services
{
    public class ComputerService
    {
        private readonly AppContextModel _context;

        public ComputerService(AppContextModel context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Computer>>> GetComputers()
        {
            return await _context.Computers.ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<ItemComputer>>> GetItemComputers(Guid id)
        {
            List<ItemComputer> itemComputers = await _context.ItemComputers
                                                        .Include(i => i.Item)
                                                        .Include(c => c.Computer)
                                                        .Where(ic => ic.ComputerId == id).ToListAsync();

            if (itemComputers == null)
            {
                return null;
            }

            return itemComputers;
        }

        public async Task<ActionResult<Computer>> GetComputer(Guid id)
        {
            var computer = await _context.Computers.FindAsync(id);

            if (computer == null)
            {
                return null;
            }

            return computer;
        }

        public async Task<ActionResult<IEnumerable<ComputerScheduleViewModel>>> GetComputerSchedule(Guid id)
        {
            List<ScheduleMaintenanceItem> scheduleMaintenanceItemComputers = new List<ScheduleMaintenanceItem>();
            List<ComputerScheduleViewModel> computerScheduleViewModelsGroup = new List<ComputerScheduleViewModel>();

            List<ScheduleItemComputer> scheduleItemComputers = await _context.ScheduleItemComputers
                                                                    .Where(sic => sic.ComputerId == id)
                                                                    .ToListAsync();

            if (scheduleItemComputers == null)
            {
                return null;
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
                return null;
            }

            foreach (var scheduleMaintenanceItemComputer in scheduleMaintenanceItemComputers)
            {
                List<ComputerScheduleViewModel> computerScheduleViewModels = await _context.Schedules
                                                    .Where(s => s.ScheduleMaintenanceItemId == scheduleMaintenanceItemComputer.Id)
                                                    .Select(schedule => new ComputerScheduleViewModel
                                                    {
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

        public async Task PostComputer(Computer computer)
        {
            _context.Computers.Add(computer);
            await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<Computer>> DeleteComputer(Guid id)
        {
            var computer = await _context.Computers.FindAsync(id);

            if (computer == null)
            {
                return null;
            }

            _context.Computers.Remove(computer);
            await _context.SaveChangesAsync();

            return computer;
        }

        public async Task<ActionResult<ItemComputer>> DeleteItemComputer(Guid computerId, Guid itemId)
        {
            var itemComputer = await _context.ItemComputers.FindAsync(computerId, itemId);

            if (itemComputer == null)
            {
                return null;
            }

            _context.ItemComputers.Remove(itemComputer);

            ScheduleItemComputer scheduleItemComputer = await _context.ScheduleItemComputers.Where(sic => sic.ComputerId == computerId
                                                                                             && sic.ItemId == sic.ItemId)
                                                                                            .FirstOrDefaultAsync();

            if (scheduleItemComputer != null)
            {
                List<ScheduleMaintenanceItem> scheduleMaintenanceItemsList = new List<ScheduleMaintenanceItem>();

                var smiList = await _context.ScheduleMaintenanceItems.Where(smi => smi.ScheduleItemComputerId == scheduleItemComputer.Id
                                                                            ).ToListAsync();
                scheduleMaintenanceItemsList.AddRange(smiList);

                if (scheduleMaintenanceItemsList.Count > 0)
                {
                    List<Schedule> schedulesList = new List<Schedule>();

                    foreach (var smiObj in scheduleMaintenanceItemsList)
                    {
                        var sList = await _context.Schedules.Where(s => s.ScheduleMaintenanceItemId == smiObj.Id).ToListAsync();

                        schedulesList.AddRange(sList);
                    }

                    if (schedulesList.Count > 0)
                    {
                        _context.Schedules.RemoveRange(schedulesList);
                    }

                    _context.ScheduleMaintenanceItems.RemoveRange(scheduleMaintenanceItemsList);
                }

                _context.ScheduleItemComputers.Remove(scheduleItemComputer);
            }

            await _context.SaveChangesAsync();

            return itemComputer;
        }

        public bool ComputerExists(Guid id)
        {
            return _context.Computers.Any(e => e.Id == id);
        }
    }
}
