using Attendance_Tracking_System.Data;

namespace Attendance_Tracking_System.Repositories
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly ITISysContext db;

        EmployeeRepo(ITISysContext db)
        {
            this.db = db;
        }
    }
}
