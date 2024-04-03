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
        public List<Instructor> GetAll()
        {
            return db.Instructor.ToList();
        }
    }
}
