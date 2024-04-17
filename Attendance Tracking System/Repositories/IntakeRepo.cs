using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using System.Data;

namespace Attendance_Tracking_System.Repositories
{
    public class IntakeRepo : IIntakeRepo
    {
        private readonly ITISysContext db;

        public IntakeRepo(ITISysContext db)
        {
            this.db = db;
        }

        public Intake GetCurrentIntake(int Pid)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var target = db.Intake.SingleOrDefault(i => i.StartDate < today && i.EndDate > today && i.ProgramID==Pid && i.IsDeleted==false);
            return target;
        }
        public List<Intake> GetAll()
        {
            return db.Intake.ToList();
        }
    }
}
