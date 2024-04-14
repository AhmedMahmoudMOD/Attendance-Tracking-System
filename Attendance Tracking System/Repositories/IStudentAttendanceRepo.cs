using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IStudentAttendanceRepo
    {
        bool Add(StudentAttendance studentAttendance);

        StudentAttendance GetStudentAttendance(int studentId, DateOnly date);

        bool MarkAbsence(List<Student> students,int ScheduleID);

        bool Update(StudentAttendance studentAttendance);

        bool CalculateNoOfDeductions(Student student);

    }
}
