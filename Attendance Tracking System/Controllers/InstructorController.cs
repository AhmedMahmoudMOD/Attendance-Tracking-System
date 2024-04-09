using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Attendance_Tracking_System.Controllers
{
    public class InstructorController : Controller
    {
        // ITISysContext db = new ITISysContext();

       // ITISysContext db = new ITISysContext();
        IInstructorRepo instructorRepo;
        ITrackRepo trackRepo;
        IScheduleRepo scheduleRepo;
        public InstructorController(IInstructorRepo _instructorRepo, ITrackRepo _trackRepo,IScheduleRepo _scheduleRepo)
        {
            instructorRepo = _instructorRepo;
            trackRepo = _trackRepo;
            scheduleRepo = _scheduleRepo;   
        }


        public IActionResult Index()
        {
           // var Users=db.Instructor.ToList();
           var Instructors=instructorRepo.GetAllInstructors();
          
     
            return View(Instructors);
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View(new Instructor());
        }


        [HttpPost]
        public async Task<IActionResult> Add(Instructor instructor, IFormFile InsImg)
        {
            instructorRepo.AddNewInstructor(instructor);
               string fileName = $"{instructor.Id}.{InsImg.FileName.Split(".").Last()}";
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string filePath = Path.Combine(directoryPath, fileName);
            using (var fs = new FileStream(filePath, FileMode.CreateNew))
            {
                await InsImg.CopyToAsync(fs);
                instructor.UserImage = fileName;
                instructorRepo.UpdateInstructorImage(fileName, instructor.Id);
            }
            return RedirectToAction("index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Instructor = instructorRepo.GetInstructorById(id);
            return View(Instructor);
        }


        [HttpPost]
        public IActionResult Edit(Instructor instructor)
        {
            instructorRepo.EditInstructor(instructor);
            return RedirectToAction("index");
        }
        
        public IActionResult Details(int ID)
        {
            bool found = false;
            var AllTracks=trackRepo.getAllTracks();
            ViewBag.AllTracks = AllTracks;
            foreach(var track in AllTracks)
            {
                if (track.SuperID == ID)
                {
                    found = true;
                    Track _track = trackRepo.getTrackById(track.Id);
                    ViewBag.TrackSupervised=_track;
                    break;
                }
            }
            ViewBag.IsSuperVisor = found;
            var Instructor = instructorRepo.GetInstructorById(ID);
           
            return View(Instructor);
        }

        [HttpGet]
        public IActionResult TrackSchedule(int id)
        {
            HashSet<Schedule> sc = instructorRepo.getSheduleForTrack(id);
            ViewBag.Instructor = id;
            return View(sc);
        }

        public IActionResult AllTrackSchedules(int id)
        {
            HashSet<Schedule> sc = instructorRepo.getSheduleForTrack(id);
            return Json(sc);
        } 
        

        [HttpPost]
        public IActionResult TrackSchedule(TimeOnly StartTime, DateOnly Date, int TrackID,int id)
        {
            Schedule schedule = new Schedule
            {
                TrackID = TrackID,
                Date = Date,
                StartTime = StartTime
            };
            HashSet<Schedule> sc = instructorRepo.getSheduleForTrack(id);
            var existingSchedule = sc.FirstOrDefault(a => a.Date == Date);
            if (existingSchedule == null)
            {   
                scheduleRepo.AddsSchedule(schedule);
            }
            ViewBag.Instructor = id;
            return View(sc);
        }


        public IActionResult WeeklyTable(int id, DateOnly date) {

            List<Schedule> WeeklySchedule = new List<Schedule>();
            WeeklySchedule=instructorRepo.getWeeklyTable(id,date);
            
            return Json (WeeklySchedule);
        }

        

    }
}
