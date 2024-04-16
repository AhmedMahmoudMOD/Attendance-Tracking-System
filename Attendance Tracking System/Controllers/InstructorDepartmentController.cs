using CRUD.CustomFilters;
using Microsoft.AspNetCore.Mvc;

namespace Attendance_Tracking_System.Controllers
{
	[AuthFilter]
	public class InstructorDepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
