using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Tracking_System.Models
{
    public class StudentAttendance : Attendance
    {
        [ForeignKey("Schedule")]
        public int? ScheduleID { get; set; }

        public virtual Schedule? Schedule { get; set; }
    }
}
