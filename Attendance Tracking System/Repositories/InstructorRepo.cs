using Attendance_Tracking_System.Data;

namespace Attendance_Tracking_System.Repositories
{
    public class InstructorRepo : IInstructorRepo
    {
        private readonly ITISysContext db;

        InstructorRepo(ITISysContext db)
        {
            this.db = db;
        }
    }
}
