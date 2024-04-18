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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PermissionID { get; set; }
        [Required(ErrorMessage = "Reason is required"), MaxLength(100), MinLength(4)]
        public string Reason { get; set; }

        public bool? IsAccepted { get; set; }

        public bool? IsDeleted { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public DateOnly Date { get; set; }
        public PermissionType Type { get; set; }

        [ForeignKey("Student")]
        public int StudentID { get; set; }
        [JsonIgnore]
        public virtual Student? Student { get; set; }
    }
}
