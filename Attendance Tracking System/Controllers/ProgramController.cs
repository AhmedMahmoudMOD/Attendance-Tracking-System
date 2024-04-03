﻿using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Attendance_Tracking_System.Controllers
{
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
        public IActionResult Index()
        {
            var programms = programRepo.GetAll();  
            return View(programms);
        }

        public IActionResult Create()
        {
            List<Track> tracks = trackRepo.GetAll();
            ViewBag.Tracks = tracks;
            List<Intake> intakes = intakeRepo.GetAll();
            ViewBag.Intakes = intakes;
            List<Instructor> instructors = instructorRepo.GetAll();
            ViewBag.Instructors = instructors;
            List<Student> students = studentRepo.GetAll();
            ViewBag.Students = students;
            return View();  
        }
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


    }
}