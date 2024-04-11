using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Attendance_Tracking_System.View_Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        public Student GetStudentById(int? id)
        {
            return db.Student
                .Include(T => T.Track)
                .Include(P => P.Program)
                .Include(I => I.Intake)
                .FirstOrDefault(s => s.Id == id);
        }
        //public async Task<Student> AddStudent(Student student)
        //{
        //    if (student.Image != null && student.Image.Length > 0)
        //    {
        //        var fileName = Path.GetFileName(student.Image.FileName);
        //        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await student.Image.CopyToAsync(stream);
        //        }

        //        student.UserImage = fileName;
        //        db.Student.Add(student);
        //        await db.SaveChangesAsync();
        //    }
        //    return student;
        //}


        public async Task EditStudent(EditStudentViewModel student)
        {
            var  existingStudent = await db.Student.FirstOrDefaultAsync(s => s.Id == student.Id);
            existingStudent.Id = student.Id;
            existingStudent.Name = student.Name;
            existingStudent.Email = student.Email;
            existingStudent.Password = student.Password;
           
        if (student.Image != null && student.Image.Length > 0)
        {
                if (existingStudent.UserImage != null)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", existingStudent.UserImage);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                existingStudent.UserImage = await UploadFileRepo.UploadFile(student.Image,existingStudent.Id,existingStudent.Name);
            }
            await  db.SaveChangesAsync();
        }

        public List<Student> GetForAttendance(int Pid, int Tid, int Ino)
        {
            // get only the students who does not have attendance for today
            var today = DateOnly.FromDateTime(DateTime.Now);
            var list = db.Student
                .Where(s => !db.Attendance.Any(a => a.UserID == s.Id && a.Date == today))
                .Where(s => s.ProgramID == Pid && s.TrackID == Tid && s.IntakeNo == Ino)
                .ToList();

            return list;
        }
        public List<Student> GetForAttendanceExplicit(int Pid, int Tid, int Ino, DateOnly date)
        {
            var list = db.Student
                .Include(s => s.Attendances.Where(a => a.Date == date)) // Apply date constraint to Attendances
                .Where(s => s.ProgramID == Pid && s.TrackID == Tid && s.IntakeNo == Ino && s.Attendances.Any())
                .ToList();

            return list;
        }

        public List<object> GetForAttendanceReport(int Pid, int Tid, int Ino, DateOnly date)
        {
            var list = db.Student
                .Where(s => s.ProgramID == Pid && s.TrackID == Tid && s.IntakeNo == Ino)
                .SelectMany(s => s.Attendances.Where(a => a.Date == date)
                                               .Select(a => new
                                               {
                                                   StudentId = s.Id,
                                                   StudentName = s.Name,
                                                   AttendanceStatus = a.AttendanceStatus,
                                                   ArrivalTime = a.ArrivalTime,
                                                   LeaveTime = a.LeaveTime
                                               }))
                .ToList();

            return list.Cast<object>().ToList();
        }

        public void AddRange(List<Student> students)
        {
            
            db.Student.AddRange(students);
            db.SaveChanges();
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
