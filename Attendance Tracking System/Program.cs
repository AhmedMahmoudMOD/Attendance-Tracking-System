using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Tracking_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ITISysContext>(options => options.UseSqlServer("Data Source=.;Initial Catalog=ITISys;Integrated Security=True;Encrypt=False;Trust Server Certificate=True"));
            builder.Services.AddScoped<IStudentRepo, StudentRepo>();
            builder.Services.AddScoped<IInstructorRepo, InstructorRepo>();
            builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
            builder.Services.AddScoped<IProgramRepo, ProgramRepo>();
            builder.Services.AddScoped<ITrackRepo, TrackRepo>();
            builder.Services.AddScoped<IIntakeRepo, IntakeRepo>();
            builder.Services.AddScoped<IAttendanceRepo, AttendacneRepo>();
            builder.Services.AddScoped<IStudentAttendanceRepo, StudentAttendanceRepo>();
            builder.Services.AddScoped<IScheduleRepo, ScheduleRepo>(); 
            builder.Services.AddScoped<IPermissionRepo, PermissionRepo>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
