using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Tracking_System.Models
{
    public class Intake
    {
        [Key]
        public int No { get; set; }
        [Required]
        [StringLength(10,MinimumLength =2,ErrorMessage ="Intake name must be at least 2 characters")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly? StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
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
