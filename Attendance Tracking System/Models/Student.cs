using Attendance_Tracking_System.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [StringLength(30, MinimumLength = 2)]
        public string? Specialization { get; set; }

        [Range(0, 250, ErrorMessage = "Attendance degrees must be between 0 and 250.")]
        public int AttendanceDegrees { get; set; } = 250;
        [Column("RegStatus")]
        public RegisterationStatus RegisterationStatus { get; set; } = RegisterationStatus.Pending;

        [ForeignKey("Program")]
        public int? ProgramID { get; set; }

        [ForeignKey("Track")]
        public int? TrackID { get; set; }
        [ForeignKey("Intake")]
        public int? IntakeNo { get; set; }
        [JsonIgnore]
        public virtual ITIProgram? Program { get; set; }
        [JsonIgnore]
        public virtual Track? Track { get; set; }
        [JsonIgnore]
        public virtual Intake? Intake { get; set; }
        [JsonIgnore]
        public virtual ICollection<Permission> Permissions { get; set; }
        public Student()
        {
            Permissions = new List<Permission>();
            Program = new ITIProgram();// to avoid null exception of program when regstatus is pending and program is Null
            Intake = new Intake();  

        }
    }
}
