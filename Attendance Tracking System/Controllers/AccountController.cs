using Attendance_Tracking_System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Attendance_Tracking_System;
using Microsoft.EntityFrameworkCore;
using Attendance_Tracking_System.Enums;
using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Controllers
{
	public class AccountController : Controller
	{
		private readonly ITISysContext context;

		public AccountController(ITISysContext _context)
		{
			context = _context;
		}
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> login(LoginViewModel loginViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(loginViewModel);
			}
			var res = context.User.Include(a=>a.role).FirstOrDefault(a =>a!=null && a.Email == loginViewModel.email && a.Password == loginViewModel.password);
			if (res?.IsDeleted == true)
			{
				ModelState.AddModelError("UserIsDeleted", "Access to your account has been restricted.");
				return View(loginViewModel);
			}
			if (res == null)
			{
				ModelState.AddModelError("StudentNotFound", "Invalid Email or Password");
				return View(loginViewModel);
			}
			Claim claim = new Claim(ClaimTypes.Name, res.Name);
			Claim claim1 = new Claim(ClaimTypes.Email, res.Email);
			Claim claim3 = new Claim(ClaimTypes.NameIdentifier, res.Id.ToString());
			var std=context.Student.FirstOrDefault(a=>a.Id==res.Id && a.RegisterationStatus== RegisterationStatus.Pending);
			if(std != null)
			{
				ModelState.AddModelError("StudentNotFound", "Sorry Your Data Is Still pending");
				return View(loginViewModel);
			}
			List<Claim> claims = new List<Claim>();
			foreach (var item in res.role)
			{
				claims.Add(new Claim(ClaimTypes.Role, item.RoleType));
			}
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
			claimsIdentity.AddClaim(claim);
			claimsIdentity.AddClaim(claim1);
			claimsIdentity.AddClaim(claim3);
			claimsIdentity.AddClaims(claims);
			ClaimsPrincipal principal = new ClaimsPrincipal();
			principal.AddIdentity(claimsIdentity);
			await HttpContext.SignInAsync(principal);
			if (claimsIdentity.HasClaim(ClaimTypes.Role, "admin"))
			{
				return RedirectToAction("Profile", "Admin");
			}
			else if (claimsIdentity.HasClaim(ClaimTypes.Role, "student"))
			{
				return RedirectToAction("Index", "Student");
			}
			else if (claimsIdentity.HasClaim(ClaimTypes.Role, "instructor"))
			{
				return RedirectToAction("Details", "Instructor");
			}
			else if (claimsIdentity.HasClaim(ClaimTypes.Role, "StudentAffairs"))
			{
				return RedirectToAction("ViewProfile", "StudentAffairs");
			}
			else if (claimsIdentity.HasClaim(ClaimTypes.Role, "Supervisor"))
			{
				return RedirectToAction("Details", "Instructor");
			}
			else if (claimsIdentity.HasClaim(ClaimTypes.Role, "Security"))
			{
				return RedirectToAction("ViewProfile", "Security");
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
			//ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
			//string Role = identity.FindFirst(ClaimTypes.Role)?.Value;
			//Response.Cookies.Append("Id", res.Id.ToString());

			//return RedirectToAction("index", "student",new {id = res.Id});
		}
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return RedirectToAction("Login");
		}
	}
}
