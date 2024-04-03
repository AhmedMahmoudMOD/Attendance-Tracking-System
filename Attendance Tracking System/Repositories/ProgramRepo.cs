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
            var list = db.Program.Where(p=>p.IsDeleted==false).Include(p=>p.Tracks).ToList();
            return list;
        }

        public ITIProgram GetByID(int id)
        {
            var target = db.Program.Include(p => p.Tracks).SingleOrDefault(target=>target.Id == id);
            return target;
        }
        public void Add(ITIProgram program)
        {
            db.Program.Add(program);
            db.SaveChanges();
        }

        public void Update(ITIProgram program)
        {
            db.Program.Update(program);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var program = GetByID(id);
            //db.Program.Remove(program);// hard delete
            program.IsDeleted= true;// soft delete
            db.SaveChanges();
        }
    }
}
