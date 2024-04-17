using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using CRUD.CustomFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Security.Claims;

namespace Attendance_Tracking_System.Controllers
{
	[AuthFilter]
	public class StudentAffairsController : Controller
    {
        private readonly IEmployeeRepo employeeRepo;
        private readonly IStudentRepo studentRepo;
        private readonly ITrackRepo trackRepo;
        private readonly IAttendanceRepo attendanceRepo;

        public StudentAffairsController(IEmployeeRepo employeeRepo,IStudentRepo studentRepo, ITrackRepo trackRepo, IAttendanceRepo attendanceRepo)
        {
            this.employeeRepo = employeeRepo;
            this.studentRepo = studentRepo;
            this.trackRepo = trackRepo;
            this.attendanceRepo = attendanceRepo;
        }
		[Authorize(Roles = "StudentAffairs")]
		public IActionResult Index()
        {
            var studentAffaira = employeeRepo.GetAllStudentAffairs();
            return View(studentAffaira);
        }

		public User GetCurrentUser()
		{
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			int id = int.Parse(userId);
			User user = employeeRepo.GetUserById(id);
			return user;
		}
		[Authorize(Roles = "StudentAffairs")]
		public IActionResult ViewProfile()
        {
          
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			var role = identity?.FindFirst(ClaimTypes.Role)?.Value;
			ViewBag.role = role;
			var user = GetCurrentUser();
            return View(user);
			
        }
		[Authorize(Roles = "StudentAffairs")]
		public IActionResult EditProfile()
        {
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			var role = identity?.FindFirst(ClaimTypes.Role)?.Value;
			ViewBag.role = role;
			var user = GetCurrentUser();
			return View(user);
		}

        [
            HttpPost]
		[Authorize(Roles = "StudentAffairs")]
		public async Task<IActionResult> EditProfile(Employee Emp, IFormFile? EmpImage)
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
                    employeeRepo.Update(Emp);
                    return RedirectToAction("ViewProfile", "StudentAffairs", new { id = Emp.Id });
                }
                else
                {
                    employeeRepo.Update(Emp);
                    return RedirectToAction("ViewProfile", "StudentAffairs", new { id = Emp.Id });
                }
            }
            else
            {
                return View(Emp);
            }
        }
		[Authorize(Roles = "StudentAffairs")]
		public IActionResult GetStudents()
        {
            
            var students = studentRepo.GetStudentsAccepted();
            ViewBag.Tracks = trackRepo.GetAll();
                return View(students);
        }
		[Authorize(Roles = "StudentAffairs")]
		public IActionResult EditStudentProfile(int? id)
        {
            if (id == null || id == 0)
                return View();
            var student = studentRepo.GetById(id.Value);
            if (student == null)
                return NotFound();
            ViewBag.Tracks = trackRepo.GetAll();
            return View(student);
        }
        [HttpPost]
		[Authorize(Roles = "StudentAffairs")]
		public async Task<IActionResult> EditStudentProfile(Student student, IFormFile? EmpImage)
        {
            if (ModelState.IsValid)
            {
                if (EmpImage != null)
                {
                    string filename = $"Student {student.Id.ToString()}.{EmpImage.FileName.Split('.').Last()}";
                    // Saving the file to the wwwroot/images folder
                    string path = $"wwwroot/Images/{filename}";
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await EmpImage.CopyToAsync(stream);
                    }

                    student.UserImage = filename;
                    studentRepo.Update(student);
                    return RedirectToAction("GetStudents", "StudentAffairs", new { id = student.Id });
                }
                else
                {
                    studentRepo.Update(student);
                    return RedirectToAction("GetStudents", "StudentAffairs", new { id = student.Id });
                }
            }
            else
            {
                ViewBag.Tracks = trackRepo.GetAll();
                return View(student);
            }
        }
		[Authorize(Roles = "StudentAffairs")]
		public IActionResult ShowAttendance()
        {
            ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
            var role = identity?.FindFirst(ClaimTypes.Role)?.Value;
            ViewBag.role = role;
            var user = GetCurrentUser();
            return View(user);
        }
        [HttpPost]
		[Authorize(Roles = "StudentAffairs")]
		public IActionResult ShowAttendancePost(DateOnly startDate, DateOnly endDate)
        {
            // Check if both start date and end date are selected
            if (startDate != null && endDate != null)
            {
                var user = GetCurrentUser();
                var attendances = attendanceRepo.GetAttendanceRecords(user.Id, startDate, endDate);

                return View("_ShowAttendanceNewPartial", attendances);
            }
            else
            {
                return View("_ShowAttendanceNewPartial", new List<Attendance>()); // Return an empty list if dates are not selected
            }
        }
    }
}
