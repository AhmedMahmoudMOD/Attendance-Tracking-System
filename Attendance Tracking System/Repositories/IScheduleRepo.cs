using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IScheduleRepo
    {
        List<Schedule> GetAllSchedules();
        
        Schedule GetScheduleById(int id);  
        void AddsSchedule (Schedule schedule);
        
        void UpdateSchedule (Schedule schedule);
        void DeleteSchedule (int scheduleId);
        Schedule GetScheduleForToday(int TrackId,DateOnly date);
         List<Schedule> GetWeeklyShedule(int? id);
        List<Schedule> GetAllScheduleForTrack(int? TrackId);



        void AddWeeklySchedules(List<Schedule> schedules);
    }
}
