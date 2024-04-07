using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Tracking_System.Models
{
    public class Intake
    {
        [Key]
        public int No { get; set; }

        public string Name { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("Program")]
        public int? ProgramID { get; set; }
        [JsonIgnore]
        public virtual ITIProgram? Program { get; set; }
        [JsonIgnore]
        public virtual ICollection<Track>? Tracks { get; set; } = new HashSet<Track>();
        [JsonIgnore]
        public virtual ICollection<Instructor>? Instructors { get; set; } = new HashSet<Instructor>();
        [JsonIgnore]
        public virtual ICollection<Student>? Students { get; set; } = new HashSet<Student>();
    }
}
