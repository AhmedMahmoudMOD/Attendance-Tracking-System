using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using CRUD.CustomFilters;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

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
			var AllTracks = repo.GetAllTracks();
			var AllPrograms = repo.GetAllPrograms();
			if (AllTracks != null)
			{
				ViewBag.Tracks = AllTracks;
			}
			if (AllPrograms != null)
			{
				ViewBag.Programs = AllPrograms;
			}

			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignUp(Student student, IFormFile Img, int TrackId)
		{
			ViewBag.Programs = repo.GetAllPrograms();

			if (Img == null || Img.Length == 0)
			{
				ModelState.AddModelError("Img", "Please select a file.");
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
				using (var fs = new FileStream(filePath, FileMode.Create))
				{
					await Img.CopyToAsync(fs);
				}
				if (repo.CheckEmailUniqueness(student.Email))
				{
					repo.RegisterStudent(student, fileName);
					repo.AssignRoleToUser(student.Id, 1);
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
		[HttpGet]
		public IActionResult GetTracksBasedOnPrograms(int programId)
		{

			var tracks = repo.GetTrackById(programId);

			var trackData = tracks.Select(t => new { value = t.Id, text = t.Name });

			return Json(trackData);

		}
		[HttpGet]
		public IActionResult CheckEmailUniqueness(string email)
		{
			if (!repo.CheckEmailUniqueness(email))
			{
				return Json(new { isUnique = false });
			}

			return Json(new { isUnique = true });
		}
	}
}
