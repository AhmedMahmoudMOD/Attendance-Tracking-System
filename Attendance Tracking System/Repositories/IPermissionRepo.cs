using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IPermissionRepo
    {
        Permission addPermission(Permission permission);
        void removePermission(int id);
        List<Permission> getAllPermission(int id);
    }
}
