using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IInstructorRepo
    {
        List<Instructor> GetForAttendance();

        List<Instructor> GetForAttendanceExplicit(DateOnly date);

        List<object> GetForAttendanceReport(DateOnly date);
    }
}
