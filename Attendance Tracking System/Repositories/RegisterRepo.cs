using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Enums;
using Attendance_Tracking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Tracking_System.Repositories
{
    public class RegisterRepo:IRegisterStudentRepo
    {
		private readonly ITISysContext context;

		public RegisterRepo(ITISysContext _context)
		{
			this.context = _context;
		}

		public bool CheckEmailUniqueness(User user)
		{
			return context.User.Any(a => a.Email != user.Email);
		}

		public void uploadImg(string ImgName, int id)
        {
            var res = context.Student.FirstOrDefault(a => a.Id == id);
            res.UserImage = ImgName;
            context.SaveChanges();
        }
        public void RegisterStudent(Student student, string userImageFileName)
        {
            if (student != null)
            {
                student.UserImage = userImageFileName;
                context.Student.Add(student);
				var tId = student.TrackID;
				var track = context.Track.FirstOrDefault(a => a.Id == tId);
				var progId = track?.ProgramID;
                student.ProgramID = progId;
				context.SaveChanges();
            }
        }
        public List<Track> GetAllTracks()
        {
            return context.Track.ToList();
        }
        public void AddStudentInUserRole(int stdId)
        {
            var studentRole = context.roles.FirstOrDefault(a => a.RoleType == RoleEnum.student.ToString());
            Console.WriteLine(studentRole.Id);
        }
        public void AssignRoleToUser(int userId, int roleId)
        {
            var user = context.User.FirstOrDefault(u => u.Id == userId);
            var role = context.roles.FirstOrDefault(r => r.Id == roleId);

            if (user != null && role != null)
            {
                user.role.Add(role);
                context.SaveChanges();
            }
        }
		
		public List<ITIProgram> GetAllPrograms()
        {
            return context.Program.ToList();
        }
		public List<Track> GetTrackById(int id)
        {
            return context.Track.Where(a=>a.ProgramID==id).ToList();
        }

    }
}
