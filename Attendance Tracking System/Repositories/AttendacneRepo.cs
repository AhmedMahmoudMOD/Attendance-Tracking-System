using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public class AttendacneRepo : IAttendanceRepo
    {
        private readonly ITISysContext db;

        public AttendacneRepo(ITISysContext db)
        {
            this.db = db;
        }

        public bool Add(Attendance attendance)
        {
            try
            {
                db.Attendance.Add(attendance);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

       
        public Attendance GetAttendance(int id, DateOnly date)
        {
            var attendance = db.Attendance.SingleOrDefault(s => s.UserID == id && s.Date == date);
            return attendance;
        }

        public bool Update(Attendance attendance)
        {
            try
            {
                db.Attendance.Update(attendance);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
