using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Tracking_System.Repositories
{
	public class AdminRepo:IAdminRepo
	{
		private readonly ITISysContext context;

		public AdminRepo(ITISysContext _context)
		{
			this.context = _context;
		}

		public User AdminData(int? id)
		{
			User user = context.User.FirstOrDefault(a => a.Id == id.Value);
			return user;
		}
		public async Task EditAdminData(Admin admin)
		{
			var user = await context.admin.FirstOrDefaultAsync(a => a.Id == admin.Id);
			if (user != null)
			{
				user.Name = admin.Name;
				user.Email = admin.Email;
				user.PhoneNumber = admin.PhoneNumber;
				user.Age = admin.Age;
				user.Password = admin.Password;
			    user.UserImage=admin.UserImage;
				await context.SaveChangesAsync();
			}
		}
		public void uploadImg(string ImgName, int id)
		{
			var res = context.admin.FirstOrDefault(a => a.Id == id);
			res.UserImage = ImgName;
			context.SaveChanges();
		}
		public bool CheckEmailUniqueness(User admin)
		{
			return !context.User.Any(a => a.Email == admin.Email && a.Id != admin.Id);
		}


	}
}
