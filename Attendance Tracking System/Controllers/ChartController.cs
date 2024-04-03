﻿using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Enums;
using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Repositories;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using Attendance_Tracking_System.Charts;

namespace Attendance_Tracking_System.Controllers
{
    public class ChartController : Controller
    {
        private readonly IStudentRepo studentRepo;
        private readonly IEmployeeRepo employeeRepo;
        private readonly IInstructorRepo instructorRepo;

        public ChartController(IStudentRepo studentRepo,IEmployeeRepo employeeRepo,IInstructorRepo instructorRepo)
        {
            this.studentRepo = studentRepo;
            this.employeeRepo = employeeRepo;
            this.instructorRepo = instructorRepo;
        }
        public IActionResult GetAttPieChart(int Pid,int Tid,int Ino,DateOnly Date)
        {
            List<Student> students = studentRepo.GetForAttendanceExplicit(Pid, Tid, Ino, Date);

            int absentCount = 0;
            int presentCount = 0;
            int lateCount = 0;

            foreach (var student in students)
            {
                foreach (var attendance in student.Attendances)
                {
                    switch (attendance.AttendanceStatus)
                    {
                        case AttendanceStatus.Absent:
                            absentCount++;
                            break;
                        case AttendanceStatus.Present:
                            presentCount++;
                            break;
                        case AttendanceStatus.Late:
                            lateCount++;
                            break;
                        default:
                            break;
                    }
                }
            }

            var chartData = new List<PieChartData>
            {
                new PieChartData { xValue = "Absent", yValue = absentCount , fill= "#ab263c" },
                new PieChartData { xValue = "Present", yValue = presentCount , fill="#008000" },
                new PieChartData { xValue = "Late", yValue = lateCount  , fill = "#1c1170"}
                };

            ViewBag.ChartData = chartData;

            return View("_AttPieChart");

        }

        public IActionResult GetStaffAttPieChart(int TypeNo, DateOnly Date)
        {
            IEnumerable<User> Staff = null;
            switch(TypeNo)
            {
                case 1:
                    Staff = instructorRepo.GetForAttendanceExplicit(Date);
                    break;
                case 2:
                    Staff = employeeRepo.GetForAttendanceExplicit(Date);
                    break;
                default:
                    break;
            }
            int absentCount = 0;
            int presentCount = 0;
            int lateCount = 0;

            foreach (var member in Staff)
            {
                foreach (var attendance in member.Attendances)
                {
                    switch (attendance.AttendanceStatus)
                    {
                        case AttendanceStatus.Absent:
                            absentCount++;
                            break;
                        case AttendanceStatus.Present:
                            presentCount++;
                            break;
                        case AttendanceStatus.Late:
                            lateCount++;
                            break;
                        default:
                            break;
                    }
                }
            }

            var chartData = new List<PieChartData>
            {
                new PieChartData { xValue = "Absent", yValue = absentCount , fill= "#ab263c" },
                new PieChartData { xValue = "Present", yValue = presentCount , fill="#008000" },
                new PieChartData { xValue = "Late", yValue = lateCount  , fill = "#1c1170"}
                };

            ViewBag.ChartData = chartData;

            return View("_StaffAttPieChart");

        }

    }
}