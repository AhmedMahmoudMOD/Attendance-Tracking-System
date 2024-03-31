using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Attendance_Tracking_System.Controllers
{
    public class SecurityController : Controller
    {
        private readonly IProgramRepo programRepo;
        private readonly IEmployeeRepo employeeRepo;

        public SecurityController(IProgramRepo programRepo,IEmployeeRepo employeeRepo)
        {
            this.programRepo = programRepo;
            this.employeeRepo = employeeRepo;
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

        public IActionResult ViewProfile() {
            int EmpId = 4;
            var model = employeeRepo.GetByID(EmpId);
            return View(model);
        
        }

        public IActionResult EditProfile(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var model = employeeRepo.GetByID(id.Value);
            if(model == null) { 
               return NotFound();   
            }
            return View(model); 
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(Employee Emp , IFormFile? EmpImage)
        {
            if (ModelState.IsValid)
            {
                if (EmpImage != null)
                {
                    string filename = $"Emp {Emp.Id.ToString()}.{ EmpImage.FileName.Split('.').Last()}";
                     // Saving the file to the wwwroot/images folder
                    string path = $"wwwroot/Images/{filename}";
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await EmpImage.CopyToAsync(stream); 
                    }

                    Emp.UserImage = filename;
                    employeeRepo.Update(Emp);
                    return RedirectToAction("ViewProfile");
                }
                else
                {
                    employeeRepo.Update(Emp);
                    return RedirectToAction("ViewProfile");
                }
            }
            else
            {
                return View(Emp);
            }
        }
    }
}
