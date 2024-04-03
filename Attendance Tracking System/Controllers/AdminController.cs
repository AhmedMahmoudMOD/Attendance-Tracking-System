using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Attendance_Tracking_System.Controllers
{
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
				if (repo.CheckEmailUniqueness(admin))
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
			ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
			var userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			int id = int.Parse(userId);
			User user = repo.AdminData(id);
			return user;
		}
	}
}
