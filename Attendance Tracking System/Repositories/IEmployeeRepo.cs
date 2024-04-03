using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IEmployeeRepo
    {
        List<Employee> GetAllStudentAffairs();
        Employee GetByID(int id);
        void Update(Employee employee);
    }
}
