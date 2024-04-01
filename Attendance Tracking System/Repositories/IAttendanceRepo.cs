using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IAttendanceRepo
    {
        bool Add(Attendance attendance);

        Attendance GetAttendance(int studentId, DateOnly date);

        bool Update(Attendance attendance);
    }
}
