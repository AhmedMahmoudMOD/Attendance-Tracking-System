using Attendance_Tracking_System.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance_Tracking_System.Models
{
    public class Student : User
    {
    
        public string University { get; set; }

        public string Faculty { get; set; }

        public int GraduationYear { get; set; }

        public string? Specialization { get; set; }

        public int AttendanceDegrees { get; set; } = 250;
        [Column("RegStatus")]
        public RegisterationStatus RegisterationStatus { get; set; } = RegisterationStatus.Pending;

        [ForeignKey("Program")]
        public int ProgramID { get; set; }

        [ForeignKey("Track")]
        public int TrackID { get; set; }
        [ForeignKey("Intake")]
        public int IntakeNo { get; set; }

        public virtual ITIProgram? Program { get; set; }

        public virtual Track? Track { get; set; }

        public virtual Intake? Intake { get; set; }

        public ICollection<Permission> Permissions { get; set; }
    }
}
