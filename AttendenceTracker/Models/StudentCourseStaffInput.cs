using System.Collections.Generic;

namespace AttendenceTracker.Models
{
    public class StudentCourseStaffInput
    {
        public List<int> CourseIds { get; set; }
        public List<int> StudentIds { get; set; }
        public int StaffId { get; set; }
    }

}
