using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Tracking_System.Repositories
{
    public class StudentRepo : IStudentRepo
    {
        private readonly ITISysContext db;
        private readonly IUploadFile UploadFileRepo;

        public StudentRepo(ITISysContext _db , IUploadFile uploadFile)
        {
            this.db = _db;
            this.UploadFileRepo = uploadFile;
        }
        public Student getStudentById(int id)
        {
            return db.Student
                .Include(T => T.Track)
                .Include(P => P.Program)
                .Include(I => I.Intake)
                .FirstOrDefault(s => s.Id == id);
        }
        public async Task<Student> AddStudent(Student student)
        {
            if (student.Image != null && student.Image.Length > 0)
            {
                var fileName = Path.GetFileName(student.Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await student.Image.CopyToAsync(stream);
                }

                student.UserImage = fileName;
                db.Student.Add(student);
                await db.SaveChangesAsync();
            }
            return student;
        }


        public async Task EditStudent(Student student)
        {
            var existingStudent = db.Student.FirstOrDefault(s => s.Id == student.Id);
            existingStudent.Name = student.Name;
            existingStudent.Email = student.Email;
            existingStudent.Password = student.Password;
        if (student.Image != null && student.Image.Length > 0)
        {
            existingStudent.UserImage = await UploadFileRepo.UploadFile(student.Image); 
        }
           db.SaveChanges();
        }

       




    }
}
