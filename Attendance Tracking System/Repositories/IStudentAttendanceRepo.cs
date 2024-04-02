using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IStudentAttendanceRepo
    {
         StudentAttendance getAllAttendance(int id);
        StudentAttendance getAttendanceById(int id);
    }
}
