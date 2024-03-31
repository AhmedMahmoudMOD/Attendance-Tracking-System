using Attendance_Tracking_System.Data;

namespace Attendance_Tracking_System.Repositories
{
    public class PermissionRepo : IPermissionRepo
    {
        private readonly ITISysContext db;

        public PermissionRepo(ITISysContext db)
        {
            this.db = db;
        }
    }
}
