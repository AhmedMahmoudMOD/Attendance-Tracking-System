using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Attendance_Tracking_System.Controllers
{
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

        [
            HttpPost]
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
        public IActionResult Delete(int ?id)
        {
            if(id == null)
            {
                return BadRequest();    
            }
            var studentAffair=employeeRepo.GetByID(id.Value);
            if(studentAffair == null)
            {
                return NotFound();
            }
            employeeRepo.Delete(studentAffair.Id);
            return View(studentAffair);

        }
        public IActionResult GetStudents()
        {
            
            var students = studentRepo.GetStudentsAccepted();
            ViewBag.Tracks = trackRepo.GetAll();
                return View(students);
        }

        public IActionResult EditStudentProfile(int? id)
        {
            if (id == null || id == 0)
                return View();
            var student = studentRepo.GetById(id.Value);
            if (student == null)
                return NotFound();
            ViewBag.Tracks = trackRepo.GetAll();
            return View(student);
        }
        [HttpPost]
      
        public async Task<IActionResult> EditStudentProfile(Student student, IFormFile? EmpImage)
        {
            if (ModelState.IsValid)
            {
                if (EmpImage != null)
                {
                    string filename = $"Student {student.Id.ToString()}.{EmpImage.FileName.Split('.').Last()}";
                    // Saving the file to the wwwroot/images folder
                    string path = $"wwwroot/Images/{filename}";
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await EmpImage.CopyToAsync(stream);
                    }

                    student.UserImage = filename;
                    studentRepo.Update(student);
                    return RedirectToAction("GetStudents", "StudentAffairs", new { id = student.Id });
                }
                else
                {
                    studentRepo.Update(student);
                    return RedirectToAction("ViewProfile", "StudentAffairs", new { id = student.Id });
                }
            }
            else
            {
                ViewBag.Tracks = trackRepo.GetAll();
                return View(student);
            }
        }











    }
}
