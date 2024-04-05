using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace Attendance_Tracking_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ITISysContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Azure")));
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
            builder.Services.AddScoped<IRegisterStudentRepo,RegisterRepo>();   
            builder.Services.AddScoped<IAdminRepo, AdminRepo>();
			//register auth type "cookie"
			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

			JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseStaticFiles();
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();

        }
    }
}
