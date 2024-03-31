namespace Attendance_Tracking_System.Models
{
    public class Instructor : User
    {
        public int Salary {  get; set; }

        public virtual ICollection<Track>? Tracks { get; set; } = new HashSet<Track>();

        public virtual ICollection<Intake>? Intakes { get; set; } = new HashSet<Intake>();

        public virtual ICollection<ITIProgram>? Programs { get; set; } = new HashSet<ITIProgram>();

        public virtual Track? SupTrack { get; set; }
    }
}
