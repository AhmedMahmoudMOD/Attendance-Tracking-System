using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IStudentAttendanceRepo
    {
        bool Add(StudentAttendance studentAttendance);

        StudentAttendance GetStudentAttendance(int studentId, DateOnly date);

        bool Update(StudentAttendance studentAttendance);
    }
}
