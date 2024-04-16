using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using CRUD.CustomFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Attendance_Tracking_System.Controllers
{
	[AuthFilter]
	public class StudentAffairsController : Controller
    {
        private readonly IEmployeeRepo employeeRepo;
        private readonly IStudentRepo studentRepo;
        private readonly ITrackRepo trackRepo;

        public StudentAffairsController(IEmployeeRepo employeeRepo,IStudentRepo studentRepo, ITrackRepo trackRepo)
        {
            this.employeeRepo = employeeRepo;
            this.studentRepo = studentRepo;
            this.trackRepo = trackRepo;
        }
        public IActionResult Index()
        {
            var studentAffaira = employeeRepo.GetAllStudentAffairs();
            return View(studentAffaira);
        }

        public IActionResult ViewProfile(int? id)
        {
            var emp = employeeRepo.GetByID(id.Value);
            if (emp != null)
            {
                return View(emp);
            }
            return NotFound();
        }

        public IActionResult EditProfile(int? id)
        {
            var emp = employeeRepo.GetByID(id.Value);
            if (emp != null)
            {
                return View(emp);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(Employee Emp, IFormFile? EmpImage)
        {
            if (ModelState.IsValid)
            {
                if (EmpImage != null)
                {
                    string filename = $"Emp {Emp.Id.ToString()}.{EmpImage.FileName.Split('.').Last()}";
                    // Saving the file to the wwwroot/images folder
                    string path = $"wwwroot/Images/{filename}";
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await EmpImage.CopyToAsync(stream);
                    }

                    Emp.UserImage = filename;
                    employeeRepo.Update(Emp);
                    return RedirectToAction("ViewProfile", "StudentAffairs", new { id = Emp.Id });
                }
                else
                {
                    employeeRepo.Update(Emp);
                    return RedirectToAction("ViewProfile", "StudentAffairs", new { id = Emp.Id });
                }
            }
            else
            {
                return View(Emp);
            }
        }

        // i dont know in track or what
        public IActionResult GetStudents()
        {
            var students = studentRepo.GetAll();  
                return View(students);
        }

       
        //public IActionResult EditStudentProfile(int ? id)
        //{
        //    if(id == null||id == 0)
        //        return View();
        //    var student = studentRepo.GetById(id.Value);
        //    if (student == null)
        //        return NotFound();  
        //    ViewBag.Tracks = trackRepo.GetAll();
        //    return View(student);
        //}
        //// again
        //[HttpPost]
        //public IActionResult EditStudentProfile(Student student)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        studentRepo.Update(student);
        //        return RedirectToAction("GetStudents");
        //    }
        //    ViewBag.Tracks = trackRepo.GetAll();
        //    return View(student);
        //}

      









    } 
}
