using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
namespace Attendance_Tracking_System.Repositories
{
    public class StudentRepo : IStudentRepo
    {
        private readonly ITISysContext db;

        public StudentRepo(ITISysContext db)
        {
            this.db = db;
        }
        public List<Student> GetAll()
        {
            return db.Student.ToList();
        }   
        public Student GetById(int id)
        {
            var student = db.Student.SingleOrDefault(x => x.Id == id);
            return student;
        }
    }
}
