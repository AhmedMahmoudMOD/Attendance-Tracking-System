using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Tracking_System.Repositories
{
    public class StudentRepo : IStudentRepo
    {
        private readonly ITISysContext db;

        public StudentRepo(ITISysContext db)
        {
            this.db = db;
        }

        public List<Student> GetForAttendance(int Pid, int Tid, int Ino)
        {
            var list = db.Student.Where(s=>s.ProgramID==Pid&&s.TrackID==Tid&&s.IntakeNo==Ino).ToList();
            return list;
        }
    }
}
