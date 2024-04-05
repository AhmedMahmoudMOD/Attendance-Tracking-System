using System.ComponentModel.DataAnnotations;

namespace Attendance_Tracking_System
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
		public string email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string password { get; set; }

	}
}
