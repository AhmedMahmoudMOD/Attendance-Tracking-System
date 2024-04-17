using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CRUD.CustomFilters;
using Microsoft.AspNetCore.Authorization;

namespace Attendance_Tracking_System.Controllers
{
	[AuthFilter]
	public class ProgramController : Controller
    {
        readonly IProgramRepo programRepo;
        readonly IIntakeRepo intakeRepo;
        readonly IInstructorRepo instructorRepo;
        readonly IStudentRepo studentRepo;
        readonly ITrackRepo trackRepo;


       public  ProgramController(IProgramRepo programRepo, IIntakeRepo intakeRepo , IInstructorRepo instructorRepo,
       IStudentRepo studentRepo , ITrackRepo trackRepo)
        {
            this.programRepo = programRepo;
            this.intakeRepo = intakeRepo;
            this.instructorRepo = instructorRepo;
            this.studentRepo = studentRepo;
            this.trackRepo = trackRepo;
        }
		[Authorize(Roles = "admin")]
		
		public IActionResult Show()
        {
            return View();
        }
		[Authorize(Roles = "admin")]
		public IActionResult Index()
        {
            var programms = programRepo.GetAll();  
            return View(programms);
        }
		[Authorize(Roles = "admin")]
		public IActionResult Create()
        {
            try
            {
                List<Track> tracks = trackRepo.GetAll() ?? new List<Track>();
                ViewBag.Tracks = tracks;

                List<Intake> intakes = intakeRepo.GetAll() ?? new List<Intake>();
                ViewBag.Intakes = intakes;

                List<Instructor> instructors = instructorRepo.GetAll() ?? new List<Instructor>();
                ViewBag.Instructors = instructors;

              
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error occurred while retrieving ViewBag data: {ex.Message}");
                return RedirectToAction("Error", "Home");
            }
        }
		[Authorize(Roles = "admin")]

		[HttpPost]
        public IActionResult Create(ITIProgram program)
        {
            if (program != null)
            {
                programRepo.Add(program);
              return  RedirectToAction("Index","Program");
            }
            // If ModelState is not valid, reload the view with data
            List<Track> tracks = trackRepo.GetAll();
            ViewBag.Tracks = tracks;
            List<Intake> intakes = intakeRepo.GetAll();
            ViewBag.Intakes = intakes;
            List<Instructor> instructors = instructorRepo.GetAll();
            ViewBag.Instructors = instructors;
            List<Student> students = studentRepo.GetAll();
            ViewBag.Students = students;
            return View(program);
        }
		[Authorize(Roles = "admin")]
		public IActionResult Delete(int ?id) {
            if (id == 0 ||id== null)
            {
                return BadRequest();
            }
           var program =programRepo.GetByID(id.Value);
            if (program == null)
            {
                return NotFound();
            }
            programRepo.Delete(id.Value);
            return RedirectToAction("Index", "Program");
        }
		[Authorize(Roles = "admin")]
		public IActionResult Update(int ?id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var program = programRepo.GetByID(id.Value);
            if (program == null) 
            {
                return NotFound();
            }
            List<Track> tracks = trackRepo.GetAll();
            ViewBag.Tracks = tracks;
            List<Intake> intakes = intakeRepo.GetAll();
            ViewBag.Intakes = intakes;
            return View(program);
        }
		[Authorize(Roles = "admin")]
		[HttpPost]
        public IActionResult Update(ITIProgram program) { 
            if(ModelState.IsValid)
            {
                programRepo.Update(program);
                return RedirectToAction("Index", "Program");
            }
            List<Track> tracks = trackRepo.GetAll();
            ViewBag.Tracks = tracks;
            List<Intake> intakes = intakeRepo.GetAll();
            ViewBag.Intakes = intakes;
            return View(program);
        }
		[Authorize(Roles = "admin")]
		public IActionResult ShowProgramTracks(int ? id)
        {
            var program = programRepo.GetByID(id.Value);
            return View(program);
        }
		[Authorize(Roles = "admin")]

		public IActionResult ShowProgramIntakes(int? id)
        {
            var program = programRepo.GetByID(id.Value);
            return View(program);
        }

		[Authorize(Roles = "admin")]
		public IActionResult ShowProgramInstructors(int? id) {
            var program = programRepo.GetByID(id.Value);
            return View(program);
        }
		[Authorize(Roles = "admin")]
		public IActionResult ShowProgramStudents(int? id)
        {
            var program = programRepo.GetByID(id.Value);
            return View(program);
        }
		[Authorize(Roles = "admin")]
		public IActionResult Details(int ?id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var program = programRepo.GetByID(id.Value);
            if (program == null)
            {
                return NotFound();
            }
            return View(program);
        }




    }
}
