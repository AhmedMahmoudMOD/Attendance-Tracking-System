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
		public List<StudentAttendance> getAllAttendance(int studentId)
		{
			var att = db.StudentAttendance.Where(s => s.UserID == studentId).ToList();
            return att;
		}
	}
}
