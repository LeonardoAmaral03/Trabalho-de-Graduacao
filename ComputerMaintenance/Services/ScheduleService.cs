﻿using ComputerMaintenance.Context;
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
    public class ScheduleService
    {
        private readonly AppContextModel _context;

        public ScheduleService(AppContextModel context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<ListSchedulesViewModel>>> GetListSchedules()
        {
            var today = DateTime.Now.Date;

            List<ListSchedulesViewModel> listSchedules = await (from s in _context.Schedules
                                                                join smi in _context.ScheduleMaintenanceItems on s.ScheduleMaintenanceItemId equals smi.Id
                                                                join sic in _context.ScheduleItemComputers on smi.ScheduleItemComputerId equals sic.Id
                                                                join m in _context.Maintenances on smi.MaintenanceId equals m.Id
                                                                join i in _context.Items on sic.ItemId equals i.Id
                                                                join c in _context.Computers on sic.ComputerId equals c.Id
                                                                where s.MaintenanceDate.Date <= today && s.Status != Status.Accomplished
                                                                orderby s.Status, s.MaintenanceDate
                                                                select new ListSchedulesViewModel
                                                                {
                                                                    ComputerId = c.Id,
                                                                    ComputerName = c.Name,
                                                                    ItemId = i.Id,
                                                                    ItemName = i.Name,
                                                                    MaintenanceId = m.Id,
                                                                    MaintenanceName = m.Name,
                                                                    MaintenanceDate = s.MaintenanceDate,
                                                                    Status = s.Status
                                                                }).ToListAsync();

            if (listSchedules.Count() == 0)
            {
                return null;
            }

            return listSchedules;
        }

        public async Task<Schedule> StatusAccomplished(UpdateStatus updateStatus)
        {
            DateTime realizationDate = DateTime.Now;

            Schedule schedule = await (from sic in _context.ScheduleItemComputers
                                       join smi in _context.ScheduleMaintenanceItems on sic.Id equals smi.ScheduleItemComputerId
                                       join s in _context.Schedules on smi.Id equals s.ScheduleMaintenanceItemId
                                       where sic.ComputerId == updateStatus.ComputerId
                                          && sic.ItemId == updateStatus.ItemId
                                          && smi.MaintenanceId == updateStatus.MaintenanceId
                                          && s.MaintenanceDate == updateStatus.MaintenanceDate
                                       select new Schedule
                                       {
                                           Id = s.Id,
                                           ScheduleMaintenanceItemId = s.ScheduleMaintenanceItemId,
                                           MaintenanceDate = s.MaintenanceDate,
                                           Status = s.Status,
                                           ScheduleMaintenanceItem = smi,
                                           MaintenanceRealizationDate = realizationDate
                                       }).FirstOrDefaultAsync();

            return schedule;
        }
    }
}
