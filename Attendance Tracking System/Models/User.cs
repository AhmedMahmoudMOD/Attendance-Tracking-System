using NuGet.Protocol;
using System.ComponentModel.DataAnnotations;

namespace Attendance_Tracking_System.Models
{
    public abstract class User
    {
        public int Id { get; set; }

        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Password { get; set; }

        public int? Age { get; set; }
        [MinLength(11), MaxLength(11)]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        public string? UserImage { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<Attendance> Attendances { get; set; } = new HashSet<Attendance>();

        public override string ToString()
        {
            return $"{Id}:{Name}:{Email}:{Password}:{Age}:{PhoneNumber}";
        }
    }
}
