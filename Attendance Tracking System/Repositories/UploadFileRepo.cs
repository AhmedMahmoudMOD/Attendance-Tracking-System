using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        public async Task<string> UploadFile(IFormFile file,int userId,string userName)
        {
            string fileName = null;
            if (file != null && file.Length > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                // Combine username, id, and extension to create a new filename
                fileName = $"{userName}_{userId}{extension}";
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return fileName;
        }
    }
}