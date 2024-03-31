using Attendance_Tracking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Tracking_System.Data
{
    public class ITISysContext : DbContext
    {
        public ITISysContext() { }

        public ITISysContext(DbContextOptions options) : base(options) { }

        public DbSet<ITIProgram> Program { get; set; }

        public DbSet<Intake> Intake { get; set; }

        public DbSet<Track> Track { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Student> Student { get; set; }

        public DbSet<Instructor> Instructor { get; set; }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<Attendance> Attendance { get; set; }

        public DbSet<StudentAttendance> StudentAttendance { get; set; }

        public DbSet<Schedule> Schedule { get; set; }

        public DbSet<Permission> Permission { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().UseTptMappingStrategy();

            modelBuilder.Entity<Attendance>().HasDiscriminator(a => a.AttendanceType).HasValue<Attendance>("StaffAttendance");

            modelBuilder.Entity<Student>().Property(e => e.AttendanceDegrees).HasDefaultValue(250);

            modelBuilder.Entity<Instructor>()
                .HasOne(e => e.SupTrack)
                .WithOne(e => e.Supervisor)
                .HasForeignKey<Track>(e => e.SuperID)
                .IsRequired();

            modelBuilder.Entity<Employee>().Property(e => e.Type).HasConversion<string>();

            modelBuilder.Entity<Permission>().Property(p => p.Type).HasConversion<string>();

            modelBuilder.Entity<Attendance>().Property(a=>a.AttendanceStatus).HasConversion<string>();  

            modelBuilder.Entity<Student>().Property(s=>s.RegisterationStatus).HasConversion<string>();

        }
    }
}
