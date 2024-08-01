using AttendenceTracker;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendenceTracker.Models
{
    public partial class Attendence
    {
        [Required]
        [Key]
        public int Sno { get; set; }
        [ForeignKey("StudentId")]
        public int StudentId { get; set; }

        [ForeignKey("StaffId")]
        public int StaffId { get; set; }
        public DateTime? Date { get; set; }
        [Required]
        public string PresentStatus { get; set; }

    }
}


