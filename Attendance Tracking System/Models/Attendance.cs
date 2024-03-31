using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Attendance_Tracking_System.Models
{
    public class Attendance
    {
        [Key]
        public int AttID { get; set; }
        public DateOnly Date { get; set; }

        public TimeOnly? ArrivalTime { get; set; }

        public TimeOnly? LeaveTime { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User? User { get; set; }
    }
}
