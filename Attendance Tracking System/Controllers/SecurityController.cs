﻿using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using CRUD.CustomFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;

namespace Attendance_Tracking_System.Controllers
{
	[AuthFilter]
	public class SecurityController : Controller
    {
        private readonly IProgramRepo programRepo;
        private readonly IEmployeeRepo employeeRepo;
        private readonly IIntakeRepo intakeRepo;
        private readonly IStudentRepo studentRepo;
        private readonly IInstructorRepo instructorRepo;
        private int EmpId;
        public SecurityController(IProgramRepo programRepo,IEmployeeRepo employeeRepo,IIntakeRepo intakeRepo,IStudentRepo studentRepo  , IInstructorRepo instructorRepo)
        {
            this.programRepo = programRepo;
            this.employeeRepo = employeeRepo;
            this.intakeRepo = intakeRepo;
            this.studentRepo = studentRepo;
            this.instructorRepo = instructorRepo;
        }
        [HttpGet]
		[Authorize(Roles = "Security")]
		public IActionResult Index()
        {
            var plist = programRepo.GetAll();
            ViewBag.Programs = plist;
            var tlist = plist[0].Tracks;
            ViewBag.Tracks = tlist;
            var currentIntake = intakeRepo.GetCurrentIntake(plist[0].Id);
            ViewBag.Intake=currentIntake;

            return View();
        }
		[Authorize(Roles = "Security")]
		public IActionResult GetTracks(int id)
        {
            var target = programRepo.GetByID(id);
            if (target!=null)
            {
                var tlist = target.Tracks;
                return Json(tlist);

            }else { return Json(null); }
    
        }
		[Authorize(Roles = "Security")]
		public IActionResult GetCurrentIntake(int Pid)
        {
            var currentIntake = intakeRepo.GetCurrentIntake(Pid);
            return Json(currentIntake); 
        }
		[Authorize(Roles = "Security")]
		public IActionResult GetAttendanceList(int Pid,int Tid,int Ino) {

            var list = studentRepo.GetForAttendance(Pid, Tid, Ino);
            ViewBag.CurentTrackId = Tid;
            return PartialView("_StudentAttendancePartial",list);
        }
		[Authorize(Roles = "Security")]
		
		public IActionResult ViewProfile() {
			 EmpId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
			var model = employeeRepo.GetByID(EmpId);
            return View(model);
        
        }
		[Authorize(Roles = "Security")]
		
		public IActionResult EditProfile(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var model = employeeRepo.GetByID(id.Value);
            if(model == null) { 
               return NotFound();   
            }
            return View("EditProfile2",model); 
        }
        [HttpPost]
		[Authorize(Roles = "Security")]
		
		public async Task<IActionResult> EditProfile(Employee Emp , IFormFile? EmpImage)
        {
            if (ModelState.IsValid)
            {
                if (EmpImage != null)
                {
                    string filename = $"Emp {Emp.Id.ToString()}.{ EmpImage.FileName.Split('.').Last()}";
                     // Saving the file to the wwwroot/images folder
                    string path = $"wwwroot/Images/{filename}";
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await EmpImage.CopyToAsync(stream); 
                    }

                    Emp.UserImage = filename;
                    employeeRepo.Update(Emp);
                    return RedirectToAction("ViewProfile");
                }
                else
                {
                    employeeRepo.Update(Emp);
                    return RedirectToAction("ViewProfile");
                }
            }
            else
            {
                return View(Emp);
            }
        }
        /*
        [HttpPost]
		[Authorize(Roles = "Security")]
		public async Task<IActionResult> Add(Employee Emp, IFormFile? EmpImage)
        {
            if (ModelState.IsValid)
            {
                if (EmpImage != null)
                {
                    string filename = $"Emp {Emp.Id.ToString()}.{EmpImage.FileName.Split('.').Last()}";
                    // Saving the file to the wwwroot/images folder
                    string path = $"wwwroot/Images/{filename}";
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await EmpImage.CopyToAsync(stream);
                    }

                    Emp.UserImage = filename;
                    employeeRepo.Add(Emp);
                    return RedirectToAction("Index"); // Placeholder for now will be implemented later by admin
                }
                else
                {
                    employeeRepo.Add(Emp);
                    return RedirectToAction("Index"); // Placeholder for now will be implemented later by admin
                }
            }
            else
            {
                return View(Emp);
            }
        }
		[Authorize(Roles = "Security,StudentAffairs,admin")]
		
		public  IActionResult Delete(int id)
        {
            employeeRepo.Delete(id);
            return RedirectToAction("Index"); // placeholder for now will be implemented later by admin 
        }
        */

		[Authorize(Roles = "Security")]
        [HttpGet]
		public IActionResult StaffAttendance()
        {
            return View();
        }
		
		public IActionResult GetStaffAttendanceList(int TypeNo)
        {
            switch (TypeNo)
            {
                case 1:
                    var instlist = instructorRepo.GetForAttendance();
                    return PartialView("_StaffAttendancePartial", instlist);
                case 2:
                    var emplist = employeeRepo.GetForAttendance();
                    return PartialView("_StaffAttendancePartial", emplist);
                default:
                    return PartialView("_StaffAttendancePartial", null);
            }
        }
		[Authorize(Roles = "Security")]
		public IActionResult MyAttendance()
        {
            EmpId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var list = employeeRepo.GetAttendancesByEmpID(EmpId);
            return View(list);
        }
    }
}
