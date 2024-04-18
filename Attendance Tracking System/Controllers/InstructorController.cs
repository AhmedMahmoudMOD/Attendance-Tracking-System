﻿using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using CRUD.CustomFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Linq;
using System.Security.Claims;


namespace Attendance_Tracking_System.Controllers
{
   
	
	[AuthFilter]
	
	public class InstructorController : Controller
    {
        ITISysContext db = new ITISysContext();

        //ITISysContext db = new ITISysContext();
        IInstructorRepo instructorRepo;
        ITrackRepo trackRepo;
        IScheduleRepo scheduleRepo;
        IStudentRepo studentRepo;
        IPermissionRepo permissionRepo;
    
        public InstructorController(IInstructorRepo _instructorRepo, ITrackRepo _trackRepo, IScheduleRepo _scheduleRepo, IStudentRepo _studentRepo,IPermissionRepo _permissionrepo)
        {
            instructorRepo = _instructorRepo;
            trackRepo = _trackRepo;
            scheduleRepo = _scheduleRepo;
            studentRepo = _studentRepo;
            permissionRepo = _permissionrepo;
        }

		[Authorize(Roles = "instructor,Supervisor,admin")]
		public IActionResult Index()
        {
            // var Users=db.Instructor.ToList();
            var Instructors = instructorRepo.GetAllInstructors();
            //var per=db.Permission.ToList();
            // ViewBag.permission = per;

            return View(Instructors);
        }



		[Authorize(Roles = "admin")]
		[HttpGet]
        public IActionResult Add()
        {
            return View(new Instructor());
        }


		[Authorize(Roles = "admin")]
		[HttpPost]
        public async Task<IActionResult> Add(Instructor instructor, IFormFile InsImg)
        {
            instructorRepo.AddNewInstructor(instructor);
            var Tracks = trackRepo.GetAll();
            ViewBag.AllTracks = Tracks;
            string fileName = $"{instructor.Id}.{InsImg.FileName.Split(".").Last()}";
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "instructor");
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
		

		public User GetCurrentUser()
        {
            ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int id = int.Parse(userId);
            User user = instructorRepo.GetInstructorById(id);
            return user;
        }


		[Authorize(Roles = "instructor,Supervisor")]
		[HttpGet]
        public IActionResult Edit()
        {
            int id = GetCurrentUser().Id;
            var Instructor = instructorRepo.GetInstructorById(id);
            return View(Instructor);
        }


		[Authorize(Roles = "admin")]
		[HttpPost]
        public async Task<IActionResult> Edit(Instructor instructor, IFormFile InsImg)
        {
            instructorRepo.EditInstructor(instructor);
            string DirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "instructor");
            string[] filePaths = Directory.GetFiles(DirectoryPath);
            string FoundFileName = string.Empty;
            string FoundFilpath = string.Empty;

            foreach (string filePath in filePaths)
            {
                string filename = filePath.Replace(DirectoryPath + "\\", "");
                if (filename.StartsWith($"{instructor.Id}"))
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        fs.Close();
                        FoundFileName = filename;
                        FoundFilpath = filePath;
                        System.IO.File.Delete(filePath);
                    }
                    using (FileStream fs = new FileStream(FoundFilpath, FileMode.Create))
                    {
                        await InsImg.CopyToAsync(fs);
                        instructor.UserImage = FoundFileName;
                        instructorRepo.UpdateInstructorImage(FoundFileName, instructor.Id);
                    }
                    return View(instructor);
                }
                
            }
                FoundFileName = $"{instructor.Id}.{InsImg.FileName.Split(".").Last()}";
                FoundFilpath = Path.Combine(DirectoryPath, FoundFileName);
      
            using (FileStream fs = new FileStream(FoundFilpath, FileMode.Create))
            {
                await InsImg.CopyToAsync(fs);
                instructor.UserImage = FoundFileName;
                instructorRepo.UpdateInstructorImage(FoundFileName, instructor.Id);
            }
            return View(instructor);
        }
        



        public IActionResult Details()
        {
            int ID = GetCurrentUser().Id;
            bool found = false;
            var AllTracks=trackRepo.GetAll();
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

		[Authorize(Roles = "instructor,Supervisor")]
		[HttpGet]
        public IActionResult TrackSchedule()
        {
            int id = GetCurrentUser().Id;
            HashSet<Schedule> sc = instructorRepo.getSheduleForTrack(id);
            ViewBag.Instructor = id;
            ViewBag.TrackSchedule = sc;
            var instructor=instructorRepo.GetInstructorById(id);
           // int trackid = db.Track.SingleOrDefault(a => a.SuperID == id).Id;

            var trackid = trackRepo.getTrackIDBySuperVisor(id);
            //ViewBag.trackid = trackid;
            return View(instructor);
        }

		[Authorize(Roles = "instructor,Supervisor")]

		public IActionResult AllTrackSchedules()
        {
            int id = GetCurrentUser().Id;
            HashSet<Schedule> sc = instructorRepo.getSheduleForTrack(id);
            return Json(sc);
        }


		[Authorize(Roles = "instructor,Supervisor")]
		[HttpPost]
        public IActionResult TrackSchedule(TimeOnly StartTime, DateOnly Date)
        {
            int id = GetCurrentUser().Id;
            // int trackid = db.Track.SingleOrDefault(a => a.SuperID == id).Id;
            var trackid = trackRepo.getTrackIDBySuperVisor(id);
            // ViewBag.trackid = trackid;
            var instructor = instructorRepo.GetInstructorById(id);
            Schedule schedule = new Schedule
            {
                TrackID = trackid,
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
            return View(instructor);
        }

		[Authorize(Roles = "instructor,Supervisor")]
		public IActionResult WeeklyTable( DateOnly date) {
            int id = GetCurrentUser().Id;
            List<Schedule> WeeklySchedule = new List<Schedule>();
            WeeklySchedule=instructorRepo.getWeeklyTable(id,date);
            if(WeeklySchedule!=null)
            return Json (WeeklySchedule);
            else 
                return Json(null);  
        }
		[Authorize(Roles = "instructor,Supervisor")]
		public IActionResult Permission()
        {
            int id = GetCurrentUser().Id;
            var Permissions = instructorRepo.GetPermissionsByTrack( id);
            ViewBag.InstructorID = id;
            ViewBag.permission = Permissions;
            var instructor = instructorRepo.GetInstructorById(id);
            //var students=db.Student.ToList();

            var students = studentRepo.GetAll();
            ViewBag.students = students;
            return View(instructor);
        }
		[Authorize(Roles = "instructor,Supervisor")]
		public IActionResult PermissionBydate(DateOnly date)
        {
            int id = GetCurrentUser().Id;
            List<Permission> permissions = instructorRepo.GetPermissionsByTrack(id);
            var FliteredPer = instructorRepo.getPermissionsByDateAndTrack(date, permissions);
            return Json(FliteredPer);
        }
		/*----------------------------------------------------------------------------------------------*/
		[Authorize(Roles = "instructor,Supervisor")]
		[HttpPost]
		
        public ActionResult InstructorResponse(int permissionId, bool acceptanceValue)
        {
            var per = permissionRepo.getPermissionByID(permissionId); 
            permissionRepo.UpdatePermissionAcceptance(per,acceptanceValue);
           
            return Json(" Permission ID: " + permissionId + "Acceptance Value: " + acceptanceValue);
        }
		[HttpGet]

		[Authorize(Roles = "instructor,Supervisor")]
		public IActionResult AddWeeklyShedule()
        {
            int id = GetCurrentUser().Id;
            var instructor=instructorRepo.GetInstructorById(id);
            //var trackid = db.Track.FirstOrDefault(a => a.SuperID == id).Id;
            var trackid = trackRepo.getTrackIDBySuperVisor(id);
            ViewBag.trackid =trackid;
            return View(instructor);
        }

		[HttpPost]
		[Authorize(Roles = "instructor,Supervisor")]

		public IActionResult AddWeeklyShedule(List<Schedule> schedules)
        {
            int id = GetCurrentUser().Id;
            scheduleRepo.AddWeeklySchedules(schedules);
            return RedirectToAction("AddWeeklyShedule");
        }
		[Authorize(Roles = "instructor,Supervisor")]
		public IActionResult Attandence()
        {
            int id = GetCurrentUser().Id;
            var AttandenecRecords=instructorRepo.getAttandence(id);
            return View(AttandenecRecords);
        }
        

    }
}
