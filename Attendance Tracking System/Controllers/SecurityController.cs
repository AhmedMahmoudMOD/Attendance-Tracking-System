using Attendance_Tracking_System.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Attendance_Tracking_System.Controllers
{
    public class SecurityController : Controller
    {
        private readonly IProgramRepo programRepo;

        public SecurityController(IProgramRepo programRepo)
        {
            this.programRepo = programRepo;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var list = programRepo.GetAll();
            ViewBag.Programs = list;
            var tlist = list[0].Tracks;
            ViewBag.Tracks = tlist;
            return View();
        }

        public IActionResult GetTracks(int id)
        {
            var target = programRepo.GetByID(id);
            if (target!=null)
            {
                var tlist = target.Tracks;
                return Json(tlist);

            }else { return Json(null); }
           

        }
    }
}
