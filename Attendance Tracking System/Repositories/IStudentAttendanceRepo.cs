using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IStudentAttendanceRepo
    {
        public List<StudentAttendance> GetAllAttendance(int studentId);
        StudentAttendance GetAttendanceById(int id);
    }
}
