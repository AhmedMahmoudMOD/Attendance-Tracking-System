using Attendance_Tracking_System.Data;

namespace Attendance_Tracking_System.Repositories
{
    public class AttendacneRepo : IAttendanceRepo
    {
        private readonly ITISysContext db;

        public AttendacneRepo(ITISysContext db)
        {
            this.db = db;
        }
    }
}
