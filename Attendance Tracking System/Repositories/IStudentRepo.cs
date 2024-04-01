using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IStudentRepo
    {
        List<Student> GetForAttendance(int Pid, int Tid, int Ino);

        void AddRange(List<Student> students);
    }
}
