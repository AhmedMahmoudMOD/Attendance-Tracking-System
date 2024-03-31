using Attendance_Tracking_System.Data;

namespace Attendance_Tracking_System.Repositories
{
    public class StudentRepo : IStudentRepo
    {
        private readonly ITISysContext db;

        StudentRepo(ITISysContext db)
        {
            this.db = db;
        }
    }
}
