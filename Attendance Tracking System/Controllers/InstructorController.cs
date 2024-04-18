using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using CRUD.CustomFilters;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
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
        IAdminRepo adminRepo;
        public InstructorController(IInstructorRepo _instructorRepo, ITrackRepo _trackRepo, IScheduleRepo _scheduleRepo, IStudentRepo _studentRepo, IPermissionRepo _permissionrepo, IAdminRepo _adminRepo)
        {
            instructorRepo = _instructorRepo;
            trackRepo = _trackRepo;
            scheduleRepo = _scheduleRepo;
            studentRepo = _studentRepo;
            permissionRepo = _permissionrepo;
            adminRepo = _adminRepo;
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
            ViewBag.AllTracks = trackRepo.GetAll();
            return View(new Instructor());
        }



        [Authorize(Roles = "admin")]
        [HttpPost]

        public async Task<IActionResult> Add(Instructor instructor, IFormFile InsImg)
        {
            if (ModelState.IsValid)
            {
                if (adminRepo.CheckEmailUniquenessForNewUsers(instructor.Email))
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
                else
                {
                    ModelState.AddModelError(nameof(instructor.Email), "Email already exists");
                }
            }

          
            ViewBag.AllTracks = trackRepo.GetAll();
            return View(instructor);
        }



      
        public int GetCurrentUserId()
        {
            ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int id = int.Parse(userId);
            User user = instructorRepo.GetInstructorById(id);
            return id;
        }


        [Authorize(Roles = "instructor,Supervisor")]
        [HttpGet]
        public IActionResult Edit()
        {
            int id = GetCurrentUserId();
            List<Role> roles = adminRepo.GetUserRoles(id);
            
            //foreach (var _role in roles)
            //{
            //    if (_role.RoleType=="admin")
            //    {
            //        id = int.Parse(RouteData.Values["id"].ToString());
            //        break;
            //    }
            //}
            var Instructor = instructorRepo.GetInstructorById(id);
            return View(Instructor);
        }


        [Authorize(Roles = "instructor,Supervisor,admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Instructor instructor, IFormFile InsImg)
        {
            if (ModelState.IsValid)
            {
                instructorRepo.EditInstructor(instructor);
                string DirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "instructor");
                string[] filePaths = Directory.GetFiles(DirectoryPath);
                string FoundFileName = string.Empty;
                string FoundFilpath = string.Empty;
                if (InsImg != null)
                {
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
                else
                {
                    var Old = instructorRepo.GetInstructorById(instructor.Id);
                    instructor.UserImage = Old.UserImage;
                    return View(instructor);
                }
            }
            else
            {
                return View(instructor);
            }
        }


        [Authorize(Roles = "instructor,Supervisor,admin")]
        public IActionResult Details()
        {
            int ID = GetCurrentUserId();
            bool found = false;
            var AllTracks = trackRepo.GetAll();
            ViewBag.AllTracks = AllTracks;
            foreach (var track in AllTracks)
            {
                if (track.SuperID == ID)
                {
                    found = true;
                    Track _track = trackRepo.getTrackById(track.Id);
                    ViewBag.TrackSupervised = _track;
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
            int id = GetCurrentUserId();
            HashSet<Schedule> sc = instructorRepo.getSheduleForTrack(id);
            ViewBag.Instructor = id;
            ViewBag.TrackSchedule = sc;
            var instructor = instructorRepo.GetInstructorById(id);
            // int trackid = db.Track.SingleOrDefault(a => a.SuperID == id).Id;

            var trackid = trackRepo.getTrackIDBySuperVisor(id);
            //ViewBag.trackid = trackid;
            return View(instructor);
        }


        [Authorize(Roles = "instructor,Supervisor")]

        public IActionResult AllTrackSchedules()
        {
            int id = GetCurrentUserId();
            HashSet<Schedule> sc = instructorRepo.getSheduleForTrack(id);
            return Json(sc);
        }



        [Authorize(Roles = "instructor,Supervisor")]
        [HttpPost]

        public IActionResult TrackSchedule(TimeOnly StartTime, DateOnly Date)
        {
            int id = GetCurrentUserId();
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
        public IActionResult WeeklyTable(DateOnly date)
        {
            int id = GetCurrentUserId();
            List<Schedule> WeeklySchedule = new List<Schedule>();
            WeeklySchedule = instructorRepo.getWeeklyTable(id, date);
            if (WeeklySchedule != null)
                return Json(WeeklySchedule);
            else
                return Json(null);
        }


        [Authorize(Roles = "instructor,Supervisor")]
        public IActionResult Permission()
        {
            int id = GetCurrentUserId();
            var Permissions = instructorRepo.GetPermissionsByTrack(id);
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
            int id = GetCurrentUserId();
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
            permissionRepo.UpdatePermissionAcceptance(per, acceptanceValue);

            return Json(" Permission ID: " + permissionId + "Acceptance Value: " + acceptanceValue);
        }
        [HttpGet]

        [Authorize(Roles = "instructor,Supervisor")]
        public IActionResult AddWeeklyShedule()

        {
            int id = GetCurrentUserId();
            var instructor = instructorRepo.GetInstructorById(id);
            //var trackid = db.Track.FirstOrDefault(a => a.SuperID == id).Id;
            var trackid = trackRepo.getTrackIDBySuperVisor(id);
            ViewBag.trackid = trackid;
            return View(instructor);
        }



		[HttpPost]
		[Authorize(Roles = "instructor,Supervisor")]
		public IActionResult AddWeeklyShedule(List<Schedule> schedules)
        {
            int id = GetCurrentUserId();
			var trackid = trackRepo.getTrackIDBySuperVisor(id);
			scheduleRepo.AddWeeklySchedules(schedules,trackid);
            return RedirectToAction("AddWeeklyShedule");
        }



		[Authorize(Roles = "instructor,Supervisor")]
		public IActionResult Attandence()
        {
            int id = GetCurrentUserId();
			var AttandenecRecords = instructorRepo.getAttandence(id);
            return View(AttandenecRecords);
        }
        public IActionResult EditInstructor(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id == -1)
            {
                return BadRequest();
            }

            var res = instructorRepo.GetInstructorById(id);
            return View(res);

        }
        [HttpPost]
        public async Task<IActionResult> EditInstructor(Instructor instructor)
        {
            if (!ModelState.IsValid)
            {
                return View(instructor);
            }
            else
            {

                instructorRepo.EditInstructor(instructor);
                return RedirectToAction("Index");
            }

        }
    }
}


