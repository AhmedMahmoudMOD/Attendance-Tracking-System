﻿using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf;
using Attendance_Tracking_System.Repositories;
using Syncfusion.Drawing;

namespace Attendance_Tracking_System.Controllers
{
    public class ReportController : Controller
    {
        private readonly IStudentRepo studentRepo;
        private readonly IInstructorRepo instructorRepo;
        private readonly IEmployeeRepo employeeRepo;

        public ReportController(IStudentRepo studentRepo,IInstructorRepo instructorRepo,IEmployeeRepo employeeRepo)
        {
            this.studentRepo = studentRepo;
            this.instructorRepo = instructorRepo;
            this.employeeRepo = employeeRepo;
        }
        public IActionResult GetStdAttendanceReport(int Pid,int Tid,int Ino,DateOnly Date)
        {
             var data = studentRepo.GetForAttendanceReport(Pid, Tid, Ino, Date);
             if (data != null)
                {
                    PdfDocument doc = new PdfDocument();
                    //Add a page.
                    PdfPage page = doc.Pages.Add();
                    //Create a PdfGrid.
                    PdfGrid pdfGrid = new PdfGrid();

                    PdfGraphics graphics = page.Graphics;
                    //Set the standard font.
                    PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 25);
                    //Draw the text.
                    graphics.DrawString("Attendance List", font, PdfBrushes.Black, new PointF(graphics.ClientSize.Width / 3, 150));
                    // Adding ITI Image 
                    FileStream imageStream = new FileStream("wwwroot/Images/iti.png", FileMode.Open, FileAccess.Read);
                    PdfBitmap image = new PdfBitmap(imageStream);
                    //Draw the image
                    graphics.DrawImage(image, 0, 0, 100, 100);

                    //Add list to IEnumerable.
                    IEnumerable<object> dataTable = data;
                    //Assign data source.
                    pdfGrid.DataSource = dataTable;
                    //Apply built-in table style
                    pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4);
                    for (int i = 0; i < pdfGrid.Rows.Count; i++)
                    {
                        pdfGrid.Rows[i].Height = 20;
                    }
                    // Paginate
                    PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
                    layoutFormat.Break = PdfLayoutBreakType.FitPage;
                    layoutFormat.Layout = PdfLayoutType.Paginate;

                    // Format 
                    PdfStringFormat stringFormat = new PdfStringFormat();
                    stringFormat.Alignment = PdfTextAlignment.Center;
                    stringFormat.LineAlignment = PdfVerticalAlignment.Middle;
                    //Apply string formatting for the whole table.
                    for (int i = 0; i < pdfGrid.Columns.Count; i++)
                    {
                        pdfGrid.Columns[i].Format = stringFormat;
                    }

                    // Renaming Table Headers 
                    PdfGridRow header = pdfGrid.Headers[0];

                    header.Cells[0].Value = "ID";
                    header.Cells[1].Value = "Name";
                    header.Cells[2].Value = "Status";
                    header.Cells[3].Value = "Arrival Time";
                    header.Cells[4].Value = "Leave Time";
                   

                //Draw grid to the page of PDF document.
                pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 200), layoutFormat);
                    //Write the PDF document to stream.
                    MemoryStream stream = new MemoryStream();
                    doc.Save(stream);
                    //If the position is not set to '0' then the PDF will be empty.
                    stream.Position = 0;
                    //Close the document.
                    doc.Close(true);
                    //Defining the ContentType for pdf file.
                    string contentType = "application/pdf";
                    //Define the file name.
                    string fileName = "Std Attendance Report.pdf";
                    //Creates a FileContentResult object by using the file contents, content type, and file name.
                    return File(stream, contentType, fileName);

             }
             else{
                    return NotFound();
             }
            
        }
    }
}