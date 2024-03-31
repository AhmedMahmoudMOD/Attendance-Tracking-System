using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IProgramRepo
    {
        List<ITIProgram> GetAll();

        ITIProgram GetByID(int id);
    }
}
