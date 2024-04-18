using Attendance_Tracking_System.Controllers;
using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Attendance_Tracking_System.Repositories
{
	public class InstructorRepo : IInstructorRepo
	{
		private readonly ITISysContext db;

		public InstructorRepo(ITISysContext db)
		{
			this.db = db;
		}

		public List<Instructor> GetForAttendance()
		{
			// get only the instructors who does not have attendance for today
			var today = DateOnly.FromDateTime(DateTime.Now);
			var list = db.Instructor
				.Where(s => !db.Attendance.Any(a => a.UserID == s.Id && a.Date == today) && s.IsDeleted == false)
				.ToList();

			return list;
		}

		public List<Instructor> GetForAttendanceExplicit(DateOnly date)
		{


			var list = db.Instructor.Include(i => i.Attendances)
				.Where(i => i.Attendances.Any(a => a.Date == date))
				.ToList();

			return list;
		}

		public List<Instructor> GetForRangeAttendanceExplicit(DateOnly date, DateOnly endDate)
		{


			var list = db.Instructor.Include(i => i.Attendances.Where(a => a.Date >= date && a.Date <= endDate))
				.Where(i => i.Attendances.Any(a => a.Date >= date && a.Date <= endDate))
				.ToList();

			return list;
		}
		public List<object> GetForAttendanceReport(DateOnly date)
		{
			var list = db.Instructor
				.SelectMany(i => i.Attendances.Where(a => a.Date == date)
											   .Select(a => new
											   {
												   InstructorId = i.Id,
												   InstructorName = i.Name,
												   AttendanceStatus = a.AttendanceStatus,
												   ArrivalTime = a.ArrivalTime,
												   LeaveTime = a.LeaveTime
											   }))
				.ToList();

			return list.Cast<object>().ToList();
		}

		public List<object> GetForRangeAttendanceReport(DateOnly date, DateOnly EndDate)
		{
			var list = db.Instructor
				.SelectMany(i => i.Attendances.Where(a => a.Date >= date && a.Date <= EndDate)
											   .Select(a => new
											   {
												   InstructorId = i.Id,
												   InstructorName = i.Name,
												   Date = a.Date,
												   AttendanceStatus = a.AttendanceStatus,
												   ArrivalTime = a.ArrivalTime,
												   LeaveTime = a.LeaveTime
											   }))
				.ToList();

			return list.Cast<object>().ToList();
		}

		public void AddNewInstructor(Instructor instructor)
		{
			db.Instructor.Add(instructor);
			db.SaveChanges();
		}
		public void DeleteInstructor(int InsID)
		{
			throw new NotImplementedException();
		}
		public void EditInstructor(Instructor Ins)
		{
			var OldInstructor = db.Instructor.SingleOrDefault(a => a.Id == Ins.Id);
			OldInstructor.Name = Ins.Name;
			OldInstructor.Age = Ins.Age;
			OldInstructor.Email = Ins.Email;
			OldInstructor.Salary = Ins.Salary;
			OldInstructor.PhoneNumber = Ins.PhoneNumber;
			//var existingInstructor = db.Instructor.Find(Ins.Id);
			//if (existingInstructor != null)
			//{
			//db.Entry(existingInstructor).State = EntityState.Detached;
			//}
			//db.Instructor.Update(Ins);
			db.SaveChanges();
		}

		public List<Instructor> GetAllInstructors()
		{
			return db.Instructor.ToList();
		}

		public Instructor GetInstructorById(int id)
		{
			return db.Instructor.Include(a => a.Tracks).SingleOrDefault(a => a.Id == id);
		}

		public void UpdateInstructorImage(string InsImgName, int Id)
		{
			var instructor = db.Instructor.FirstOrDefault(s => s.Id == Id);
			instructor.UserImage = InsImgName;
			db.SaveChanges();
		}

		HashSet<Schedule> IInstructorRepo.getSheduleForTrack(int id)
		{
			var Track = db.Track.Include(a => a.Schedules).SingleOrDefault(a => a.SuperID == id);
			return Track.Schedules.ToHashSet();
		}
		public List<Instructor> GetAll()
		{
			return db.Instructor.ToList();
		}


		public List<Permission> getPermissionsByDateAndTrack(DateOnly date, List<Permission> permissions)
		{
			return permissions.Where(a => a.Date == date).ToList();
		}

		public List<Permission> GetPermissionsByTrack(int id)
		{
			int trackId = db.Track.FirstOrDefault(a => a.SuperID == id).Id;
			List<Permission> pers = db.Permission.Include(a => a.Student).ToList();
			List<Permission> filter = pers.Where(a => a.Student.TrackID == trackId).ToList();
			return filter;
		}


		public List<Track> getInstructorTracks(int id)
		{
			var instructor = db.Instructor.Include(a => a.Tracks).FirstOrDefault(a => a.Id == id);
			return instructor.Tracks.ToList();
		}

		public List<Attendance> getAttandence(int id)
		{
			return db.Attendance.Where(a => a.UserID == id).ToList();
		}

		public List<Schedule> getWeeklyTable(int id, DateOnly date)
		{
			int trackId = db.Track.SingleOrDefault(a => a.SuperID == id)?.Id ?? -1; // Default value if not found
			if (trackId == -1)
			{
				// Handle the case where the track is not found
				return new List<Schedule>();
			}

			Schedule schedule = db.Schedule.SingleOrDefault(sh => sh.TrackID == trackId && sh.Date == date);
			if (schedule == null)
			{
				// Handle the case where the schedule is not found
				return new List<Schedule>();
			}

			int startDay = schedule.Date.Day;
			int startMonth = schedule.Date.Month;
			int startYear = schedule.Date.Year;
			int scheduleId = schedule.Id;
			List<Schedule> weeklySchedule = new List<Schedule>();
			for (int i = startDay; i < startDay + 6; i++)
			{
				Schedule sc = db.Schedule.FirstOrDefault(
					a => a.Date.Day == i && a.Date.Month == startMonth && a.Date.Year == startYear);
				weeklySchedule.Add(sc);
			}

			return weeklySchedule;
		}
	}
}
