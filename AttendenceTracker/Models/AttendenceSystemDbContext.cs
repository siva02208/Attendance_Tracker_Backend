using Microsoft.EntityFrameworkCore;
using AttendenceTracker;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Threading;
using AttendenceTracker.Models;

namespace AttendenceTracker.Models
{
    public partial class AttendenceSystemDbContext : DbContext
    {
        public AttendenceSystemDbContext()
        {
        }

        public AttendenceSystemDbContext(DbContextOptions<AttendenceSystemDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attendence> Attendence { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public DbSet<AttendenceTracker.Models.Courses> Courses { get; set; }
        public DbSet<AttendenceTracker.Models.TeacherCourses> TeacherCourses { get; set; }
        public DbSet<AttendenceTracker.Models.StudentCourses> StudentCourses { get; set; }
        public DbSet<AttendenceTracker.Models.Leave> Leave { get; set; }
    }
}



