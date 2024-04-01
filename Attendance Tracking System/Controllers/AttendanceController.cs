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

        public AttendanceController(IProgramRepo programRepo, IEmployeeRepo employeeRepo, IIntakeRepo intakeRepo, IStudentRepo studentRepo,IScheduleRepo scheduleRepo , IStudentAttendanceRepo studentAttendanceRepo )
        {
            this.programRepo = programRepo;
            this.employeeRepo = employeeRepo;
            this.intakeRepo = intakeRepo;
            this.studentRepo = studentRepo;
            this.scheduleRepo = scheduleRepo;
            this.studentAttendanceRepo = studentAttendanceRepo;
        }
        [HttpPost]
        public IActionResult SetArrivalTime(StudentAttendance studentAttendance,int TrackID)
        {
            // get the schedule of the track for today
            var schedule = scheduleRepo.GetScheduleForToday(TrackID, DateOnly.FromDateTime(DateTime.Now));
            studentAttendance.ScheduleID = schedule.Id;
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
                studentAttendance.LeaveTime = leaveTimeVM.LeaveTime;
                studentAttendanceRepo.Update(studentAttendance);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });

            }
        }   
    }
}
