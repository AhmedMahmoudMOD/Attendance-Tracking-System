using CRUD.CustomFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance_Tracking_System.Controllers
{
	[AuthFilter]
	[Authorize(Roles = "instructor,Supervisor")]
	public class InstructorDepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
