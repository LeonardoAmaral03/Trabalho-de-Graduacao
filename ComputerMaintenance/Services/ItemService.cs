using ComputerMaintenance.Context;
using ComputerMaintenance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerMaintenance.Services
{
    public class ItemService
    {
        private readonly AppContextModel _context;

        public ItemService(AppContextModel context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<ActionResult<IEnumerable<MaintenanceItem>>> GetMaintenanceItems(Guid id)
        {
            List<MaintenanceItem> maintenanceItems = await _context.MaintenanceItems
                                                        .Include(m => m.Maintenance)
                                                        .Include(i => i.Item)
                                                        .Where(mi => mi.ItemId == id).ToListAsync();

            if (maintenanceItems == null)
            {
                return null;
            }

            return maintenanceItems;
        }

        public async Task<ActionResult<Item>> GetItem(Guid id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return null;
            }

            return item;
        }

        public async Task PostItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<Item>> DeleteItem(Guid id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return null;
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<ActionResult<MaintenanceItem>> DeleteMaintenanceItem(Guid itemId, Guid maintenanceId)
        {
            var maintenanceItem = await _context.MaintenanceItems.FindAsync(itemId, maintenanceId);

            if (maintenanceItem == null)
            {
                return null;
            }

            _context.MaintenanceItems.Remove(maintenanceItem);

            var scheduleItemComputersId = await (from sic in _context.ScheduleItemComputers
                                                 where sic.ItemId == itemId
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
                                                                          && smi.MaintenanceId == maintenanceId).ToListAsync();
                    scheduleMaintenanceItemsList.AddRange(smiList);
                }

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
            }

            await _context.SaveChangesAsync();

            return maintenanceItem;
        }

        public bool ItemExists(Guid id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
