using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IStudentRepo
    {
        public List<Student> GetAll();
        public Student GetById(int id);


    }
}
