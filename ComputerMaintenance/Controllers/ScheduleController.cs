using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerMaintenance.Context;
using ComputerMaintenance.Models;
using ComputerMaintenance.Models.ViewModels;
using ComputerMaintenance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static ComputerMaintenance.Models.Enum.EnumStatus;

namespace ComputerMaintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly AppContextModel _context;
        private ScheduleService scheduleService;

        public ScheduleController(AppContextModel context)
        {
            _context = context;
            scheduleService = new ScheduleService(_context);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListSchedulesViewModel>>> GetListSchedules()
        {
            var result = await scheduleService.GetListSchedules();

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpPut()]
        public async Task<IActionResult> StatusAccomplished(UpdateStatus updateStatus)
        {
            var result = await scheduleService.StatusAccomplished(updateStatus);

            if (result == null)
            {
                return NotFound();
            }

            result.Status = Status.Accomplished;

            _context.Entry(result).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }
    }
}