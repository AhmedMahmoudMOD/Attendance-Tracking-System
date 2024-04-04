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
		public List<StudentAttendance> GetAllAttendance(int studentId)
		{
			var att =db.StudentAttendance.Where(s => s.UserID == studentId).ToList();
            return att;
		}


		public StudentAttendance GetAttendanceById(int id)
        {
            return db.StudentAttendance.FirstOrDefault(s => s.AttID == id);
        }
    }
}
