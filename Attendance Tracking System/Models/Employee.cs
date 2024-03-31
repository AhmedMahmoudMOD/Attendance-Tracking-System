using Attendance_Tracking_System.Enums;

namespace Attendance_Tracking_System.Models
{
    public class Employee : User
    {
        public int Salary {  get; set; }

        public EmployeeType Type { get; set; }
    }
}
