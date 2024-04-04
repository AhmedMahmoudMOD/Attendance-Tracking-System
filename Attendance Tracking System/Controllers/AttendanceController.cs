using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Attendance_Tracking_System.View_Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;

namespace Attendance_Tracking_System.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IProgramRepo programRepo;
        private readonly IEmployeeRepo employeeRepo;
        private readonly IIntakeRepo intakeRepo;
        private readonly IStudentRepo studentRepo;
        private readonly IScheduleRepo scheduleRepo;
        private readonly IStudentAttendanceRepo studentAttendanceRepo;
        private readonly IAttendanceRepo attendanceRepo;
        private readonly IInstructorRepo instructorRepo;

        public AttendanceController(IProgramRepo programRepo, IEmployeeRepo employeeRepo, IIntakeRepo intakeRepo, IStudentRepo studentRepo,IScheduleRepo scheduleRepo , IStudentAttendanceRepo studentAttendanceRepo , IAttendanceRepo attendanceRepo , IInstructorRepo instructorRepo)
        {
            this.programRepo = programRepo;
            this.employeeRepo = employeeRepo;
            this.intakeRepo = intakeRepo;
            this.studentRepo = studentRepo;
            this.scheduleRepo = scheduleRepo;
            this.studentAttendanceRepo = studentAttendanceRepo;
            this.attendanceRepo = attendanceRepo;
            this.instructorRepo = instructorRepo;
        }
        [HttpPost]
        public IActionResult SetArrivalTime(StudentAttendance studentAttendance,int TrackID)
        {
            // get the schedule of the track for today
            var schedule = scheduleRepo.GetScheduleForToday(TrackID, DateOnly.FromDateTime(DateTime.Now));
            studentAttendance.ScheduleID = schedule.Id;
            studentAttendance.ArrivalTime = TimeOnly.FromDateTime(DateTime.Now);
            if (ModelState.IsValid)
            {
                // check if the arrival time is within five minutes of the scheduled time
                if (studentAttendance.ArrivalTime >= schedule.StartTime && studentAttendance.ArrivalTime <= schedule.StartTime.AddMinutes(5)||studentAttendance.ArrivalTime<schedule.StartTime)
                {
                    studentAttendance.AttendanceStatus = Enums.AttendanceStatus.Present;
                }
                else
                {
                    studentAttendance.AttendanceStatus = Enums.AttendanceStatus.Late;
                }
                studentAttendanceRepo.Add(studentAttendance);
                return Json(new { success = true });
            }else{                
                return Json(new { success = false });
            }


        }

        public IActionResult SetLeaveTime(LeaveTimeVM leaveTimeVM)
        {
            var studentAttendance = studentAttendanceRepo.GetStudentAttendance(leaveTimeVM.UserId, leaveTimeVM.Date);
            if (ModelState.IsValid)
            {
                studentAttendance.LeaveTime = TimeOnly.FromDateTime(DateTime.Now);
                studentAttendanceRepo.Update(studentAttendance);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });

            }
        }

        public IActionResult SetAbsent(StudentAttendance studentAttendance, int TrackID)
        {
            // get the schedule of the track for today
            var schedule = scheduleRepo.GetScheduleForToday(TrackID, DateOnly.FromDateTime(DateTime.Now));
            studentAttendance.ScheduleID = schedule.Id;
            if (ModelState.IsValid)
            {
                studentAttendance.AttendanceStatus = Enums.AttendanceStatus.Absent;
                studentAttendanceRepo.Add(studentAttendance);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }


        }

        public IActionResult SetStaffArrivalTime(Attendance attendance)
        {
 
            if (ModelState.IsValid)
            {
                attendance.AttendanceStatus = Enums.AttendanceStatus.Present;
                var timenow = TimeOnly.FromDateTime(DateTime.Now);
                attendance.ArrivalTime = timenow;

                attendanceRepo.Add(attendance);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }


        }

        public IActionResult SetStaffLeaveTime(LeaveTimeVM leaveTimeVM)
        {
            var attendance = attendanceRepo.GetAttendance(leaveTimeVM.UserId, leaveTimeVM.Date);
            if (ModelState.IsValid)
            {
                var timenow = TimeOnly.FromDateTime(DateTime.Now);
                attendance.LeaveTime = timenow;
                attendanceRepo.Update(attendance);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });

            }
        }

        public IActionResult SetStaffAbsent(Attendance attendance)
        {
           
            
            if (ModelState.IsValid)
            {
                attendance.AttendanceStatus = Enums.AttendanceStatus.Absent;
                attendanceRepo.Add(attendance);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }


        }

        public IActionResult ShowStudentsAttendance()
        {
            var plist = programRepo.GetAll();
            ViewBag.Programs = plist;
            var tlist = plist[0].Tracks;
            ViewBag.Tracks = tlist;
            var currentIntake = intakeRepo.GetCurrentIntake(plist[0].Id);
            ViewBag.Intake = currentIntake;

            return View("StudentsAttendance");

        }

        public IActionResult MarkStdAbsence(int Pid,int Tid , int Ino)
        {
            var Schedule = scheduleRepo.GetScheduleForToday(Tid, DateOnly.FromDateTime(DateTime.Now));
            var ScheduleID = Schedule.Id;
            var stdlist = studentRepo.GetForAttendance(Pid, Tid, Ino);

            if (studentAttendanceRepo.MarkAbsence(stdlist, ScheduleID))
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }

        }
        public IActionResult MarkStaffAbsence(int TypeNo)
        {
           switch (TypeNo)
            {
                case 1:
                    var instlist = instructorRepo.GetForAttendance();
                    if (attendanceRepo.MarkInstAbsence(instlist))
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                case 2:
                    var emplist = employeeRepo.GetForAttendance();
                    if (attendanceRepo.MarkEmpAbsence(emplist))
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                default:
                    return BadRequest();
            }

        }

        public IActionResult GetStudentsAttendace (int Pid, int Tid, int Ino,DateOnly Date)
        {
            var list = studentRepo.GetForAttendanceExplicit(Pid, Tid, Ino,Date);
            ViewBag.CurentTrackId = Tid;
            return PartialView("_StudentsAttListPartial", list);
        }

        public IActionResult GetRangeStudentsAttendace(int Pid, int Tid, int Ino, DateOnly Date,DateOnly EndDate)
        {
            var list = studentRepo.GetForRangeAttendanceExplicit(Pid, Tid, Ino, Date,EndDate);
            ViewBag.CurentTrackId = Tid;
            return PartialView("_StudentsRangeAttListPartial", list);
        }

        public IActionResult ShowStaffAttendance()
        {
            return View("StaffAttendance");
        }

        public IActionResult StudentsRangeAttendance()
        {
            var plist = programRepo.GetAll();
            ViewBag.Programs = plist;
            var tlist = plist[0].Tracks;
            ViewBag.Tracks = tlist;
            var currentIntake = intakeRepo.GetCurrentIntake(plist[0].Id);
            ViewBag.Intake = currentIntake;
            return View();
        }

        public IActionResult StaffRangeAttendance()
        {
            return View();
        }

        public IActionResult GetStaffAttendance(int TypeNo , DateOnly Date)
        {
            switch (TypeNo)
            {
                case 1:
                    var instlist = instructorRepo.GetForAttendanceExplicit(Date);
                    return PartialView("_InstAttendancePartial", instlist);
                case 2:
                    var emplist = employeeRepo.GetForAttendanceExplicit(Date);
                    return PartialView("_EmpAttendancePartial", emplist);
                default:
                    return BadRequest();
            }
        }

        public IActionResult GetRangeStaffAttendance(int TypeNo, DateOnly Date,DateOnly EndDate)
        {
            switch (TypeNo)
            {
                case 1:
                    var instlist = instructorRepo.GetForRangeAttendanceExplicit(Date,EndDate);
                    return PartialView("_InstAttendancePartial", instlist);
                case 2:
                    var emplist = employeeRepo.GetForRangeAttendanceExplicit(Date, EndDate);
                    return PartialView("_EmpAttendancePartial", emplist);
                default:
                    return BadRequest();
            }
        }
    }
}
