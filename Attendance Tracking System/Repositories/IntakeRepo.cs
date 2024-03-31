using Attendance_Tracking_System.Data;

namespace Attendance_Tracking_System.Repositories
{
    public class IntakeRepo : IIntakeRepo
    {
        private readonly ITISysContext db;

        IntakeRepo(ITISysContext db)
        {
            this.db = db;
        }
    }
}
