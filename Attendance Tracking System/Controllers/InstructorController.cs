using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Linq;


namespace Attendance_Tracking_System.Controllers
{
    [Authorize(Roles="instructor")]
    public class InstructorController : Controller
    {
        ITISysContext db = new ITISysContext();

        //ITISysContext db = new ITISysContext();
        IInstructorRepo instructorRepo;
        ITrackRepo trackRepo;
        IScheduleRepo scheduleRepo;
        IStudentRepo studentRepo;
        public InstructorController(IInstructorRepo _instructorRepo, ITrackRepo _trackRepo, IScheduleRepo _scheduleRepo, IStudentRepo _studentRepo)
        {
            instructorRepo = _instructorRepo;
            trackRepo = _trackRepo;
            scheduleRepo = _scheduleRepo;
           studentRepo = _studentRepo;
        }


        public IActionResult Index()
        {
            // var Users=db.Instructor.ToList();
            var Instructors = instructorRepo.GetAllInstructors();
            //var per=db.Permission.ToList();
            // ViewBag.permission = per;

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


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Instructor = instructorRepo.GetInstructorById(id);
            var InstructorTracks = instructorRepo.getInstructorTracks(id);
            ViewBag.InstructorTracks = InstructorTracks;
            var Alltracks = trackRepo.GetAll();
            var ExceptTracks = Alltracks.Except(InstructorTracks).ToList();
            ViewBag.ExceptTracks = ExceptTracks;

            return View(Instructor);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(Instructor instructor, IFormFile InsImg)
        {
            instructorRepo.EditInstructor(instructor);
            //HashSet<Track> InstructorTrack = (HashSet<Track>)instructor.Tracks;
            //InstructorTrack.AddRange(AddedTracks);
            //foreach (var track in Removedtraks)
            //{
            //    InstructorTrack.Remove(track);
            //}
                

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
        



        public IActionResult Details(int ID)
        {
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




        [HttpGet]
        public IActionResult TrackSchedule(int id)
        {
            HashSet<Schedule> sc = instructorRepo.getSheduleForTrack(id);
            ViewBag.Instructor = id;
            ViewBag.TrackSchedule = sc;
            var instructor=instructorRepo.GetInstructorById(id);
           // int trackid = db.Track.SingleOrDefault(a => a.SuperID == id).Id;

            var trackid = trackRepo.getTrackIDBySuperVisor(id);
            //ViewBag.trackid = trackid;
            return View(instructor);
        }



        public IActionResult AllTrackSchedules(int id)
        {
            HashSet<Schedule> sc = instructorRepo.getSheduleForTrack(id);
            return Json(sc);
        } 
        



        [HttpPost]
        public IActionResult TrackSchedule(TimeOnly StartTime, DateOnly Date,int id)
        {
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


        public IActionResult WeeklyTable(int id, DateOnly date) {

            List<Schedule> WeeklySchedule = new List<Schedule>();
            WeeklySchedule=instructorRepo.getWeeklyTable(id,date);
            
            return Json (WeeklySchedule);
        }

        public IActionResult Permission(int id)
        {
            var Permissions = instructorRepo.GetPermissionsByTrack( id);
            ViewBag.InstructorID = id;
            ViewBag.permission = Permissions;
            var instructor = instructorRepo.GetInstructorById(id);
            //var students=db.Student.ToList();

            var students = studentRepo.GetAll();
            ViewBag.students = students;
            return View(instructor);
        }

        public IActionResult PermissionBydate(DateOnly date,int id)
        {
            List<Permission> permissions = instructorRepo.GetPermissionsByTrack(id);
            var FliteredPer = instructorRepo.getPermissionsByDateAndTrack(date, permissions);
            return Json(FliteredPer);
        }


        [HttpPost]
        public ActionResult InstructorResponse(int permissionId, bool acceptanceValue)
        {
            var per = db.Permission.SingleOrDefault(a => a.PermissionID == permissionId);
            per.IsAccepted = acceptanceValue;
            db.SaveChanges();
            return Json(" Permission ID: " + permissionId + "Acceptance Value: " + acceptanceValue);
        }


        [HttpGet]
        public IActionResult AddWeeklyShedule(int id)
        {
            var instructor=instructorRepo.GetInstructorById(id);
            //var trackid = db.Track.FirstOrDefault(a => a.SuperID == id).Id;
            var trackid = trackRepo.getTrackIDBySuperVisor(id);
            ViewBag.trackid =trackid;
            return View(instructor);
        }



        [HttpPost]
        public IActionResult AddWeeklyShedule(List<Schedule> schedules,int id)
        {
            db.Schedule.AddRange(schedules);
            db.SaveChanges();
            return RedirectToAction("AddWeeklyShedule");
        }

        

    }
}
