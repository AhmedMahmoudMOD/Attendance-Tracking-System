using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IStudentRepo
    {
        List<Student> GetForAttendance(int Pid, int Tid, int Ino);

        List<Student> GetForAttendanceExplicit(int Pid, int Tid, int Ino, DateOnly date);

       List<object> GetForAttendanceReport(int Pid, int Tid, int Ino, DateOnly date);

        void AddRange(List<Student> students);
        List<Student> GetAll();
         Student GetById(int id);
        void Update(Student student);   


    }
}
