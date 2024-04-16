using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Attendance_Tracking_System.View_Models;
using CRUD.CustomFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace Attendance_Tracking_System.Controllers
{
	[AuthFilter]
	public class StudentController : Controller
    {
        private readonly IStudentRepo studentRepo;
        private readonly IAttendanceRepo AttendanceRepo;
        private readonly IPermissionRepo permissionRepo;

        public StudentController(IStudentRepo studentRepo, IAttendanceRepo _AttendanceRepo ,IPermissionRepo permissionRepo)
        {
            this.studentRepo = studentRepo;
            this.AttendanceRepo = _AttendanceRepo;
            this.permissionRepo = permissionRepo;
        }
       
            //private int? GetUserIdFromCookie()
            //{
            //    // Retrieve the value of the "UserId" cookie
            //    string userIdString = HttpContext.Request.Cookies["Id"];

            //    // Check if the cookie exists and has a value
            //    if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
            //    {
            //        // User ID retrieved successfully
            //        return userId;
            //    }
            //    else
            //    {
            //        // Cookie does not exist or has no value, or the value is not a valid integer
            //        return null;
            //    }
            //}

            public IActionResult Index()
            {
            // var id = GetUserIdFromCookie();
            var id = GetCurrentUser();


				if (id!=null)
                {
                    var student = studentRepo.GetStudentById(id);
                    if (student != null)
                    {
                        return View(student);
                    }
                    else
                    {
                        return Content("Student not found.");
                    }
                }
                else
                {
                    return Content("User ID cookie not found, empty, or not a valid integer.");
                }
            }

        //public IActionResult AddStd()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult AddStd(Student student)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(student);
        //    }
        //    else
        //    {
        //        var std = studentRepo.AddStudent(student);
        //        return RedirectToAction("Index", std);
        //    }
        //}

        public IActionResult ShowAttendence(int id)
        {
            var attendance = AttendanceRepo.getAllAttendance(id);
            return View(attendance);
        }
		public IActionResult EditStd()
		{
			var id = GetCurrentUser();
			var student = studentRepo.GetStudentById(id);
			EditStudentViewModel editStudentViewModel = new EditStudentViewModel
			{
				Id = student.Id,
				Name = student.Name,
				Email = student.Email,
				Image = student.Image,
				ImagePath = student.UserImage,
				Password = student.Password,

			};
			return View(editStudentViewModel);
		}
		[HttpPost]
        public async Task<IActionResult> EditStd(EditStudentViewModel student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }
            else
            {
               await studentRepo.EditStudent(student);
                return RedirectToAction("Index", new { id = student.Id });
            }
        }
        [HttpGet]
        public IActionResult AddPermission()
        {
            ViewBag.StudentID = GetCurrentUser();
            return View();
        }
        [HttpPost]
        public IActionResult AddPermission(Permission permission)
        {
           
               var per = permissionRepo.addPermission(permission);
               var std = studentRepo.GetStudentById(per.StudentID);
                return View("Index",std);
            
        
        }
        public IActionResult GetAllPermission()
        {
            var id = GetCurrentUser();
            var permission = permissionRepo.getAllPermission(id);
            return View(permission);
        }
        public IActionResult RemovePermission(int Perid, int Stdid)
        {
            permissionRepo.removePermission(Perid);
            return RedirectToAction("GetAllPermission",Stdid);
        }
		public int GetCurrentUser()
		{
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			int id = int.Parse(userId);
			
			return id;
		}
	}
}
