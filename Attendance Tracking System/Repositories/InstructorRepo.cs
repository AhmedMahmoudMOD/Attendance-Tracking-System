using Attendance_Tracking_System.Controllers;
using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;

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
    }
}
