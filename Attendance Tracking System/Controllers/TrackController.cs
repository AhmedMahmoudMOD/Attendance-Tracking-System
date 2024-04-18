using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using CRUD.CustomFilters;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Office2019.Presentation;
using Microsoft.AspNetCore.Mvc;
using Track = Attendance_Tracking_System.Models.Track;

namespace Attendance_Tracking_System.Controllers
{
	[AuthFilter]
	[Authorize(Roles = "admin")]
	public class TrackController : Controller
	{
		ITrackRepo TrackRepo { get; set; }

         IProgramRepo programRepo { get; set; }
         IInstructorRepo InstructorRepo { get;  set; }
         IStudentRepo StudentRepo { get;  set; }

		 IAdminRepo adminRepo { get; set; }	

		//ITISysContext db =new ITISysContext();

        public TrackController(ITrackRepo _trackrepo,IProgramRepo _programRepo,IInstructorRepo _instructorRepo, IStudentRepo _studentRepo,IAdminRepo _adminrepo) { 
			TrackRepo = _trackrepo;
			programRepo = _programRepo;
			InstructorRepo =_instructorRepo;
			StudentRepo =_studentRepo;
			adminRepo = _adminrepo;
        }
		public IActionResult Index()
		{
			List<int>StudentCounter = new List<int>();
			var tracks = TrackRepo.getAllTracks(out StudentCounter);
			ViewBag.StudentCounter = StudentCounter;
			var Programs = programRepo.GetAllPrograms();
            ViewBag.Programs = Programs;
			

            return View(tracks);
		}

		public IActionResult ProgramTracks(int ProgramID)
		{
			var Tracks = TrackRepo.getTrackByProgram(ProgramID);
			return Json(Tracks);
		}

		public IActionResult Details (int Id)
		{
			var track= TrackRepo.getTrackById(Id);
			var SuperVisorID = track.SuperID;
			 if(SuperVisorID!=null)
			{
                var SuperVisor = InstructorRepo.GetInstructorById(SuperVisorID.Value);
				ViewBag.SuperVisorID = SuperVisor;
            }
		    var ProgramID =track.ProgramID;
			var Program = programRepo.GetByID(ProgramID.Value);
            ViewBag.Program = Program;
			var Intakes = track.Intakes;
            return View(track);
        }

		[HttpGet]
		public IActionResult Edit(int id)
		{
			var track = TrackRepo.getTrackById(id);
		 	var programs = programRepo.GetAllPrograms();
			ViewBag.programs=programs;
			var InsNotSuperVisor = TrackRepo.NotSuperVisor();
            var SuperVisorID = track.SuperID;
            var TrackSuperVisor = InstructorRepo.GetInstructorById(SuperVisorID.Value);
			InsNotSuperVisor.Add(TrackSuperVisor);
            ViewBag.InsNotSuperVisor = InsNotSuperVisor;
			var TrackInstructor = track.Instructors;
			var AllInstructor = InstructorRepo.GetAll();
			var NotTeachingIns=AllInstructor.Except(TrackInstructor);
			ViewBag.TrackInstructor=TrackInstructor;
			ViewBag.NotTeachingIns = NotTeachingIns;

            return View(track);
		}

        [HttpPost]
        public IActionResult Edit(Track track,List<int> RemovedIns,List<int> AddedIns)
        {
			if (ModelState.IsValid)
			{
				if (RemovedIns != null)
					TrackRepo.RemoveInsFromTrack(RemovedIns, track.Id);
				if (AddedIns != null)
					TrackRepo.AddInstructorToTrack(AddedIns, track.Id);
				TrackRepo.UpdateTrack(track);
				return RedirectToAction("index");
			}
			else
			{
				return View(track);	
			}
			
        }


		[HttpGet]
		public IActionResult Add()
		{
            var programs = programRepo.GetAllPrograms();
            var InsNotSuperVisor = TrackRepo.NotSuperVisor();
            var AllInstructor = InstructorRepo.GetAll();
            ViewBag.InsNotSuperVisor = InsNotSuperVisor;
            ViewBag.programs = programs;
            ViewBag.AllInstructor = AllInstructor;
            return View(new Track());
		}


		[HttpPost]
		public IActionResult Add(Track _track , List<int> AddedIns)
		{
			if(ModelState.IsValid)
			{
                TrackRepo.AddTrack(_track);
                if (AddedIns != null)
                    TrackRepo.AddInstructorToTrack(AddedIns, _track.Id);
				var superVisorID=_track.SuperID;
                return RedirectToAction("index");
            }
			else{
				var programs = programRepo.GetAllPrograms();
				var InsNotSuperVisor = TrackRepo.NotSuperVisor();
				var AllInstructor = InstructorRepo.GetAll();
				ViewBag.InsNotSuperVisor = InsNotSuperVisor;
				ViewBag.programs = programs;
				ViewBag.AllInstructor = AllInstructor;
				return View(new Track());
			}
		}


    }
}
