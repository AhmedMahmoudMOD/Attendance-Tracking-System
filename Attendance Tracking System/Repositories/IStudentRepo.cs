using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IStudentRepo
    {
        Student getStudentById(int id);
        void addStudent(Student student);
    }
}
