using Attendance_Tracking_System.Data;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Tracking_System.Repositories
{
    public class UploadFileRepo : IUploadFile
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ITISysContext db;
        public UploadFileRepo(IWebHostEnvironment hostingEnvironment, ITISysContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            db = context;
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            string fileName = null;
            if (file != null && file.Length > 0)
            {
                fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                db.SaveChanges();
            }

            return fileName;
        }
    }
}