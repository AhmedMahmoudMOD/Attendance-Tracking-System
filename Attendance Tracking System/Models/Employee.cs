using Attendance_Tracking_System.Enums;
using System.ComponentModel.DataAnnotations;

namespace Attendance_Tracking_System.Models
{
    public class Employee : User
    {
        [Range(2001, int.MaxValue, ErrorMessage = "Salary must be greater than 2000")]
        public int Salary {  get; set; }

        public EmployeeType Type { get; set; }
    }
}
