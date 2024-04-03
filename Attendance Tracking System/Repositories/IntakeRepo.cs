using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public class IntakeRepo : IIntakeRepo
    {
        private readonly ITISysContext db;

        public IntakeRepo(ITISysContext db)
        {
            this.db = db;
        }
        public List<Intake> GetAll()
        {
            return db.Intake.ToList();
        }
    }
}
