using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Tracking_System.Repositories
{
    public class ProgramRepo : IProgramRepo
    {
        private readonly ITISysContext db;

        public ProgramRepo(ITISysContext db)
        {
            this.db = db;
        }

        public List<ITIProgram> GetAll()
        {
            var list = db.Program.Include(p=>p.Tracks).ToList();

            return list;
        }

        public ITIProgram GetByID(int id)
        {
            var target = db.Program.Include(p => p.Tracks).SingleOrDefault(target=>target.Id == id);
            return target;
        }
    }
}
