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
            OldInstructor.PhoneNumber=Ins.PhoneNumber;
            db.SaveChanges();
        }

        public List<Instructor> GetAllInstructors()
        {
            return db.Instructor.ToList();
        }

        public Instructor GetInstructorById(int id)
        {
            return db.Instructor.Include(a=>a.Tracks).SingleOrDefault(a => a.Id == id);
        }

        public void UpdateInstructorImage(string InsImgName, int Id)
        {
            var instructor = db.Instructor.FirstOrDefault(s => s.Id == Id);
            instructor.UserImage = InsImgName;
            db.SaveChanges();
        }

        

        HashSet<Schedule> IInstructorRepo.getSheduleForTrack(int id)
        {
            var Track = db.Track.Include(a => a.Schedules).SingleOrDefault(a => a.SuperID == id);
            return Track.Schedules.ToHashSet();
        }
    }
}
