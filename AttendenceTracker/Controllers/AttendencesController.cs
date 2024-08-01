using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AttendenceTracker.Models;

namespace AttendenceTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendencesController : ControllerBase
    {
        private readonly AttendenceSystemDbContext _context;

        public AttendencesController(AttendenceSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/Attendences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendence>>> GetAttendence()
        {
            return await _context.Attendence.ToListAsync();
        }

        // GET: api/Attendences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Attendence>> GetAttendence(int id)
        {
            var attendence = await _context.Attendence.FindAsync(id);

            if (attendence == null)
            {
                return NotFound();
            }

            return attendence;
        }


        // PUT: api/Attendences/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttendence(int id, Attendence attendence)
        {
            if (id != attendence.Sno)
            {
                return BadRequest();
            }

            _context.Entry(attendence).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendenceExists(id))
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

        // POST: api/Attendences
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Attendence>> PostAttendence(Attendence attendence)
        {
            _context.Attendence.Add(attendence);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAttendence", new { id = attendence.Sno }, attendence);
        }

        // DELETE: api/Attendences/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Attendence>> DeleteAttendence(int id)
        {
            var attendence = await _context.Attendence.FindAsync(id);
            if (attendence == null)
            {
                return NotFound();
            }

            _context.Attendence.Remove(attendence);
            await _context.SaveChangesAsync();

            return attendence;
        }

        private bool AttendenceExists(int id)
        {
            return _context.Attendence.Any(e => e.Sno == id);
        }
    }
}
