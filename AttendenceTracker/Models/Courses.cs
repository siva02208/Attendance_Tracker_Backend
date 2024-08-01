using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AttendenceTracker.Models
{
    public class Courses
    {
        [Required]
        [Key]
        public int CourseId { get; set; }
        
        [Required]
        public string CourseName { get; set; }

        public string CourseBranch { get; set; }
       
        [Required]
        public string CourseDescription { get; set; }

    }
}
