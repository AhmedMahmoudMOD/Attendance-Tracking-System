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
    }
}
