using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public class PermissionRepo : IPermissionRepo
    {
        private readonly ITISysContext db;

        public PermissionRepo(ITISysContext db)
        {
            this.db = db;
        }
        public Permission getPermissionByID(int id)
        {
            return db.Permission.SingleOrDefault(a => a.PermissionID == id);
        }

        public void UpdatePermissionAcceptance(Permission permission,bool Response)
        {
            permission.IsAccepted = Response;
            db.SaveChanges();
        }
    }
}
