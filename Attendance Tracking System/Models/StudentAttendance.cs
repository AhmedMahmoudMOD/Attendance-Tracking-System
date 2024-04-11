using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Attendance_Tracking_System.Models
{
    public class StudentAttendance : Attendance
    {
        [ForeignKey("Schedule")]
        public int? ScheduleID { get; set; }
        [JsonIgnore]
        public virtual Schedule? Schedule { get; set; }

        public bool? IsMarked {  get; set; } 
    }
}
