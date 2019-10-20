using ComputerMaintenance.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ComputerMaintenance.Models.Enum.EnumStatus;

namespace ComputerMaintenance.Services
{
    public class StatusSchedule
    {
        private readonly AppContextModel _context;

        public StatusSchedule(AppContextModel context)
        {
            _context = context;
        }

        public async Task CheckStatus()
        {
            var today = DateTime.Now.Date;

            var schedules = await _context.Schedules.Where(s => s.MaintenanceDate < today
                                                     && s.Status == Status.Pending).ToListAsync();

            if (schedules.Count() > 0)
            {
                foreach (var schedule in schedules)
                {
                    schedule.Status = Status.NotAccomplished;

                    _context.Entry(schedule).State = EntityState.Modified;
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
        }
    }
}
