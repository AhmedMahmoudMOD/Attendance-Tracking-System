using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IAttendanceRepo
    {
        bool Add(Attendance attendance);

        bool MarkEmpAbsence(List<Employee> staff);

        bool MarkInstAbsence(List<Instructor> staff);

        Attendance GetAttendance(int studentId, DateOnly date);

        bool Update(Attendance attendance);
    }
}
