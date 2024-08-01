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
    public class TeacherCoursesController : ControllerBase
    {
        private readonly AttendenceSystemDbContext _context;

        public TeacherCoursesController(AttendenceSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/TeacherCourses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherCourses>>> GetTeacherCourses()
        {
            return await _context.TeacherCourses.ToListAsync();
        }

        // GET: api/TeacherCourses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherCourses>> GetTeacherCourses(int id)
        {
            var teacherCourses = await _context.TeacherCourses.FindAsync(id);

            if (teacherCourses == null)
            {
                return NotFound();
            }

            return teacherCourses;
        }

        // PUT: api/TeacherCourses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacherCourses(int id, TeacherCourses teacherCourses)
        {
            if (id != teacherCourses.id)
            {
                return BadRequest();
            }

            _context.Entry(teacherCourses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherCoursesExists(id))
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

        // POST: api/TeacherCourses
        [HttpPost]
        public async Task<ActionResult<TeacherCourses>> PostTeacherCourses(TeacherCourses teacherCourses)
        {
            // Check if the course already exists
            if (CourseExists(teacherCourses.CourseId, teacherCourses.StaffId))
            {
                return Conflict(); // Return a conflict response if the course already exists
            }

            _context.TeacherCourses.Add(teacherCourses);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeacherCourses", new { id = teacherCourses.id }, teacherCourses);
        }

        private bool CourseExists(int courseId, int staffId)
        {
            return _context.TeacherCourses.Any(tc => tc.CourseId == courseId && tc.StaffId == staffId);
        }

        // DELETE: api/TeacherCourses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacherCourses(int id)
        {
            var teacherCourses = await _context.TeacherCourses.FindAsync(id);
            if (teacherCourses == null)
            {
                return NotFound();
            }

            _context.TeacherCourses.Remove(teacherCourses);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("StaffId/{staffId}")]
        public async Task<IActionResult> DeleteTeacherCoursesByStaffId(int staffId)
        {
            var teacherCourses = await _context.TeacherCourses
                .Where(tc => tc.StaffId == staffId)
                .ToListAsync();

            if (teacherCourses == null || teacherCourses.Count == 0)
            {
                return NotFound();
            }

            _context.TeacherCourses.RemoveRange(teacherCourses);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("StaffIdCourseId/{staffId}/{courseId}")]
        public async Task<IActionResult> DeleteTeacherCourseByStaffIdAndCourseId(int staffId, int courseId)
        {
            var teacherCourse = await _context.TeacherCourses
                .FirstOrDefaultAsync(tc => tc.StaffId == staffId && tc.CourseId == courseId);

            if (teacherCourse == null)
            {
                return NotFound();
            }

            _context.TeacherCourses.Remove(teacherCourse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/TeacherCourses/GetByCourseId/10
        [HttpGet("GetByCourseId/{courseId}")]
        public async Task<ActionResult<IEnumerable<TeacherCourses>>> GetTeacherCoursesByCourseId(int courseId)
        {
            var teacherCourses = await _context.TeacherCourses
                .Where(tc => tc.CourseId == courseId)
                .ToListAsync();

            if (teacherCourses == null || teacherCourses.Count == 0)
            {
                return NotFound();
            }

            return teacherCourses;
        }

        // GET: api/TeacherCourses/GetByStaffId/20
        [HttpGet("GetByStaffId/{staffId}")]
        public async Task<ActionResult<IEnumerable<TeacherCourses>>> GetTeacherCoursesByStaffId(int staffId)
        {
            var teacherCourses = await _context.TeacherCourses
                .Where(tc => tc.StaffId == staffId)
                .ToListAsync();

            if (teacherCourses == null || teacherCourses.Count == 0)
            {
                return NotFound();
            }

            return teacherCourses;
        }


        // GET: api/TeacherCourses/GetBystaffIdAndCourseId/5/10
        [HttpGet("GetBystaffIdAndCourseId/{staffId}/{courseId}")]
        public async Task<ActionResult<TeacherCourses>> GetTeacherCourseBystaffIdAndCourseId(int staffId, int courseId)
        {
            var teacherCourse = await _context.TeacherCourses
                .FirstOrDefaultAsync(tc => tc.StaffId == staffId && tc.CourseId == courseId);

            if (teacherCourse == null)
            {
                return NotFound();
            }

            return teacherCourse;
        }


        // GET: /GetTeacherAssignedCourses/5
        [HttpGet("GetTeacherAssignedCourses/{staffId}")]
        public async Task<ActionResult<IEnumerable<Courses>>> GetTeacherAssignedCourses(int staffId)
        {
            var teacherAssignedCourses = await _context.TeacherCourses
                .Where(tc => tc.StaffId == staffId)
                .ToListAsync();

            if (teacherAssignedCourses == null || teacherAssignedCourses.Count == 0)
            {
                return NotFound();
            }

            var courseDetails = teacherAssignedCourses
                .Select(tc => new Courses
                {
                    CourseId = tc.CourseId,
                    CourseName = _context.Courses
                        .FirstOrDefault(c => c.CourseId == tc.CourseId)?.CourseName,
                    CourseDescription = _context.Courses
                        .FirstOrDefault(c => c.CourseId == tc.CourseId)?.CourseDescription,
                    CourseBranch = _context.Courses
                        .FirstOrDefault(c => c.CourseId == tc.CourseId)?.CourseBranch
                })
                .ToList();

            return courseDetails;
        }


        private bool TeacherCoursesExists(int id)
        {
            return _context.TeacherCourses.Any(e => e.id == id);
        }




    }
}