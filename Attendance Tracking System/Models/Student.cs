using Attendance_Tracking_System.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Tracking_System.Models
{
    public class Student : User
    {
        [Required]
        [StringLength(30,MinimumLength =2)]
        public string University { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2)]

        public string Faculty { get; set; }
        [Required]
        [Range(2018,2024)]
        public int GraduationYear { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string? Specialization { get; set; }

        public int AttendanceDegrees { get; set; } = 250;
        [Column("RegStatus")]
        public RegisterationStatus RegisterationStatus { get; set; } = RegisterationStatus.Pending;

        [ForeignKey("Program")]
        public int? ProgramID { get; set; }

        [ForeignKey("Track")]
        public int? TrackID { get; set; }
        [ForeignKey("Intake")]
        public int? IntakeNo { get; set; }

        public virtual ITIProgram? Program { get; set; }

        public virtual Track? Track { get; set; }

        public virtual Intake? Intake { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
        public Student()
        {
            Permissions = new List<Permission>();
        }
    }
}
