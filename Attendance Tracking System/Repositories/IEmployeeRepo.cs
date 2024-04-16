using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IEmployeeRepo
    {
        Employee GetByID(int id);

        bool Update(Employee employee);

        bool Add(Employee employee);

        bool Delete(int id);

        List<Employee> GetForAttendance();

        List<Employee> GetForAttendanceExplicit(DateOnly date);

        List<Employee> GetForRangeAttendanceExplicit(DateOnly date, DateOnly endDate);

        List<object> GetForAttendanceReport(DateOnly date);

        List<object> GetForRangeAttendanceReport(DateOnly date, DateOnly EndDate);

        List<Employee> GetAllStudentAffairs();

        List<Attendance> GetAttendancesByEmpID(int EmpID);
    }
		User GetUserById(int id);
	}
}
