using Attendance_Tracking_System.Data;

using Attendance_Tracking_System.Enums;
using Attendance_Tracking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Tracking_System.Repositories
{
    public class StudentAttendanceRepo : IStudentAttendanceRepo
    {
        private readonly ITISysContext db;


        public StudentAttendanceRepo(ITISysContext db)
        {
            this.db = db;
        }
        public List<StudentAttendance> GetAllAttendance(int studentId)
        {
            var att = db.StudentAttendance.Where(s => s.UserID == studentId).ToList();
            return att;
        }


        public StudentAttendance GetAttendanceById(int id)
        {
            return db.StudentAttendance.FirstOrDefault(s => s.AttID == id);
        }

            public bool Add(StudentAttendance studentAttendance)
            {
                try
                {
                    db.StudentAttendance.Add(studentAttendance);
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public bool MarkAbsence(List<Student> students, int ScheduleID)
            {
                var today = DateOnly.FromDateTime(DateTime.Now);

            try
            {
                foreach (var student in students)
                {
                    var studentAttendance = new StudentAttendance
                    {
                        Date = today,
                        AttendanceStatus = AttendanceStatus.Absent,
                        AttendanceType = "StudentAttendance",
                        UserID = student.Id,
                        ScheduleID = ScheduleID
                    };
                    db.StudentAttendance.Add(studentAttendance);
                }
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
              
        }



            // get student attendance by student id and date
            public StudentAttendance GetStudentAttendance(int studentId, DateOnly date)
            {
                var studentAttendance = db.StudentAttendance.SingleOrDefault(s => s.UserID == studentId && s.Date == date);
                return studentAttendance;
            }

        public bool Update(StudentAttendance studentAttendance)
        {
            try
            {
                db.StudentAttendance.Update(studentAttendance);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CalculateNoOfDeductions(Student student)
        {
            try
            {
                var studentAttendance = db.StudentAttendance.SingleOrDefault(s => s.UserID == student.Id);
                if ((studentAttendance.AttendanceStatus == AttendanceStatus.Absent || studentAttendance.AttendanceStatus==AttendanceStatus.Late)&& studentAttendance.IsMarked==true)
                {
                    student.NoOfDeductions++;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    

    }
}
