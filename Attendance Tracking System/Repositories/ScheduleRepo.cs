using Attendance_Tracking_System.Data;

namespace Attendance_Tracking_System.Repositories
{
    public class ScheduleRepo : IScheduleRepo
    {
        private readonly ITISysContext db;

        public ScheduleRepo(ITISysContext db)
        {
            this.db = db;
        }
    }
}
