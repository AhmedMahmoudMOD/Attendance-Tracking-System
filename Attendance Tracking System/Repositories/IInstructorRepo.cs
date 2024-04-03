using Attendance_Tracking_System.Controllers;
using Attendance_Tracking_System.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Attendance_Tracking_System.Repositories
{
    public interface IInstructorRepo
    {
        ICollection<Instructor> GetAllInstructors();

        void AddNewInstructor(Instructor instructor);
        void EditInstructor(Instructor Ins);

        void DeleteInstructor(int InsID);

        Instructor GetInstructorById(int id);

        void UpdateInstructorImage(string stdImageName, int stdId);
        List<Instructor> GetForAttendance();

        List<Instructor> GetForAttendanceExplicit(DateOnly date);

        List<object> GetForAttendanceReport(DateOnly date);
    }
}
