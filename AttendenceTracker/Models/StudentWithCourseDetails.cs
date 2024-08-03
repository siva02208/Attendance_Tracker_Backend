using System.ComponentModel.DataAnnotations;

namespace AttendenceTracker.Models
{
    public class StudentWithCourseDetails
    {
        
        [Required]
        public int StudentId { get; set; }

        [Required]
        public string StudentName { get; set; }

        public string StudentBranch { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public string CourseName { get; set; }

        public string CourseBranch { get; set; }

        [Required]
        public string CourseDescription { get; set; }
    }
}
