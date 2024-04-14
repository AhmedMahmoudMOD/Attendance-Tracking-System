namespace Attendance_Tracking_System.View_Models
{
    public class EditStudentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IFormFile Image { get; set; }
        public string ImagePath { get; set; }

    }
}
