using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Attendance_Tracking_System.View_Models;
using Microsoft.AspNetCore.Mvc;

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

        public AttendanceController(IProgramRepo programRepo, IEmployeeRepo employeeRepo, IIntakeRepo intakeRepo, IStudentRepo studentRepo,IScheduleRepo scheduleRepo , IStudentAttendanceRepo studentAttendanceRepo , IAttendanceRepo attendanceRepo )
        {
            this.programRepo = programRepo;
            this.employeeRepo = employeeRepo;
            this.intakeRepo = intakeRepo;
            this.studentRepo = studentRepo;
            this.scheduleRepo = scheduleRepo;
            this.studentAttendanceRepo = studentAttendanceRepo;
            this.attendanceRepo = attendanceRepo;
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

        public IActionResult GetStudentsAttendace (int Pid, int Tid, int Ino,DateOnly Date)
        {
            var list = studentRepo.GetForAttendanceExplicit(Pid, Tid, Ino,Date);
            ViewBag.CurentTrackId = Tid;
            return PartialView("_StudentsAttListPartial", list);
        }
    }
}
