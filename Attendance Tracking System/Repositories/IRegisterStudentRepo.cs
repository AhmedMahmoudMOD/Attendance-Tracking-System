using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
	public interface IRegisterStudentRepo
	{
		public bool checkEmailUniqueness(User user);
		public void uploadImg(string ImgName, int id);
		public void RegisterStudent(Student student, string userImageFileName);
		public List<Track> GetAllTracks();
		public void AddStudentInUserRole(int stdId);
		public void AssignRoleToUser(int userId, int roleId);
	}
}
