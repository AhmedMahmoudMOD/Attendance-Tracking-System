using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance_Tracking_System.Repositories
{
    public class TrackRepo : ITrackRepo
    {
        private readonly ITISysContext db;

       public TrackRepo(ITISysContext db)
        {
            this.db = db; 
        }
        public List<Track> GetAll()
        {
            return db.Track.ToList();
        }

        public List<Track> getAllTracks()
        {
            return db.Track.ToList();
        }

        public List<Track> getInstructorTracks(int Id)
        {
            return db.Track.Include(a=>a.Instructors).ToList();
            throw new NotImplementedException();
        }

        public Track getTrackById(int id)
        {
            return db.Track.SingleOrDefault(a => a.Id == id);
        }
    }
}
