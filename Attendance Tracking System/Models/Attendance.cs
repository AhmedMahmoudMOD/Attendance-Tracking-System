using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Attendance_Tracking_System.Enums;
using System.Text.Json.Serialization;

namespace Attendance_Tracking_System.Models
{
    public class Attendance
    {
        [Key]
        public int AttID { get; set; }
        public DateOnly Date { get; set; }

        public TimeOnly? ArrivalTime { get; set; }

        public TimeOnly? LeaveTime { get; set; }
        
        public AttendanceStatus? AttendanceStatus { get; set; }

        public string AttendanceType { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
