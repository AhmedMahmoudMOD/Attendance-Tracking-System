using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Attendance_Tracking_System.Models
{
    public class Track
    {
        public int Id { get; set; }
		[Required]
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabetic characters are allowed.")]
		public string Name { get; set; }
		[Range(0, 100, ErrorMessage = "The value must be between 0 and 100.")]
		public int Capacity { get; set; }

        public bool Status { get; set; } = true;

        [ForeignKey("Program")]
        public int? ProgramID { get; set; }
        [JsonIgnore]
        public virtual ITIProgram? Program { get; set; }
        [JsonIgnore]
        public virtual ICollection<Intake>? Intakes { get; set; } = new HashSet<Intake>();
        [JsonIgnore]
        public virtual ICollection<Instructor>? Instructors { get; set; } = new HashSet<Instructor>();
        [JsonIgnore]
        public virtual ICollection<Student>? Students { get; set; } = new HashSet<Student>();
        [JsonIgnore]
        public virtual ICollection<Schedule> Schedules { get; set; } = new HashSet<Schedule>();
        [JsonIgnore]
        public virtual Instructor? Supervisor { get; set; }
        [ForeignKey("Supervisor")]
        public int? SuperID { get; set; }
    }
}
