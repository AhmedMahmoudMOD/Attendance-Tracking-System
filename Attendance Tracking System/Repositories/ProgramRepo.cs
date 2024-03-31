using Attendance_Tracking_System.Data;

namespace Attendance_Tracking_System.Repositories
{
    public class ProgramRepo : IProgramRepo
    {
        private readonly ITISysContext db;

        ProgramRepo(ITISysContext db)
        {
            this.db = db;
        }
    }
}
