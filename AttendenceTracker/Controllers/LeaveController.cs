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
    public class LeaveController : ControllerBase
    {
        private readonly AttendenceSystemDbContext _context;

        public LeaveController(AttendenceSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/Leave
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Leave>>> GetLeave()
        {
            return await _context.Leave.ToListAsync();
        }

        // GET: api/Leave/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Leave>> GetLeave(int id)
        {
            var leave = await _context.Leave.FindAsync(id);

            if (leave == null)
            {
                return NotFound();
            }

            return leave;
        }

        // PUT: api/Leave/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeave(int id, Leave leave)
        {
            if (id != leave.Id)
            {
                return BadRequest();
            }

            _context.Entry(leave).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveExists(id))
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

        // POST: api/Leave
        [HttpPost]
        public async Task<ActionResult<Leave>> PostLeave(Leave leave)
        {
            // Check if a leave with the same dates and teacher already exists
            bool leaveExists = _context.Leave.Any(l =>
                l.StudentId == leave.StudentId &&
                l.TeacherId == leave.TeacherId &&
                l.FromDate == leave.FromDate &&
                l.ToDate == leave.ToDate);

            if (leaveExists)
            {
                // Return a conflict response with an appropriate message
                return Conflict("Leave with the same dates and teacher already exists.");
            }

            _context.Leave.Add(leave);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLeave", new { id = leave.Id }, leave);
        }



        // DELETE: api/Leave/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Leave>> DeleteLeave(int id)
        {
            var leave = await _context.Leave.FindAsync(id);
            if (leave == null)
            {
                return NotFound();
            }

            _context.Leave.Remove(leave);
            await _context.SaveChangesAsync();

            return leave;
        }

        private bool LeaveExists(int id)
        {
            return _context.Leave.Any(e => e.Id == id);
        }

        [HttpGet("bystudentandteacher/{studentId}/{teacherId}")]
        public async Task<ActionResult<IEnumerable<Leave>>> GetLeavesByStudentAndTeacher(int studentId, int teacherId)
        {
            var leaves = await _context.Leave
                .Where(leave => leave.StudentId == studentId && leave.TeacherId == teacherId)
                .ToListAsync();

            if (leaves == null || leaves.Count == 0)
            {
                return NotFound();
            }

            return leaves;
        }


        [HttpGet("bystudent/{studentId}")]
        public async Task<ActionResult<IEnumerable<Leave>>> GetLeavesByStudent(int studentId)
        {
            var leaves = await _context.Leave
                .Where(leave => leave.StudentId == studentId)
                .ToListAsync();

            if (leaves == null || leaves.Count == 0)
            {
                return NotFound();
            }

            return leaves;
        }

        // Custom action to get leaves by TeacherId
        [HttpGet("byteacher/{teacherId}")]
        public async Task<ActionResult<IEnumerable<Leave>>> GetLeavesByTeacher(int teacherId)
        {
            var leaves = await _context.Leave
                .Where(leave => leave.TeacherId == teacherId)
                .ToListAsync();

            if (leaves == null || leaves.Count == 0)
            {
                return NotFound();
            }

            return leaves;
        }

    }
}
