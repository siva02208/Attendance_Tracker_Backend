using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;

namespace AttendenceTracker.Models
{
    public class Leave
    {
        [Key]
        [Required]
        public int Id { set; get; }
        [Required]
        public int StudentId { set; get; }
        [Required]
        public DateTime FromDate { set; get; }
        [Required]
        public DateTime ToDate { set;get; }
        [Required]
        public int TeacherId { get; set; }
        [Required]
        public string LeaveDescription { set; get;}
        [Required]
        public string LeaveStatus { set; get; }

    }
}
