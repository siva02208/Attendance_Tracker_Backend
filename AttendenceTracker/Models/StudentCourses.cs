using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace AttendenceTracker.Models

{
    public class StudentCourses
    {
        [Required]
        [Key]
        public int id { get; set; }

        public int StudentId { get; set; }

        public int StaffId { get; set; }

        public int CourseId { get; set; }



    }
}
