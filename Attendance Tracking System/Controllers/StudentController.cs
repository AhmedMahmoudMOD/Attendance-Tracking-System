using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Microsoft.AspNetCore.Mvc;
using Attendance_Tracking_System.Data;


namespace Attendance_Tracking_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepo studentRepo;
        private readonly IStudentAttendanceRepo studentAttendanceRepo;
        public StudentController(IStudentRepo studentRepo,IStudentAttendanceRepo studentAttendanceRepo) { 
            this.studentRepo = studentRepo;
            this.studentAttendanceRepo = studentAttendanceRepo;
        }
        public IActionResult Index(int id)
        {
            var student = studentRepo.getStudentById(id);
            return View(student);
        }
        public IActionResult AddStd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddStd(Student student)
        {
            if (!ModelState.IsValid)
            {
               return View(student);
            }
            else
            {
                studentRepo.addStudent(student);
                return RedirectToAction("Index", new { id = student.Id });
            }
        }
        public IActionResult ShowAttendence(int id)
        {
            var attendance = studentAttendanceRepo.getAllAttendance(id);
            return View(attendance);
        }
    }
}
