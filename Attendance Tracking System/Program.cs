using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Text.Json.Serialization;

namespace Attendance_Tracking_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBMAY9C3t2UFhhQlJBfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hTX5XdkRhW31YdXBRQ2Vd");

            builder.Services.AddDbContext<ITISysContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("Azure")));
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
            builder.Services.AddScoped<IAdminRepo, AdminRepo>();
            builder.Services.AddScoped<IUploadFile, UploadFileRepo>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            };
        //    builder.Services.AddControllersWithViews()
        //.AddJsonOptions(options =>
        //{
        //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        //});

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
