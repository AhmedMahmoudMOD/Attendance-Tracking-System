using NuGet.Protocol;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using Attendance_Tracking_System.CustomFilters;

namespace Attendance_Tracking_System.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(10,MinimumLength =3)]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
       
		[RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "Please enter a valid email address.")]
		public string Email { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 6,ErrorMessage ="Password must be at least 6 characters")]
        public string Password { get; set; }

        [Range(20,100)]
        public int Age { get; set; }
        [MinLength(11), MaxLength(11)]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        public string? UserImage { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }

        public bool IsDeleted { get; set; } = false;
        [JsonIgnore]
        public virtual ICollection<Attendance> Attendances { get; set; } = new HashSet<Attendance>();
        [JsonIgnore]
        public virtual ICollection<Role> role { get; set; }= new HashSet<Role>();

        public override string ToString()
        {
            return $"{Id}:{Name}:{Email}:{Password}:{Age}:{PhoneNumber}";
        }
    }
}
