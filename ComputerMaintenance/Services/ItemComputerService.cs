using ComputerMaintenance.Context;
using ComputerMaintenance.Models;
using ComputerMaintenance.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ComputerMaintenance.Models.Enum.EnumStatus;

namespace ComputerMaintenance.Services
{
    public class ItemComputerService
    {
        private readonly AppContextModel _context;

        public ItemComputerService(AppContextModel context)
        {
            _context = context;
        }

        public async Task<ActionResult<ItemComputerViewModel>> GetItemComputers(Guid computerId)
        {
            List<ItemComputer> itemComputers = await _context.ItemComputers
                                                        .Include(i => i.Item)
                                                        .Include(c => c.Computer)
                                                        .Where(ic => ic.ComputerId == computerId).ToListAsync();

            if (itemComputers == null)
            {
                return null;
            }

            var computer = await _context.Computers.FindAsync(computerId);

            if (computer == null)
            {
                return null;
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

        public async Task PostItemComputer(ItemComputer itemComputer)
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

            foreach (var maintenanceItem in listMaintenanceItems)
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

                    dateAddMonth = dateAddMonth.AddMonths(listMaintenanceItems.Find(mi => mi.MaintenanceId == scheduleMI.MaintenanceId).Period);

                    schedule.MaintenanceDate = dateAddMonth;

                    schedules.Add(schedule);
                }
            }

            _context.ItemComputers.Add(itemComputer);
            _context.ScheduleItemComputers.Add(scheduleItemComputer);
            _context.ScheduleMaintenanceItems.AddRange(scheduleMaintenanceItems);
            _context.Schedules.AddRange(schedules);
        }

        public bool ItemComputerExists(Guid id)
        {
            return _context.ItemComputers.Any(e => e.ComputerId == id);
        }
    }
}
