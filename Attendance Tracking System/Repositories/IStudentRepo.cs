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

        List<Student> GetForRangeAttendanceExplicit(int Pid, int Tid, int Ino, DateOnly date, DateOnly EndDate);

       List<Student> GetForUpdateAttendanceDegExplicit(int Pid, int Tid, int Ino, DateOnly date, DateOnly EndDate);
       List<object> GetForAttendanceReport(int Pid, int Tid, int Ino, DateOnly date);

        List<object> GetForRangeAttendanceReport(int Pid, int Tid, int Ino, DateOnly date, DateOnly EndDate);

        void AddRange(List<Student> students);
        List<Student> GetAll();
         Student GetById(int id);
        void Update(Student student);

        bool UpdateAttendanceDegrees(List<Student> students);


    }
}
