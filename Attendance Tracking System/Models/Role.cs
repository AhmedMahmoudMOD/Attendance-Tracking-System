using System.Text.Json.Serialization;

namespace Attendance_Tracking_System.Models
{
	public class Role
	{
		public int Id { get; set; }
		public string RoleType { get; set; }
        [JsonIgnore]
        public virtual ICollection<User> user { get; set; } = new HashSet<User>();
	}
}
