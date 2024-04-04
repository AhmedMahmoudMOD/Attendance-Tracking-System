using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Attendance_Tracking_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepo studentRepo;
        private readonly IAttendanceRepo AttendanceRepo;
        private readonly IPermissionRepo permissionRepo;

        public StudentController(IStudentRepo studentRepo, IAttendanceRepo _AttendanceRepo ,IPermissionRepo permissionRepo)
        {
            this.studentRepo = studentRepo;
            this.AttendanceRepo = _AttendanceRepo;
            this.permissionRepo = permissionRepo;
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
                var std = studentRepo.AddStudent(student);
                return RedirectToAction("Index", std);
            }
        }

        public IActionResult ShowAttendence(int id)
        {
            var attendance = AttendanceRepo.getAllAttendance(id);
            return View(attendance);
        }
        public IActionResult EditStd(int id)
        {
            var student = studentRepo.getStudentById(id);
            return View(student);
        }
        [HttpPost]
        public IActionResult EditStd(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }
            else
            {
                studentRepo.EditStudent(student);
                return RedirectToAction("Index", new { id = student.Id });
            }
        }
        [HttpGet]
        public IActionResult AddPermission(int id)
        {
            ViewBag.StudentID = id;
            return View();
        }
        [HttpPost]
        public IActionResult AddPermission(Permission permission)
        {
            if (!ModelState.IsValid)
            {
                return View(permission);
            }
            else
            {
               var per = permissionRepo.addPermission(permission);
               var std = studentRepo.getStudentById(per.StudentID);
                return View("Index",std);
            }
        
        }
        public IActionResult GetAllPermission(int id)
        {
            var permission = permissionRepo.getAllPermission(id);
            return View(permission);
        }
        public IActionResult RemovePermission(int Perid, int Stdid)
        {
            permissionRepo.removePermission(Perid);
            return RedirectToAction("GetAllPermission",Stdid);
        }
    }
}
