using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IEmployeeRepo
    {
        Employee GetByID(int id);

        bool Update(Employee employee);

        List<Employee> GetForAttendance();

        List<Employee> GetForAttendanceExplicit(DateOnly date);

        List<Employee> GetForRangeAttendanceExplicit(DateOnly date, DateOnly endDate);

        List<object> GetForAttendanceReport(DateOnly date);

        List<Employee> GetAllStudentAffairs();
    }
}
