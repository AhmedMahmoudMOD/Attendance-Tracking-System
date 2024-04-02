using Attendance_Tracking_System.Data;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Tracking_System.Repositories
{
    public class UploadFile: IUploadFile
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ITISysContext db;
        public UploadFile(IWebHostEnvironment hostingEnvironment,ITISysContext _context)
        {
            _hostingEnvironment = hostingEnvironment;
            db = _context;
        }

        public async Task<string> uploadFile(IFormFile file)
        {
            string fileName = null;
            if (file != null && file.Length > 0)
            {
                fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Upload", fileName);
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
