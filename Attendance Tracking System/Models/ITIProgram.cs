using Attendance_Tracking_System.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public virtual ICollection<Track>? Tracks { get; set; } = new HashSet<Track>();

        public virtual ICollection<Intake>? Intakes { get; set; } = new HashSet<Intake>();

        public virtual ICollection<Instructor>? Instructors { get; set; } = new HashSet<Instructor>();

        public virtual ICollection<Student>? Students { get; set; } = new HashSet<Student>();
    }
}
