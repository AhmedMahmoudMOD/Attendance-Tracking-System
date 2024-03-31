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

    }
}
