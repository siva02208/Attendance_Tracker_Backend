﻿using AttendenceTracker;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace AttendenceTracker.Models
{
    public partial class Staff
    {

        public int StaffId { get; set; }
        [MaxLength(20)]
        [MinLength(5)]
        public string StaffName { get; set; }
     
        public int MobileNumber { get; set; }
        public string Gender { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}



