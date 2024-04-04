namespace Attendance_Tracking_System.Repositories
{
    public interface IUploadFile
    {
        public Task<string> UploadFile(IFormFile file);
    }
}
