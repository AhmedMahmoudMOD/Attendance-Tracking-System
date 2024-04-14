using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IProgramRepo
    {
        List<ITIProgram> GetAll();
        void Add(ITIProgram program);
        ITIProgram GetByID(int id);
        void Update(ITIProgram program);
        void Delete(int id);

        public List<ITIProgram> GetAllPrograms();

    }
}
