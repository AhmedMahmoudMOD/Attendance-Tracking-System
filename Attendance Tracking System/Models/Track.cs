using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Attendance_Tracking_System.Models
{
    public class Track
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public bool Status { get; set; } = true;

        [ForeignKey("Program")]
        public int? ProgramID { get; set; }
        [JsonIgnore]
        public virtual ITIProgram? Program { get; set; }

        public virtual ICollection<Intake>? Intakes { get; set; } = new HashSet<Intake>();

        public virtual ICollection<Instructor>? Instructors { get; set; } = new HashSet<Instructor>();

        public virtual ICollection<Student>? Students { get; set; } = new HashSet<Student>();

        public virtual ICollection<Schedule> Schedules { get; set; } = new HashSet<Schedule>();

        public  Instructor? Supervisor { get; set; }
        [ForeignKey("Supervisor")]
        public int? SuperID { get; set; }
    }
}
