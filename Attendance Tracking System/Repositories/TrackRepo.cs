using Attendance_Tracking_System.Data;
using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public class TrackRepo : ITrackRepo
    {
        private readonly ITISysContext db;

       public TrackRepo(ITISysContext db)
        {
            this.db = db;
        }

        public List<Track> getAllTracks()
        {
            return db.Track.ToList();
        }

        public Track getTrackById(int id)
        {
            return db.Track.SingleOrDefault(a => a.Id == id);
        }
    }
}
