using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IStudentRepo
    {
        Student getStudentById(int id);
        public Task<Student> AddStudent(Student student);
        public Task EditStudent(Student student);

    }
}
