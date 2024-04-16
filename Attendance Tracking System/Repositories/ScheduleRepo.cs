using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Tracking_System.Repositories
{
    public class ScheduleRepo : IScheduleRepo
    {
        private readonly ITISysContext db;

        public ScheduleRepo(ITISysContext db)
        {
            this.db = db;
        }

        public void AddsSchedule(Schedule schedule)
        {
            db.Schedule.Add(schedule);
            db.SaveChanges();
        }

        public void DeleteSchedule(int scheduleId)
        {
            throw new NotImplementedException();
        }

        public List<Schedule> GetAllSchedules()
        {
            return db.Schedule.Include(a=>a.Track).ToList();
        }

        public Schedule GetScheduleById(int id)
        {
            return db.Schedule.Include(a=>a.Track).SingleOrDefault(a => a.Id == id);
        }

        public void UpdateSchedule(Schedule schedule)
        {
            throw new NotImplementedException();
       
    }
     public Schedule GetScheduleForToday(int TrackId,DateOnly date)
        {
            var schedule = db.Schedule.SingleOrDefault(s => s.TrackID == TrackId && s.Date == date);
            return schedule;
        }

        public void AddSchedule(Schedule schedule)
        {
            
            db.Schedule.Add(schedule);
            db.SaveChanges();
        }

        public void AddWeeklySchedules(List<Schedule> schedules)
        {
            List<Schedule> AllSC = GetAllSchedules();
            bool FoundDayAdded = false;
            foreach (var schedule in schedules)
            {
                var ScheduleDay = AllSC.FirstOrDefault(a => a.Date == schedule.Date);
                if (ScheduleDay != null) 
                {
                    FoundDayAdded = true;
                    break;
                }
            }
            if (!FoundDayAdded)
            {
                db.Schedule.AddRange(schedules);
                db.SaveChanges();
            }
        }
    }
}
