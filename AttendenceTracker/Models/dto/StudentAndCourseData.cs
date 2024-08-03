using AttendenceTracker;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendenceTracker.Models.dto
{
    public class StudentAndCourseData
{
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public string StudentBranch { get; set; }
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public string CourseBranch { get; set; }
    public string CourseDescription { get; set; }
        }
}
