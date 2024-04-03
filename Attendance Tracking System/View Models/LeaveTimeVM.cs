namespace Attendance_Tracking_System.View_Models
{
    public class LeaveTimeVM
    {
        public int UserId { get; set; } 

        public TimeOnly? LeaveTime { get; set; }

        public DateOnly Date { get; set; }
    }
}
