using Attendance_Tracking_System.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

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

		public DbSet<Admin> admin { get; set; }
		public DbSet<Instructor> Instructor { get; set; }
		public DbSet<Role> roles { get; set; }

		public DbSet<Employee> Employee { get; set; }
		public DbSet<Attendance> Attendance { get; set; }

		public DbSet<StudentAttendance> StudentAttendance { get; set; }

		public DbSet<Schedule> Schedule { get; set; }

		public DbSet<Permission> Permission { get; set; }

		public async Task BulkInsertStudentsAsync(List<Student> students)
		{
			await Student.AddRangeAsync(students);
			await SaveChangesAsync();
		}

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

			modelBuilder.Entity<Attendance>().Property(a => a.AttendanceStatus).HasConversion<string>();

			modelBuilder.Entity<Student>().Property(s => s.RegisterationStatus).HasConversion<string>();

			modelBuilder.Entity<Role>(entity =>
			{
				entity.HasMany(d => d.user).WithMany(p => p.role)
					.UsingEntity<Dictionary<string, object>>(
						"RoleUser",
						r => r.HasOne<User>().WithMany().HasForeignKey("usersId"),
						l => l.HasOne<Role>().WithMany().HasForeignKey("RolesId"),
						j =>
						{
							j.HasKey("RolesId", "usersId");
							j.ToTable("RoleUser");
							j.HasIndex(new[] { "usersId" }, "IX_RoleUser_usersId");
						});
			});
		}
	}
}
