using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerMaintenance.Context;
using ComputerMaintenance.Models;
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

        private bool ComputerExists(Guid id)
        {
            return _context.Computers.Any(e => e.Id == id);
        }
    }
}