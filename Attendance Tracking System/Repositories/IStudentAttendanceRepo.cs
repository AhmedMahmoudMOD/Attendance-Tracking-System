using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IStudentAttendanceRepo
    {
        public List<StudentAttendance> GetAllAttendance(int studentId);
        StudentAttendance GetAttendanceById(int id);

        bool Add(StudentAttendance studentAttendance);

        StudentAttendance GetStudentAttendance(int studentId, DateOnly date);

        bool MarkAbsence(List<Student> students,int ScheduleID);

        bool Update(StudentAttendance studentAttendance);
    }
}
