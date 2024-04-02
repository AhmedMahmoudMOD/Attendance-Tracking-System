using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public class StudentAttendanceRepo : IStudentAttendanceRepo
    {
        private readonly ITISysContext db;

        public StudentAttendanceRepo(ITISysContext db)
        {
            this.db = db;
        }
        public StudentAttendance getAllAttendance(int id)
        { 
           return db.StudentAttendance.FirstOrDefault(s => s.UserID == id);
        }
        public StudentAttendance getAttendanceById(int id)
        {
            return db.StudentAttendance.FirstOrDefault(s => s.AttID == id);
        }
    }
}
