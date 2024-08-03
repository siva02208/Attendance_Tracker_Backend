using AttendenceTracker.Models;
using System;
using System.Collections.Generic;


namespace AttendenceTracker.Helper
{
    public class AttendenceData
    {
        public Student Student { get; set; }
        public Staff Teacher { get; set; } 
        public DateTime OnDate { set; get; }

    }
}
