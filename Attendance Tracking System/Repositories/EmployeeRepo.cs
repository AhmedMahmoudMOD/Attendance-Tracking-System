using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;

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

        public List<Employee> GetForAttendance()
        {
            // get only the Employees who does not have attendance for today
            var today = DateOnly.FromDateTime(DateTime.Now);
            var list = db.Employee
                .Where(s => !db.Attendance.Any(a => a.UserID == s.Id && a.Date == today))
                .ToList();

            return list;
        }

    }
}
