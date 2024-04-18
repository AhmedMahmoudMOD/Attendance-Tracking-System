using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Attendance_Tracking_System.View_Models;
using CRUD.CustomFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Attendance_Tracking_System.Controllers
{
	[AuthFilter]
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
		[Authorize(Roles = "Security")]
		[HttpPost]
        public IActionResult SetArrivalTime(StudentAttendance studentAttendance,int TrackID)
        {
            // get the schedule of the track for today
            var schedule = scheduleRepo.GetScheduleForToday(TrackID, DateOnly.FromDateTime(DateTime.Now));
            studentAttendance.ScheduleID = schedule?.Id;
            studentAttendance.ArrivalTime = TimeOnly.FromDateTime(DateTime.Now);
            studentAttendance.IsMarked = false;
            if (ModelState.IsValid)
            {
                // check if the arrival time is within five minutes of the scheduled time
                if (schedule != null)
                {
                    if (studentAttendance.ArrivalTime >= schedule.StartTime && studentAttendance.ArrivalTime <= schedule.StartTime.AddMinutes(5) || studentAttendance.ArrivalTime < schedule.StartTime)
                    {
                        studentAttendance.AttendanceStatus = Enums.AttendanceStatus.Present;
                    }
                    else
                    {
                        studentAttendance.AttendanceStatus = Enums.AttendanceStatus.Late;
                    }
                }
                else
                {
                    studentAttendance.AttendanceStatus = Enums.AttendanceStatus.Present;
                }
                studentAttendanceRepo.Add(studentAttendance);
                return Json(new { success = true });
            }else{                
                return Json(new { success = false });
            }


        }
		[Authorize(Roles = "Security")]
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
		[Authorize(Roles = "Security")]
		public IActionResult SetAbsent(StudentAttendance studentAttendance, int TrackID)
        {
            // get the schedule of the track for today
            var schedule = scheduleRepo.GetScheduleForToday(TrackID, DateOnly.FromDateTime(DateTime.Now));
            studentAttendance.ScheduleID = schedule?.Id;
            if (ModelState.IsValid)
            {
                studentAttendance.AttendanceStatus = Enums.AttendanceStatus.Absent;
                studentAttendance.IsMarked = false;
                studentAttendanceRepo.Add(studentAttendance);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }


        }
		[Authorize(Roles = "Security")]
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
		[Authorize(Roles = "Security")]
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
		[Authorize(Roles = "Security")]
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
		[Authorize(Roles = "Admin,StudentAffairs")]
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
		[Authorize(Roles = "Security")]
		public IActionResult MarkStdAbsence(int Pid,int Tid , int Ino)
        {
            var Schedule = scheduleRepo.GetScheduleForToday(Tid, DateOnly.FromDateTime(DateTime.Now));
            var ScheduleID = Schedule?.Id;
            var stdlist = studentRepo.GetForAttendance(Pid, Tid, Ino);

            if (studentAttendanceRepo.MarkAbsence(stdlist, ScheduleID.Value))
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }

        }
		[Authorize(Roles = "Security")]
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
		[Authorize(Roles = "Admin,StudentAffairs")]
		public IActionResult GetStudentsAttendace (int Pid, int Tid, int Ino,DateOnly Date)
        {
            var list = studentRepo.GetForAttendanceExplicit(Pid, Tid, Ino,Date);
            ViewBag.CurentTrackId = Tid;
            return PartialView("_StudentsAttListPartial", list);
        }
		[Authorize(Roles = "Admin,StudentAffairs")]
		public IActionResult GetRangeStudentsAttendace(int Pid, int Tid, int Ino, DateOnly Date,DateOnly EndDate)
        {
            var list = studentRepo.GetForRangeAttendanceExplicit(Pid, Tid, Ino, Date,EndDate);
            ViewBag.CurentTrackId = Tid;
            return PartialView("_StudentsRangeAttListPartial", list);
        }
		[Authorize(Roles = "Admin,StudentAffairs")]
		public IActionResult ShowStaffAttendance()
        {
            return View("StaffAttendance");
        }
		[Authorize(Roles = "Admin,StudentAffairs")]
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
		[Authorize(Roles = "Admin,StudentAffairs")]
		public IActionResult StaffRangeAttendance()
        {
            return View();
        }
		[Authorize(Roles = "Admin,StudentAffairs")]
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
		[Authorize(Roles = "Admin,StudentAffairs")]
		public IActionResult GetRangeStaffAttendance(int TypeNo, DateOnly Date,DateOnly EndDate)
        {
            switch (TypeNo)
            {
                case 1:
                    var instlist = instructorRepo.GetForRangeAttendanceExplicit(Date,EndDate);
                    return PartialView("_InstRangeAttListPartial", instlist);
                case 2:
                    var emplist = employeeRepo.GetForRangeAttendanceExplicit(Date, EndDate);
                    return PartialView("_EmpRangeAttListPartial", emplist);
                default:
                    return BadRequest();
            }
        }

		//public IActionResult CalculateStudentsAttendace()
		//{
		//    int Pid = 1; int Tid = 4; int Ino = 2; DateOnly Date = new DateOnly(2024, 04, 09); DateOnly EndDate = new DateOnly(2024, 04, 09);
		//    var list = studentRepo.GetForUpdateAttendanceDegExplicit(Pid, Tid, Ino, Date, EndDate);

		//    foreach (var student in list)
		//    {
		//        studentAttendanceRepo.CalculateNoOfDeductions(student);
		//    }
		//    foreach (var student in list)
		//    {
		//        for (DateOnly date = Date; date <= EndDate; date = date.AddDays(1)){
		//            //var stdAttendance = student.StudentAttendances.SingleOrDefault(a => a.Date == date && ( a.AttendanceStatus==Enums.AttendanceStatus.Late || a.AttendanceStatus==Enums.AttendanceStatus.Absent));
		//            var stdAttendance = studentAttendanceRepo.GetStdAttendance(student, date);
		//            if (stdAttendance != null)
		//            {
		//                var permission = student.Permissions.SingleOrDefault(p => p.Date == date);
		//                if(permission != null)
		//                {
		//                    if(permission.IsAccepted==true)
		//                    {
		//                        if(student.NoOfDeductions>=0&& student.NoOfDeductions < 3)
		//                        {
		//                            student.AttendanceDegrees -= 5;
		//                            student.NoOfDeductions++;
		//                            stdAttendance.IsMarked = true;
		//                        }
		//                        else if(student.NoOfDeductions>=3&& student.NoOfDeductions < 6)
		//                        {
		//                            student.AttendanceDegrees -= 10;
		//                            student.NoOfDeductions++;
		//                            stdAttendance.IsMarked = true;
		//                        }
		//                        else if(student.NoOfDeductions>=6)
		//                        {
		//                            student.AttendanceDegrees -= 15;
		//                            student.NoOfDeductions++;
		//                            stdAttendance.IsMarked = true;
		//                        }

		//                    }
		//                    else
		//                    {
		//                        student.AttendanceDegrees -= 25;
		//                        student.NoOfDeductions++;
		//                        stdAttendance.IsMarked = true;
		//                    }
		//                }
		//                else
		//                {
		//                    student.AttendanceDegrees -= 25;
		//                    student.NoOfDeductions++;
		//                    stdAttendance.IsMarked = true;
		//                }
		//            }

		//        }
		//    }
		//    if (studentRepo.UpdateAttendanceDegrees(list))
		//    {
		//        return Json(new { success = true });
		//    }
		//    else
		//    {
		//        return Json(new { success = false });
		//    }
		//}
		[Authorize(Roles = "Admin,StudentAffairs")]
		public IActionResult CalculateStudentsAttendace(int Pid,int Tid,int Ino,DateOnly Date , DateOnly EndDate)
        {
            var list = studentRepo.GetForUpdateAttendanceDegExplicit(Pid, Tid, Ino, Date, EndDate);

            foreach (var student in list)
            {
                studentAttendanceRepo.CalculateNoOfDeductions(student);
            }
            foreach (var student in list)
            {
                for (DateOnly date = Date; date <= EndDate; date = date.AddDays(1))
                {
                    var stdAttendance = student.Attendances.SingleOrDefault(a => a.Date == date &&  (a as StudentAttendance).IsMarked==false) as StudentAttendance;
                    if (stdAttendance != null )
                    {
                        if (stdAttendance.AttendanceStatus == Enums.AttendanceStatus.Late || stdAttendance.AttendanceStatus == Enums.AttendanceStatus.Absent)
                        {
                            var permission = student.Permissions.SingleOrDefault(p => p.Date == date);
                            if (permission != null)
                            {
                                if (permission.IsAccepted == true)
                                {
                                    if (student.NoOfDeductions >= 0 && student.NoOfDeductions < 3)
                                    {
                                        student.AttendanceDegrees -= 5;
                                        student.NoOfDeductions++;

                                    }
                                    else if (student.NoOfDeductions >= 3 && student.NoOfDeductions < 6)
                                    {
                                        student.AttendanceDegrees -= 10;
                                        student.NoOfDeductions++;

                                    }
                                    else if (student.NoOfDeductions >= 6)
                                    {
                                        student.AttendanceDegrees -= 15;
                                        student.NoOfDeductions++;

                                    }

                                }
                                else
                                {
                                    student.AttendanceDegrees -= 25;
                                    student.NoOfDeductions++;

                                }
                            }
                            else
                            {
                                student.AttendanceDegrees -= 25;
                                student.NoOfDeductions++;

                            }
                        }
                      
                      stdAttendance.IsMarked = true;
                    }
                    

                }
            }
            if (studentRepo.UpdateAttendanceDegrees(list))
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}
