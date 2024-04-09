using Attendance_Tracking_System.Enums;
using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using CRUD.CustomFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;

namespace Attendance_Tracking_System.Controllers
{
	[AuthFilter]
	public class AdminController : Controller
	{
		IAdminRepo repo;
		public AdminController(IAdminRepo _repo)
		{
			repo = _repo;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Profile()
		{
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			var role = identity?.FindFirst(ClaimTypes.Role)?.Value;
			ViewBag.role=role;
			ViewBag.currentUser = GetCurrentUser();
			return View();
		}
		public IActionResult EditProfile()
		{
			var adminId = GetCurrentUser();
			var currentUser = repo.AdminData(adminId.Id);

			return View(currentUser);
		}
		[HttpPost]
		public async Task<IActionResult> EditProfile(Admin admin, IFormFile Img)
		{
			if (Img == null)
			{
				ModelState.AddModelError("Img", "Please select a file.");
				return View(admin);
			}
			if (ModelState.IsValid)
			{
				string fileName = $"{admin.Id}.{Img.FileName}";
				string filePath = Path.Combine("wwwroot/images/", fileName);

				if (System.IO.File.Exists(filePath))
				{
					System.IO.File.Delete(filePath);
				}
				using (var fs = new FileStream("wwwroot/images/" + fileName,
				FileMode.CreateNew))
				{
					await Img.CopyToAsync(fs);
					admin.UserImage = fileName;
					repo.uploadImg(fileName, admin.Id);
				}
				if (repo.CheckEmailUniqueness(admin.Email,admin.Id))
				{
					await repo.EditAdminData(admin);
					return RedirectToAction("Profile");
				}
				ModelState.AddModelError(nameof(admin.Email), "Email already exists");
			}
			return View(admin);
		}
		public User GetCurrentUser()
		{
			ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;
			var userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			int id = int.Parse(userId);
			User user = repo.AdminData(id);
			return user;
		}
		public IActionResult GetAllStudents()
		{
			var students = repo.GetStudents();

			return View(students);
		}
		public IActionResult StudentDetails(int? Id)
		{
			if (Id == null)
			{
				return NotFound();
			}
			if (Id == -1)
			{
				return BadRequest();
			}
			var student = repo.GetStudentById(Id.Value);
			return View(student);
		}

		public IActionResult DeleteStudent(int? Id)
		{
			if (Id == null)
			{
				return NotFound();
			}
			if (Id == -1)
			{
				return BadRequest();
			}
			var res = repo.DeleteStudent(Id.Value);
			if (res == 1)
			{
				TempData["ErrorMessage"] = "Student Deleted Successfully.";
				TempData["ShowAlert"] = true;
				return RedirectToAction("GetAllStudents");
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to delete student.";
				TempData["ShowAlert"] = true;
				return RedirectToAction("GetAllStudents");
			}

		}
		[HttpPost]
		public ActionResult UploadExcel(IFormFile file)
		{
			try
			{
				if (file != null && file.Length > 0)
				{
					string fileName = Path.GetFileName(file.FileName);
					string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);

					using (FileStream stream = new FileStream(filePath, FileMode.Create))
					{
						file.CopyTo(stream);
					}

					repo.ImportDataFromExcel(filePath);

					ViewBag.Message = "Bulk insert from Excel to database successful!";
				}
				else
				{
					ViewBag.Message = "No file uploaded.";
				}
			}
			catch (Exception ex)
			{
				ViewBag.Message = "Error: " + ex.Message;
			}

			return RedirectToAction("GetAllStudents");
		}
		public IActionResult UpdateStudent(int? Id)
		{
			ViewBag.Tracks = repo.GetAllTracks();
			ViewBag.programs = repo.GetAllPrograms();
			if (Id == null)
			{
				return NotFound();
			}
			if (Id == -1)
			{
				return BadRequest();
			}
			var res = repo.GetStudentById(Id);
			return View(res);
		}
		[HttpPost]
		public async Task<IActionResult> UpdateStudent(Student student, IFormFile Img)
		{
			ViewBag.Tracks = repo.GetAllTracks();
			if (!ModelState.IsValid)
			{
				return View(student);
			}
			if (ModelState.IsValid)
			{
				string fileName = $"{student.Id}.{Img.FileName}";
				string filePath = Path.Combine("wwwroot/images/", fileName);
				if (System.IO.File.Exists(filePath))
				{
					System.IO.File.Delete(filePath);
				}
				using (var fs = new FileStream("wwwroot/images/" + fileName,
				FileMode.CreateNew))
				{
					await Img.CopyToAsync(fs);
					student.UserImage = fileName;
					repo.uploadImg(fileName, student.Id);
				}
				repo.UpdateStudentData(student);
				return RedirectToAction("GetAllStudents");
			}
			return View(student);
		}
		public IActionResult GetEmployees()
		{
			var res = repo.GetAllEmployees();

			return View(res);
		}
		public IActionResult EmployeeDetails(int? Id)
		{
			if (Id == null)
			{
				return NotFound();
			}
			if (Id == -1)
			{
				return BadRequest();
			}
			var student = repo.GetEmployeeById(Id.Value);
			return View(student);
		}

		public IActionResult DeleteEmployee(int? Id)
		{
			if (Id == null)
			{
				return NotFound();
			}
			if (Id == -1)
			{
				return BadRequest();
			}
			var res = repo.DeleteEmployee(Id.Value);
			if (res == 1)
			{
				TempData["ErrorMessage"] = "The Employee Deleted Successfully.";
				TempData["ShowAlert"] = true;
				return RedirectToAction("GetEmployees");
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to delete Employee.";
				TempData["ShowAlert"] = true;
				return RedirectToAction("GetEmployees");
			}

		}

		public IActionResult UpdateEmployee(int? Id)
		{
			if (Id == null)
			{
				return NotFound();
			}
			if (Id == -1)
			{
				return BadRequest();
			}
			var res = repo.GetEmployeeById(Id);
			return View(res);
		}
		[HttpPost]
		public async Task<IActionResult> UpdateEmployee(Employee employee, IFormFile Img)
		{
			if (!ModelState.IsValid)
			{
				return View(employee);
			}

			if (ModelState.IsValid)
			{
				string fileName = $"{employee.Id}.{Img.FileName}";
				string filePath = Path.Combine("wwwroot/images/", fileName);
				if (System.IO.File.Exists(filePath))
				{
					System.IO.File.Delete(filePath);
				}
				using (var fs = new FileStream("wwwroot/images/" + fileName,
				FileMode.CreateNew))
				{
					await Img.CopyToAsync(fs);
					employee.UserImage = fileName;
					repo.uploadImg(fileName, employee.Id);
				}

				repo.UpdateEmployeeData(employee);
				var roleId = repo.GetRoleId(employee.Type.ToString());
				repo.UpdateUserRole(employee.Id, roleId);
				return RedirectToAction("GetEmployees");
			}
			return View(employee);
		}
		public IActionResult AddEmployee()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> AddEmployee(Employee employee, IFormFile Img)
		{

			if (Img == null || Img.Length == 0)
			{
				ModelState.AddModelError("Img", "Please select a file.");
				return View(employee);
			}

			if (ModelState.IsValid)
			{
				if (repo.CheckEmailUniquenessForNewUsers(employee.Email))
				{
					string fileName = $"{employee.Id}.{Img.FileName}";
					string filePath = Path.Combine("wwwroot/images/", fileName);
					if (System.IO.File.Exists(filePath))
					{
						System.IO.File.Delete(filePath);
					}
					using (var fs = new FileStream(filePath, FileMode.Create))
					{
						await Img.CopyToAsync(fs);
					}
					repo.AddEmployee(employee, fileName);
					var roleId = repo.GetRoleId(employee.Type.ToString());
					repo.AssignRoleToUser(employee.Id, roleId);
					return RedirectToAction("GetEmployees");
				}
				else
				{
					ModelState.AddModelError(nameof(employee.Email), "Email already exists");
				}
			}
			return View(employee);
		}
		public IActionResult GetAllIntakes()
		{
			var res = repo.GetIntakes();
			return View(res);
		}
		public IActionResult DeleteIntake(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			if (id == -1)
			{
				return BadRequest();
			}
			var deletionRes = repo.DeleteIntake(id);
			if (deletionRes == 1)
			{
				TempData["errorMessage"] = "Intake Deleted Successfully";
				TempData["showAlert"] = true;
				return RedirectToAction("GetAllIntakes");
			}
			else
			{
				return RedirectToAction("GetAllIntakes");
			}
		}
		public IActionResult EditIntake(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			if (id == -1)
			{
				return BadRequest();
			}
			var intake = repo.GetIntakeById(id);
			return View(intake);
		}
		[HttpPost]
		public IActionResult EditIntake(Intake intake)
		{

			if (ModelState.IsValid)
			{
				repo.updateIntake(intake);
				return RedirectToAction("GetAllIntakes");
			}
			return View(intake);
		}
		public IActionResult AddIntake()
		{
			ViewBag.progs= repo.GetITIPrograms();
			return View();
		}
		[HttpPost]
		public IActionResult AddIntake(Intake intake)
		{
			ViewBag.progs = repo.GetITIPrograms();
			if (ModelState.IsValid)
			{
				repo.AddIntake(intake);
				return RedirectToAction("GetAllIntakes");
			}
			return View();
		}
		public IActionResult GetDetails(int? id)
		{
			if(id==null)
			{
				return NotFound();
			}
			if (id == -1)
			{
				return BadRequest();
			}
			var intake=repo.GetIntakeById(id.Value);
			return View(intake);
		}
		[HttpGet]
		public IActionResult CheckEmailUniqueness(string email,int id)
		{
			if (!repo.CheckEmailUniqueness(email,id))
			{
				return Json(new { isUnique = false });
			}

			return Json(new { isUnique = true });
		}
		[HttpGet]
		public IActionResult CheckEmailUniquenessForNewUsers(string email)
		{
			if (!repo.CheckEmailUniquenessForNewUsers(email))
			{
				return Json(new { isUnique = false });
			}

			return Json(new { isUnique = true });
		}
	}
}
