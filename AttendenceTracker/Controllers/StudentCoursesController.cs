using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AttendenceTracker.Models;
using AttendenceTracker.Models.dto;

namespace AttendenceTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCoursesController : ControllerBase
    {
        private readonly AttendenceSystemDbContext _context;

        public StudentCoursesController(AttendenceSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/StudentCourses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentCourses>>> GetStudentCourses()
        {
            return await _context.StudentCourses.ToListAsync();
        }

        // GET: api/StudentCourses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentCourses>> GetStudentCourses(int id)
        {
            var studentCourses = await _context.StudentCourses.FindAsync(id);

            if (studentCourses == null)
            {
                return NotFound();
            }

            return studentCourses;
        }

        // PUT: api/StudentCourses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentCourses(int id, StudentCourses studentCourses)
        {
            if (id != studentCourses.id)
            {
                return BadRequest();
            }

            _context.Entry(studentCourses).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentCoursesExists(id))
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

        // POST: api/StudentCourses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentCourses>> PostStudentCourses(StudentCourses studentCourses)
        {
            _context.StudentCourses.Add(studentCourses);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentCourses", new { id = studentCourses.id }, studentCourses);
        }

        // DELETE: api/StudentCourses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentCourses(int id)
        {
            var studentCourses = await _context.StudentCourses.FindAsync(id);
            if (studentCourses == null)
            {
                return NotFound();
            }

            _context.StudentCourses.Remove(studentCourses);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentCoursesExists(int id)
        {
            return _context.StudentCourses.Any(e => e.id == id);
        }



        [HttpGet("StudentWithCourseDetails/{studentId}")]
        public async Task<ActionResult<IEnumerable<StudentWithCourseDetails>>> GetStudentWithCourseDetails(int studentId)
        {
            var studentWithCourses = await _context.Student
                .Where(s => s.StudentId == studentId)
                .Join(
                    _context.StudentCourses,
                    student => student.StudentId,
                    studentCourse => studentCourse.StudentId,
                    (student, studentCourse) => new { student, studentCourse }
                )
                .Join(
                    _context.Courses,
                    sc => sc.studentCourse.CourseId,
                    course => course.CourseId,
                    (sc, course) => new StudentWithCourseDetails
                    {
                        StudentId = sc.student.StudentId,
                        StudentName = sc.student.Name,
                        StudentBranch = sc.student.Branch,
                        CourseId = course.CourseId,
                        CourseName = course.CourseName,
                        CourseBranch = course.CourseBranch,
                        CourseDescription = course.CourseDescription
                    }
                )
                .ToListAsync();

            if (studentWithCourses == null || studentWithCourses.Count == 0)
            {
                return NotFound();
            }

            return studentWithCourses;
        }
        [HttpGet("StudentsWithCourseDetails")]
        public async Task<ActionResult<IEnumerable<StudentWithCourseDetails>>> GetStudentsWithCourseDetails()
        {
            var studentsWithCourses = await _context.Student
                .Join(
                    _context.StudentCourses,
                    student => student.StudentId,
                    studentCourse => studentCourse.StudentId,
                    (student, studentCourse) => new { student, studentCourse }
                )
                .Join(
                    _context.Courses,
                    sc => sc.studentCourse.CourseId,
                    course => course.CourseId,
                    (sc, course) => new StudentWithCourseDetails
                    {
                        StudentId = sc.student.StudentId,
                        StudentName = sc.student.Name,
                        StudentBranch = sc.student.Branch,
                        CourseId = course.CourseId,
                        CourseName = course.CourseName,
                        CourseBranch = course.CourseBranch,
                        CourseDescription = course.CourseDescription
                    }
                )
                .ToListAsync();

            if (studentsWithCourses == null || studentsWithCourses.Count == 0)
            {
                return NotFound();
            }

            return studentsWithCourses;
        }


        [HttpGet("GetByStaffId/{StaffId}")]
        public async Task<ActionResult<IEnumerable<StudentCourses>>> GetStudentCoursesByStaffId(int StaffId)
        {
            var studentCourses = await _context.StudentCourses
                .Where(sc => sc.StaffId == StaffId)
                .ToListAsync();

            if (studentCourses == null || studentCourses.Count == 0)
            {
                return NotFound();
            }

            return studentCourses;
        }


        [HttpGet("GetByCourseId/{courseId}")]
        public async Task<ActionResult<IEnumerable<StudentCourses>>> GetStudentCoursesByCourseId(int courseId)
        {
            var studentCourses = await _context.StudentCourses
                .Where(sc => sc.CourseId == courseId)
                .ToListAsync();

            if (studentCourses == null || studentCourses.Count == 0)
            {
                return NotFound();
            }

            return studentCourses;
        }


        [HttpGet("GetStudentAndCourseData/{staffId}")]
        public async Task<ActionResult<IEnumerable<StudentAndCourseData>>> GetStudentAndCourseData(int staffId)
        {
            var studentCourseData = await _context.StudentCourses
                .Where(sc => sc.StaffId == staffId)
                .Select(sc => new { sc.StudentId, sc.CourseId })
                .ToListAsync();

            if (studentCourseData == null || studentCourseData.Count == 0)
            {
                return NotFound();
            }

            var result = new List<StudentAndCourseData>();

            foreach (var item in studentCourseData)
            {
                var student = await _context.Student.FirstOrDefaultAsync(s => s.StudentId == item.StudentId);
                var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == item.CourseId);

                if (student != null && course != null)
                {
                    result.Add(new StudentAndCourseData
                    {
                        StudentId = student.StudentId,
                        StudentName = student.Name,
                        StudentBranch = student.Branch,
                        CourseId = course.CourseId,
                        CourseName = course.CourseName,
                        CourseBranch = course.CourseBranch,
                        CourseDescription = course.CourseDescription
                    });
                }
            }

            if (result.Count == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }
    

[HttpGet("GetByStudentIdAndStaffId/{studentId}/{StaffId}")]
        public async Task<ActionResult<IEnumerable<StudentCourses>>> GetStudentCoursesByStudentIdAndStaffId(int studentId, int StaffId)
        {
            var studentCourses = await _context.StudentCourses
                .Where(sc => sc.StudentId == studentId && sc.StaffId == StaffId)
                .ToListAsync();

            if (studentCourses == null || studentCourses.Count == 0)
            {
                return NotFound();
            }

            return studentCourses;
        }


        [HttpGet("GetByStaffIdAndCourseId/{StaffId}/{courseId}")]
        public async Task<ActionResult<IEnumerable<StudentCourses>>> GetStudentCoursesByStaffIdAndCourseId(int StaffId, int courseId)
        {
            var studentCourses = await _context.StudentCourses
                .Where(sc => sc.StaffId == StaffId && sc.CourseId == courseId)
                .ToListAsync();

            if (studentCourses == null || studentCourses.Count == 0)
            {
                return NotFound();
            }

            return studentCourses;
        }


        [HttpGet("GetByStudentIdAndCourseId/{studentId}/{courseId}")]
        public async Task<ActionResult<IEnumerable<StudentCourses>>> GetStudentCoursesByStudentIdAndCourseId(int studentId, int courseId)
        {
            var studentCourses = await _context.StudentCourses
                .Where(sc => sc.StudentId == studentId && sc.CourseId == courseId)
                .ToListAsync();

            if (studentCourses == null || studentCourses.Count == 0)
            {
                return NotFound();
            }

            return studentCourses;
        }


        [HttpGet("GetByStudentIdStaffIdAndCourseId/{studentId}/{StaffId}/{courseId}")]
        public async Task<ActionResult<IEnumerable<StudentCourses>>> GetStudentCoursesByStudentIdStaffIdAndCourseId(int studentId, int StaffId, int courseId)
        {
            var studentCourses = await _context.StudentCourses
                .Where(sc => sc.StudentId == studentId && sc.StaffId == StaffId && sc.CourseId == courseId)
                .ToListAsync();

            if (studentCourses == null || studentCourses.Count == 0)
            {
                return NotFound();
            }

            return studentCourses;
        }


        [HttpDelete("DeleteByStudentId/{studentId}")]
        public async Task<IActionResult> DeleteStudentCoursesByStudentId(int studentId)
        {
            var studentCourses = await _context.StudentCourses
                .Where(sc => sc.StudentId == studentId)
                .ToListAsync();

            if (studentCourses == null || studentCourses.Count == 0)
            {
                return NotFound();
            }

            _context.StudentCourses.RemoveRange(studentCourses);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("DeleteByStaffId/{StaffId}")]
        public async Task<IActionResult> DeleteStudentCoursesByStaffId(int StaffId)
        {
            var studentCourses = await _context.StudentCourses
                .Where(sc => sc.StaffId == StaffId)
                .ToListAsync();

            if (studentCourses == null || studentCourses.Count == 0)
            {
                return NotFound();
            }

            _context.StudentCourses.RemoveRange(studentCourses);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("DeleteByCourseId/{courseId}")]
        public async Task<IActionResult> DeleteStudentCoursesByCourseId(int courseId)
        {
            var studentCourses = await _context.StudentCourses
                .Where(sc => sc.CourseId == courseId)
                .ToListAsync();

            if (studentCourses == null || studentCourses.Count == 0)
            {
                return NotFound();
            }

            _context.StudentCourses.RemoveRange(studentCourses);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("DeleteByStudentIdAndStaffId/{studentId}/{StaffId}")]
        public async Task<IActionResult> DeleteStudentCoursesByStudentIdAndStaffId(int studentId, int StaffId)
        {
            var studentCourses = await _context.StudentCourses
                .Where(sc => sc.StudentId == studentId && sc.StaffId == StaffId)
                .ToListAsync();

            if (studentCourses == null || studentCourses.Count == 0)
            {
                return NotFound();
            }

            _context.StudentCourses.RemoveRange(studentCourses);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("DeleteByStaffIdAndCourseId/{StaffId}/{courseId}")]
        public async Task<IActionResult> DeleteStudentCoursesByStaffIdAndCourseId(int StaffId, int courseId)
        {
            var studentCourses = await _context.StudentCourses
                .Where(sc => sc.StaffId == StaffId && sc.CourseId == courseId)
                .ToListAsync();

            if (studentCourses == null || studentCourses.Count == 0)
            {
                return NotFound();
            }

            _context.StudentCourses.RemoveRange(studentCourses);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("DeleteByStudentIdAndCourseId/{studentId}/{courseId}")]
        public async Task<IActionResult> DeleteStudentCoursesByStudentIdAndCourseId(int studentId, int courseId)
        {
            var studentCourses = await _context.StudentCourses
                .Where(sc => sc.StudentId == studentId && sc.CourseId == courseId)
                .ToListAsync();

            if (studentCourses == null || studentCourses.Count == 0)
            {
                return NotFound();
            }

            _context.StudentCourses.RemoveRange(studentCourses);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("DeleteByStudentIdStaffIdAndCourseId/{studentId}/{StaffId}/{courseId}")]
        public async Task<IActionResult> DeleteStudentCoursesByStudentIdStaffIdAndCourseId(int studentId, int StaffId, int courseId)
        {
            var studentCourses = await _context.StudentCourses
                .Where(sc => sc.StudentId == studentId && sc.StaffId == StaffId && sc.CourseId == courseId)
                .ToListAsync();

            if (studentCourses == null || studentCourses.Count == 0)
            {
                return NotFound();
            }

            _context.StudentCourses.RemoveRange(studentCourses);
            await _context.SaveChangesAsync();

            return NoContent();
        }





        [HttpPost("AddStudentCoursesByStaff")]
        public async Task<IActionResult> AddStudentCoursesByStaff(StudentCourseStaffInput input)
        {
            try
            {
                // Iterate over the provided CourseIds and StudentIds
                foreach (var courseId in input.CourseIds)
                {
                    foreach (var studentId in input.StudentIds)
                    {
                        // Check if a record with the same StudentId, CourseId, and StaffId already exists
                        var existingRecord = await _context.StudentCourses.FirstOrDefaultAsync(sc =>
                            sc.StudentId == studentId &&
                            sc.CourseId == courseId &&
                            sc.StaffId == input.StaffId);

                        if (existingRecord == null)
                        {
                            // If no record exists, add a new one
                            var studentCourse = new StudentCourses
                            {
                                StudentId = studentId,
                                CourseId = courseId,
                                StaffId = input.StaffId
                            };
                            _context.StudentCourses.Add(studentCourse);
                        }
                        else
                        {
                            // Handle the case where the record already exists (e.g., log or skip)
                        }
                    }
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok("StudentCourses added successfully.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the process
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding StudentCourses.");
            }
        }






        [HttpGet("GetStudentsByStaffId/{staffId}")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByStaffId(int staffId)
        {
            try
            {
                // Find the list of StudentIds based on the StaffId in the StudentCourses table
                var studentIds = await _context.StudentCourses
                    .Where(sc => sc.StaffId == staffId)
                    .Select(sc => sc.StudentId)
                    .ToListAsync();

                if (studentIds.Any())
                {
                    // Fetch the student data from the Student table for each StudentId
                    var students = new List<Student>();

                    foreach (var studentId in studentIds)
                    {
                        var student = await _context.Student.FirstOrDefaultAsync(s => s.StudentId == studentId);
                        if (student != null)
                        {
                            students.Add(student);
                        }
                    }

                    if (students.Any())
                    {
                        // Return the list of student data as a response
                        return Ok(students);
                    }
                    else
                    {
                        return NotFound("No students found for the given StaffId");
                    }
                }
                else
                {
                    return NotFound("No students found for the given StaffId");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }



        [HttpGet("GetStaffByStudentId/{studentId}")]
        public async Task<ActionResult<IEnumerable<Staff>>> GetStaffByStudentId(int studentId)
        {
            try
            {
                // Find the list of StaffIds based on the StudentId in the StudentCourses table
                var staffIds = await _context.StudentCourses
                    .Where(sc => sc.StudentId == studentId)
                    .Select(sc => sc.StaffId)
                    .ToListAsync();

                if (staffIds.Any())
                {
                    // Fetch the staff data from the Staff table for each StaffId
                    var staffList = new List<Staff>();

                    foreach (var staffId in staffIds)
                    {
                        var staff = await _context.Staff.FirstOrDefaultAsync(s => s.StaffId == staffId);
                        if (staff != null)
                        {
                            staffList.Add(staff);
                        }
                    }

                    if (staffList.Any())
                    {
                        // Return the list of staff data as a response
                        return Ok(staffList);
                    }
                    else
                    {
                        return NotFound("No staff found for the given StudentId");
                    }
                }
                else
                {
                    return NotFound("No staff found for the given StudentId");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }





    }
}
