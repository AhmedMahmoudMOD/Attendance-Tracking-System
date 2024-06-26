﻿using Microsoft.AspNetCore.Mvc;

namespace Attendance_Tracking_System.View_Models
{
    public class EmailModel
    {
        public string To {  get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public HttpPostedFileBase Attachment { get; set; }
        public string Email { get; set; }
        public string Password {  get; set; }
    }
}
