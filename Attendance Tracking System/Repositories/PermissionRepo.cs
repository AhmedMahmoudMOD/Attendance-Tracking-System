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
        public Permission addPermission(Permission permission)
        {
            db.Permission.Add(permission);
            db.SaveChanges();
            return permission;
        }
        public void removePermission(int id)
        {
            var permission = db.Permission.FirstOrDefault(p => p.PermissionID == id);
            db.Permission.Remove(permission);
            db.SaveChanges();
        }
        public List<Permission> getAllPermission(int id)
        {
            return db.Permission.Where(p => p.StudentID == id).ToList();
        }
    }
}
