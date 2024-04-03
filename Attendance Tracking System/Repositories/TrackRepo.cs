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
        public List<Track> GetAll()
        {
            return db.Track.ToList();
        }
    }
}
