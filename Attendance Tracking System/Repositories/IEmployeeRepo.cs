using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IEmployeeRepo
    {
        Employee GetByID(int id);
    }
}
