using Attendance_Tracking_System.Data;
using Microsoft.EntityFrameworkCore;
using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.Enums;

namespace Attendance_Tracking_System.Repositories
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly ITISysContext db;

        public EmployeeRepo(ITISysContext db)
        {
            this.db = db;
        }

        public Employee GetByID(int id)
        {
            var target = db.Employee.SingleOrDefault(e=>e.Id == id);
            return target;
        }

        public bool Update(Employee employee) {
            try
            {
                
                db.Employee.Update(employee);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
       

        public bool Add(Employee employee)
        {
            try
            {
                db.Employee.Add(employee);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var target = db.Employee.SingleOrDefault(e => e.Id == id);
                target.IsDeleted = true;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Employee> GetForAttendance()
        {
            // get only the Employees who does not have attendance for today
            var today = DateOnly.FromDateTime(DateTime.Now);
            var list = db.Employee
                .Where(s => !db.Attendance.Any(a => a.UserID == s.Id && a.Date == today))
                .ToList();

            return list;
        }

        public List<Employee> GetForAttendanceExplicit(DateOnly date)
        {
            
           
            var list = db.Employee.Include(e=>e.Attendances)
                .Where(e => e.Attendances.Any(a => a.Date == date))
                .ToList();

            return list;
        }

        public List<Employee> GetForRangeAttendanceExplicit(DateOnly date,DateOnly endDate)
        {


            var list = db.Employee.Include(e => e.Attendances)
                .Where(e => e.Attendances.Any(a => a.Date >= date && a.Date<=endDate))
                .ToList();

            return list;
        }
        public List<object> GetForAttendanceReport(DateOnly date)
        {
            var list = db.Employee
                .SelectMany(e => e.Attendances.Where(a => a.Date == date)
                                               .Select(a => new
                                               {
                                                   EmpId = e.Id,
                                                   EmpName = e.Name,
                                                   AttendanceStatus = a.AttendanceStatus,
                                                   ArrivalTime = a.ArrivalTime,
                                                   LeaveTime = a.LeaveTime
                                               }))
                .ToList();

            return list.Cast<object>().ToList();
        }

        public List<object> GetForRangeAttendanceReport(DateOnly date,DateOnly EndDate)
        {
            var list = db.Employee
                .SelectMany(e => e.Attendances.Where(a => a.Date >= date && a.Date <= EndDate)
                                               .Select(a => new
                                               {
                                                   EmpId = e.Id,
                                                   EmpName = e.Name,
                                                   Date = a.Date,
                                                   AttendanceStatus = a.AttendanceStatus,
                                                   ArrivalTime = a.ArrivalTime,
                                                   LeaveTime = a.LeaveTime
                                               }))
                .ToList();

            return list.Cast<object>().ToList();
        }

        public List<Employee> GetAllStudentAffairs()
        {
            var studentAffairs = db.Employee.Where(e => e.Type == Enums.EmployeeType.StudentAffairs).ToList();
            return studentAffairs;
        }



    }
}
