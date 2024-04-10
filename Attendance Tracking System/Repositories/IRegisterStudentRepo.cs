using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
	public interface IRegisterStudentRepo
	{
		public bool CheckEmailUniqueness(string email);
		public void uploadImg(string ImgName, int id);
		public void RegisterStudent(Student student, string userImageFileName);
		public List<Track> GetAllTracks();
		public void AddStudentInUserRole(int stdId);
		public void AssignRoleToUser(int userId, int roleId);
		public List<ITIProgram> GetAllPrograms();
		public List<Track> GetTrackById(int id);
	}
}
