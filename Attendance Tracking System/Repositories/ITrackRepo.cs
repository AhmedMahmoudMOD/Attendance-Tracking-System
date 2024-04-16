using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface ITrackRepo
    {
        List<Track> getAllTracks(out List<int> TrackCounter);

        Track getTrackById(int id);
        public List<Track> GetAll();

        List<Track> getTrackByProgram(int programID);
        public List<Track> getInstructorTracks(int ID); 
       // public Track GetById(int id);

        public int getTrackIDBySuperVisor(int ID);

        List<Instructor> getSuperVisor();

        Track getTrackByID(int ID);  

        Instructor GetTrackSuperVisor(int ID);

        void UpdateTrack(Track track);

        List<Instructor> NotSuperVisor();

        void RemoveInsFromTrack(List<int> RemovedIns, int TrackId);

        void AddInstructorToTrack(List<int> AddedIns, int TrackId);

        void AddTrack(Track _track);
    }
}
