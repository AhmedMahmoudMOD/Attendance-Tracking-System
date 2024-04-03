using Attendance_Tracking_System.Data;
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
        public List<Employee> GetAllStudentAffairs()
        {
            var studentAffairs = db.Employee.Where(e=>e.Type == Enums.EmployeeType.StudentAffairs).ToList();
            return studentAffairs;   
        }

        public Employee GetByID(int id)
        {
            return db.Employee.FirstOrDefault(e => e.Id == id);
        }

        public void Update(Employee employee)
        {
            db.Employee.Update(employee);
            db.SaveChanges();
        }



    }
}
