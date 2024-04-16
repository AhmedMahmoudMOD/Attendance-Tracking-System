using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IAttendanceRepo
    {
        public List<StudentAttendance> getAllAttendance(int studentId);
        bool Add(Attendance attendance);

        bool MarkEmpAbsence(List<Employee> staff);

        bool MarkInstAbsence(List<Instructor> staff);

        Attendance GetAttendance(int studentId, DateOnly date);
        List<Attendance> GetAttendanceRecords(int id ,DateOnly startDate,DateOnly endDate );

        bool Update(Attendance attendance);
    }
}
