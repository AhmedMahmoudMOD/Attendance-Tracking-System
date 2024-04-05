using System.Text.Json.Serialization;

namespace Attendance_Tracking_System.Models
{
    public class Instructor : User
    {
        public int Salary {  get; set; }
        [JsonIgnore]
        public virtual ICollection<Track>? Tracks { get; set; } = new HashSet<Track>();
        [JsonIgnore]
        public virtual ICollection<Intake>? Intakes { get; set; } = new HashSet<Intake>();
        [JsonIgnore]
        public virtual ICollection<ITIProgram>? Programs { get; set; } = new HashSet<ITIProgram>();
        [JsonIgnore]
        public virtual Track? SupTrack { get; set; }
        public override string ToString()
        {
            return $"{Salary}";
        }
    }
}
