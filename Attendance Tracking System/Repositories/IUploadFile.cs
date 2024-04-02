namespace Attendance_Tracking_System.Repositories
{
    public interface IUploadFile
    {
        Task<string> uploadFile(IFormFile file);
    }
}
