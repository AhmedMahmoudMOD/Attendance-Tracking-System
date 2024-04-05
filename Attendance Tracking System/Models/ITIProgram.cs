using Attendance_Tracking_System.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Attendance_Tracking_System.Models
{
    [Table("Program")]
    public class ITIProgram
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ProgramDuration Duration { get; set; } // Number of Months

        public bool IsDeleted { get; set; } = false; // Soft delete prop
        [JsonIgnore]
        public virtual ICollection<Track>? Tracks { get; set; } = new HashSet<Track>();
        [JsonIgnore]
        public virtual ICollection<Intake>? Intakes { get; set; } = new HashSet<Intake>();
        [JsonIgnore]
        public virtual ICollection<Instructor>? Instructors { get; set; } = new HashSet<Instructor>();
        [JsonIgnore]
        public virtual ICollection<Student>? Students { get; set; } = new HashSet<Student>();
    }
}
