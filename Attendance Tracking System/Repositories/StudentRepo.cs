using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Tracking_System.Repositories
{
    public class StudentRepo : IStudentRepo
    {
        private readonly ITISysContext db;

        public StudentRepo(ITISysContext db)
        {
            this.db = db;
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
            var students = db.Student.ToList();
            return students;
        }   
        public Student GetById(int id)
        {
            var student = db.Student.SingleOrDefault(x => x.Id == id);
            return student;
        }

        public void Update(Student student)
        {
            db.Update(student);
            db.SaveChanges();
        }
    }
}
