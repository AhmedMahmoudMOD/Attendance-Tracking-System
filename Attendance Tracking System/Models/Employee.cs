using Attendance_Tracking_System.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Attendance_Tracking_System.Models
{
    public class Employee : User
    {
        [Required]
        [Range(2001, int.MaxValue, ErrorMessage = "Salary must be greater than 2000")]
        public int Salary {  get; set; }
		[Required]
        [JsonIgnore]
		public EmployeeType Type { get; set; }
    }
}
