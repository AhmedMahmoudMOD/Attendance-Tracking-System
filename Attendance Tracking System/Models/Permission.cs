using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Attendance_Tracking_System.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Attendance_Tracking_System.Models
{
    [PrimaryKey("StudentID", "PermissionID")]
    public class Permission
    {
        public int PermissionID { get; set; }
        public string Reason { get; set; }

        public bool? IsAccepted { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime DateTime { get; set; }

        public PermissionType Type { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }
        [JsonIgnore]
        public virtual Student? Student { get; set; }
    }
}
