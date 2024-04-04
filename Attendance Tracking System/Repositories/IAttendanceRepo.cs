using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IAttendanceRepo
    {
        public List<StudentAttendance> getAllAttendance(int studentId);
    }
}
