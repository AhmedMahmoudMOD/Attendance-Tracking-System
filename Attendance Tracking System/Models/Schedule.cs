using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Tracking_System.Models
{
    public class Schedule
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }
        [ForeignKey("Track")]
        public int TrackID { get; set; }

        public virtual Track? Track { get; set; }
    }
}
