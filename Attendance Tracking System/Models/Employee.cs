using Attendance_Tracking_System.Enums;
using System.ComponentModel.DataAnnotations;

namespace Attendance_Tracking_System.Models
{
    public class Employee : User
    {
        [Required]
        [Range(3000,20000)]
        public int Salary {  get; set; }
		[Required]
		public EmployeeType Type { get; set; }
    }
}
