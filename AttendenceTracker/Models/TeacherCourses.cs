

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AttendenceTracker;

namespace AttendenceTracker.Models
{
    public class TeacherCourses
    {
        [Required]
        [Key]
        public int id { get; set; } 

        public int CourseId { get; set; }

        public int StaffId { get; set; }
   

    }
}
