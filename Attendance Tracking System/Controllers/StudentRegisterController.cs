using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Attendance_Tracking_System.Controllers
{
	public class StudentRegisterController : Controller
	{
		IRegisterStudentRepo repo;
		public StudentRegisterController(IRegisterStudentRepo _repo)
		{
			this.repo = _repo;
		}
		public IActionResult SignUp()
		{
			var AllTracks=repo.GetAllTracks();
			if (AllTracks!=null)
			{
				ViewBag.Tracks=AllTracks;	
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignUp(Student student, IFormFile Img,int TrackId)
		{ 
			if (Img == null || Img.Length == 0)
			{
				ModelState.AddModelError("Img", "Please select a file.");
				return View(student);
			}

			if (ModelState.IsValid)
			{
				if (repo.checkEmailUniqueness(student))
				{
					string fileName = $"{student.Id}.{Img.FileName}";
					string filePath = Path.Combine("wwwroot/images/", fileName);
					if (System.IO.File.Exists(filePath))
					{
						System.IO.File.Delete(filePath);
					}
					using (var fs = new FileStream(filePath, FileMode.Create))
					{
						await Img.CopyToAsync(fs);
					}
					repo.RegisterStudent(student, fileName);
					repo.AssignRoleToUser(student.Id,1);
                    return RedirectToAction("Pending");
				}
				else
				{
					ModelState.AddModelError(nameof(student.Email), "Email already exists");
				}
			}

			return View(student);
		}
		public IActionResult Pending(Student student)
		{
			return View();
		}
	}
}
