using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
	public interface IAdminRepo
	{
		public bool CheckEmailUniqueness(User admin);
		public  Task EditAdminData(Admin admin);
		public void uploadImg(string ImgName, int id);
		public User AdminData(int? id);
	}
}
