using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public class ScheduleRepo : IScheduleRepo
    {
        private readonly ITISysContext db;

        public ScheduleRepo(ITISysContext db)
        {
            this.db = db;
        }

        public Schedule GetScheduleForToday(int TrackId,DateOnly date)
        {
            var schedule = db.Schedule.SingleOrDefault(s => s.TrackID == TrackId && s.Date == date);
            return schedule;
        }
    }
}
