using Attendance_Tracking_System.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Attendance_Tracking_System;
using Microsoft.EntityFrameworkCore;
using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Controllers
{
	public class AccountController : Controller
	{
		ITISysContext context = new ITISysContext();
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
			if (res == null)
			{
				ModelState.AddModelError("", "Invalid Email or Password");
				return View(loginViewModel);
			}
			Claim claim = new Claim(ClaimTypes.Name, res.Name);
			Claim claim1 = new Claim(ClaimTypes.Email, res.Email);
			Claim claim3 = new Claim(ClaimTypes.NameIdentifier, res.Id.ToString());
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
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;
            string Role = identity.FindFirst(ClaimTypes.Role)?.Value;
            return RedirectToAction("index", "student",new {id = res.Id});
		}
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return RedirectToAction("Login");
		}
	}
}
