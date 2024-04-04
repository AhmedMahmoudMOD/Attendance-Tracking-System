using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface ITrackRepo
    {
        List<Track> getAllTracks();

        Track getTrackById(int id);
        public List<Track> GetAll();
       // public Track GetById(int id);
    }
}
