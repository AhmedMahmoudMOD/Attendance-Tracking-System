using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Attendance_Tracking_System.CustomFilters
{
    public class UniqueEmail : ValidationAttribute
    {
        private readonly ITISysContext _db;
        public string ErrorMessage { get; set; }
        public UniqueEmail(ITISysContext db)
        {
            _db = db;
        }

        public UniqueEmail(string errorMessage) : base(errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        // public UniqueEmail() { }    

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string uniqueName = value.ToString();
            Instructor ? ins = _db.Instructor.FirstOrDefault(a => a.Email == uniqueName);
            if (ins == null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}