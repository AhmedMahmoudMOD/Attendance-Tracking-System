using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using CRUD.CustomFilters;
using Microsoft.AspNetCore.Mvc;

namespace Attendance_Tracking_System.Controllers
{
	[AuthFilter]
	public class ScheduleController : Controller
    {
        IScheduleRepo scheduleRepo;
        ITrackRepo trackRepo;
      

        
        public ScheduleController(IScheduleRepo _scheduleRepo,ITrackRepo _trackRepo) {
            scheduleRepo = _scheduleRepo;
            trackRepo = _trackRepo;
        } 
        public IActionResult Index()
        {
            var schedules = scheduleRepo.GetAllSchedules();
            return View(schedules);
        }

        public IActionResult Details(int ID) 
        {
            Schedule schedule=scheduleRepo.GetScheduleById(ID);    
            return View(schedule);
        }

       

       
    }
}
