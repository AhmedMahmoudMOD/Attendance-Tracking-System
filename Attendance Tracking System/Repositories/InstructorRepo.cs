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

        public List<Instructor> GetForAttendance()
        {
            // get only the instructors who does not have attendance for today
            var today = DateOnly.FromDateTime(DateTime.Now);
            var list = db.Instructor
                .Where(s => !db.Attendance.Any(a => a.UserID == s.Id && a.Date == today))
                .ToList();

            return list;
        }
    }
}
