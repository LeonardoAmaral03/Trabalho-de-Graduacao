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

namespace ComputerMaintenance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        private readonly AppContextModel _context;
        private ComputerService computerService;

        public ComputerController(AppContextModel context)
        {
            _context = context;
            computerService = new ComputerService(_context);
        }

        // GET: api/Computer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Computer>>> GetComputers()
        {
            var result = await computerService.GetComputers();
            return result;
        }

        // GET: api/Item/ItemComputer/id
        [HttpGet("ItemComputer/{id}")]
        public async Task<ActionResult<IEnumerable<ItemComputer>>> GetItemComputers(Guid id)
        {
            var result = await computerService.GetItemComputers(id);
            
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // GET: api/Computer/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Computer>> GetComputer(Guid id)
        {
            var result = await computerService.GetComputer(id);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // GET: api/Item/ComputerSchedule/id
        [HttpGet("ComputerSchedule/{id}")]
        public async Task<ActionResult<IEnumerable<ComputerScheduleViewModel>>> GetComputerSchedule(Guid id)
        {
            var result = await computerService.GetComputerSchedule(id);

            if (result == null)
            {
                return NotFound();
            }            

            return result;
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
                if (!computerService.ComputerExists(id))
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
            await computerService.PostComputer(computer);

            return CreatedAtAction("GetComputer", new { id = computer.Id }, computer);
        }        

        // DELETE: api/Computer/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Computer>> DeleteComputer(Guid id)
        {
            var result = await computerService.DeleteComputer(id);

            if (result == null)
            {
                return NotFound();
            }           

            return result;
        }

        // DELETE: api/MaintenanceItem/5/6
        [HttpDelete("{computerId}/{itemId}")]
        public async Task<ActionResult<ItemComputer>> DeleteItemComputer(Guid computerId, Guid itemId)
        {
            var result = await computerService.DeleteItemComputer(computerId, itemId);

            if (result == null)
            {
                return NotFound();
            }            

            return result;
        }
    }
}