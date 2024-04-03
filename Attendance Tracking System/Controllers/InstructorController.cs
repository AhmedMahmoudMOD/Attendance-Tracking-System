using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Tracking_System.Controllers
{
    public class InstructorController : Controller
    {
        // ITISysContext db = new ITISysContext();
        IInstructorRepo instructorRepo;
        ITrackRepo trackRepo;
        public InstructorController(IInstructorRepo _instructorRepo, ITrackRepo _trackRepo)
        {
            instructorRepo = _instructorRepo;
            trackRepo = _trackRepo;
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
            var tracks=Instructor.Tracks.ToList();  
            return View(Instructor);
        }
        public IActionResult TrackSchedule(int id)
        {
            HashSet<Schedule> sc = instructorRepo.getSheduleForTrack(id);
            return View(sc);
        }

    }
}
