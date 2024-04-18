using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Schedule;
using Schedule = Attendance_Tracking_System.Models.Schedule;

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
        //Function Get the date of the first day of the week for a given date\\
        public static DateOnly GetFirstDayOfWeek(DateOnly date)
        {
            // Calculate the difference between the current day of the week and the first day of the week (usually Sunday)
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Sunday)) % 7;

            // Subtract the difference from the current date to get the date of the first day of the week
            DateOnly firstDayOfWeek = date.AddDays(-diff);

            return firstDayOfWeek;
        }

        public List<Schedule> GetWeeklyShedule(int? id)
        {
			var todayDate = DateOnly.FromDateTime(DateTime.Now);
            var firstDayOfWeek = GetFirstDayOfWeek(todayDate);
            var lastDayOfWeek = firstDayOfWeek.AddDays(5);
			var schedule = db.Schedule.Include(a => a.Track).Where(a => a.TrackID == id && a.Date >= firstDayOfWeek && a.Date <= lastDayOfWeek ).ToList();
            return schedule;
		}
        public List<Schedule> GetAllScheduleForTrack(int? id)
        {
            var AllSchedules = db.Schedule.Include(a => a.Track).Where(a => a.TrackID == id).ToList();
            return AllSchedules;
        }

		public void AddSchedule(Schedule schedule)
        {
            
            db.Schedule.Add(schedule);
            db.SaveChanges();
        }

        public void AddWeeklySchedules(List<Schedule> schedules,int id)
        {
            List<Schedule> AllSC = getAllSchedulesByTrack(id);
            bool FoundDayAdded = false;
            foreach (var schedule in schedules)
            {
                var ScheduleDay = AllSC.FirstOrDefault(a => a.Date == schedule.Date );
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


        public List<Schedule> getAllSchedulesByTrack(int TrackID)
        {
            return db.Schedule.Where(a=>a.TrackID==TrackID).ToList();
        }
    }
}
