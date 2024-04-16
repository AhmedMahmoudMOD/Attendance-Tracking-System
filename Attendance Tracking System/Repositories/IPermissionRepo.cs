using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
namespace Attendance_Tracking_System.Repositories
{
    public interface IPermissionRepo
    {
        Permission getPermissionByID(int id);
        void UpdatePermissionAcceptance(Permission permission, bool Response);
        Permission addPermission(Permission permission);
        void removePermission(int id);
        List<Permission> getAllPermission(int id);
    }
}
