using Attendance_Tracking_System.Controllers;
using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Tracking_System.Repositories
{
    public class InstructorRepo : IInstructorRepo
    {
        private readonly ITISysContext db;

        public InstructorRepo(ITISysContext db)
        {
            this.db = db;
        }

        public List<Instructor> GetForAttendance()
        {
            // get only the instructors who does not have attendance for today
            var today = DateOnly.FromDateTime(DateTime.Now);
            var list = db.Instructor
                .Where(s => !db.Attendance.Any(a => a.UserID == s.Id && a.Date == today))
                .ToList();

            return list;
        }

        public List<Instructor> GetForAttendanceExplicit(DateOnly date)
        {


            var list = db.Instructor.Include(i => i.Attendances)
                .Where(i=> i.Attendances.Any(a => a.Date == date))
                .ToList();

            return list;
        }
        public List<Instructor> GetForRangeAttendanceExplicit(DateOnly date,DateOnly endDate)
        {


            var list = db.Instructor.Include(i => i.Attendances)
                .Where(i => i.Attendances.Any(a => a.Date >= date && a.Date <=endDate))
                .ToList();

            return list;
        }
        public List<object> GetForAttendanceReport(DateOnly date)
        {
            var list = db.Instructor
                .SelectMany(i => i.Attendances.Where(a => a.Date == date)
                                               .Select(a => new
                                               {
                                                   InstructorId = i.Id,
                                                   InstructorName = i.Name,
                                                   AttendanceStatus = a.AttendanceStatus,
                                                   ArrivalTime = a.ArrivalTime,
                                                   LeaveTime = a.LeaveTime
                                               }))
                .ToList();

            return list.Cast<object>().ToList();
        }

        public void AddNewInstructor(Instructor instructor)
        {
             db.Instructor.Add(instructor);
             db.SaveChanges();
        }

        public void DeleteInstructor(int InsID)
        {
            throw new NotImplementedException();
        }

        public void EditInstructor(Instructor Ins)
        {
            var OldInstructor= db.Instructor.SingleOrDefault(a=>a.Id==Ins.Id);
            OldInstructor.Name=Ins.Name;
            OldInstructor.Age = Ins.Age;
            OldInstructor.Email = Ins.Email;
            OldInstructor.Salary=Ins.Salary;
            db.SaveChanges();
        }

        public ICollection<Instructor> GetAllInstructors()
        {
            return db.Instructor.ToList();
        }

        public Instructor GetInstructorById(int id)
        {
            return db.Instructor.SingleOrDefault(a => a.Id == id);
        }

        public void UpdateInstructorImage(string InsImgName, int Id)
        {
            var std = db.Instructor.FirstOrDefault(s => s.Id == Id);
            std.UserImage = InsImgName;
            db.SaveChanges();
        }
        public List<Instructor> GetAll()
        {
            return db.Instructor.ToList();
        }
    }
}
