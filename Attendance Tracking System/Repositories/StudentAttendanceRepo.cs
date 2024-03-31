using Attendance_Tracking_System.Data;

namespace Attendance_Tracking_System.Repositories
{
    public class StudentAttendanceRepo : IStudentAttendanceRepo
    {
        private readonly ITISysContext db;

        public StudentAttendanceRepo(ITISysContext db)
        {
            this.db = db;
        }
    }
}
