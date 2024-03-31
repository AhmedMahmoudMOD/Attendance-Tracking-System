using Attendance_Tracking_System.Data;

namespace Attendance_Tracking_System.Repositories
{
    public class StudentRepo : IStudentRepo
    {
        private readonly ITISysContext db;

        public StudentRepo(ITISysContext db)
        {
            this.db = db;
        }
    }
}
