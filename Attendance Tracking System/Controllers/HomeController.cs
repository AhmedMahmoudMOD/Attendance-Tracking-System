using Attendance_Tracking_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Attendance_Tracking_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
			ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;

			// Retrieve specific claims from the identity
			string userId = identity.FindFirst(ClaimTypes.Role)?.Value;
            
			Console.WriteLine(userId);
			ViewBag.id = userId;
			return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
