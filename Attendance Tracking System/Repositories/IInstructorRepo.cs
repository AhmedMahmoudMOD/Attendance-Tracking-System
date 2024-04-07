using Attendance_Tracking_System.Controllers;
using Attendance_Tracking_System.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Attendance_Tracking_System.Repositories
{
    public interface IInstructorRepo
    {
        List<Instructor> GetAllInstructors();

        void AddNewInstructor(Instructor instructor);
        void EditInstructor(Instructor Ins);

        void DeleteInstructor(int InsID);

        Instructor GetInstructorById(int id);

        void UpdateInstructorImage(string stdImageName, int stdId);

        HashSet<Schedule> getSheduleForTrack(int id);
        List<Instructor> GetForAttendance();

        List<Instructor> GetForAttendanceExplicit(DateOnly date);

        List<Instructor> GetForRangeAttendanceExplicit(DateOnly date, DateOnly endDate);

        List<object> GetForAttendanceReport(DateOnly date);

        List<object> GetForRangeAttendanceReport(DateOnly date, DateOnly EndDate);
        List<Instructor> GetAll();

        List<Schedule> getWeeklyTable(int id, DateOnly date);
        }
}
