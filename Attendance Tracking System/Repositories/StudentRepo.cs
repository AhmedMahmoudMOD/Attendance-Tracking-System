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
        public Student getStudentById(int id)
        {
            return db.Student.FirstOrDefault(s => s.Id == id);
        }
        public async void addStudent(Student student)
        {
            if (student.Image != null && student.Image.Length > 0)
            {
                var fileName = Path.GetFileName(student.Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await student.Image.CopyToAsync(stream);
                }
                student.UserImage = fileName;
                db.Student.Add(student);
                db.SaveChanges();
            }
        }
    }
}
