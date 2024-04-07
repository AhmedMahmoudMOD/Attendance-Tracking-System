using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.View_Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IStudentRepo
    {
        public Student GetStudentById(int? id);
        //public Task<Student> AddStudent(Student student);
        public Task EditStudent(EditStudentViewModel student);
        List<Student> GetForAttendance(int Pid, int Tid, int Ino);

        List<Student> GetForAttendanceExplicit(int Pid, int Tid, int Ino, DateOnly date);

       List<object> GetForAttendanceReport(int Pid, int Tid, int Ino, DateOnly date);

        void AddRange(List<Student> students);
        List<Student> GetAll();
         Student GetById(int id);


    }
}
