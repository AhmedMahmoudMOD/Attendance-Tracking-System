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

        public List<Track> getAllTracks(out List<int> TrackCounter)
        {
            var Tracks= db.Track.Include(a=>a.Students).ToList();
            //List<int> TrackCounter = new List<int>();  
              TrackCounter = new List<int>();  
            foreach (var item in Tracks)
            {
                TrackCounter.Add(item.Students.Count());
            }
            return Tracks;
        }

        public List<Track> getInstructorTracks(int Id)
        {
            return db.Track.Include(a=>a.Instructors).ToList();
         
        }

        public Track getTrackById(int id)
        {
            return db.Track.SingleOrDefault(a => a.Id == id);
        }

		public List<Track> getTrackByProgram(int programID)
		{
			return db.Track.Where(a=>a.ProgramID == programID).ToList();    
		}

		public int getTrackIDBySuperVisor(int ID)
        {
            return db.Track.FirstOrDefault(a => a.SuperID == ID).Id;
        }

        public List<Instructor> getSuperVisor()
        {
            var tracks=db.Track.Include(a => a.Instructors);
            List<Instructor> ins = new List<Instructor>();
            foreach (var item in tracks)
            {
                ins.Add(db.Instructor.SingleOrDefault(a => a.Id == item.SuperID));
            }
            return ins;
        }

        public Track getTrackByID(int ID)
        {
            return db.Track.Include(a=>a.Instructors).Include(a=>a.Intakes).Include(a=>a.Program) 
                           .SingleOrDefault(a => a.Id == ID);
        }

        public Instructor GetTrackSuperVisor(int ID)
        {
          var track=  db.Track.SingleOrDefault(a => a.SuperID == ID);
            return db.Instructor.SingleOrDefault(a => a.SupTrack == track);
        }


        public void UpdateTrack(Track track)
        {
            //db.Track.Update(track);
            var oldTrack = getTrackByID(track.Id);
            oldTrack.Name=track.Name;
            oldTrack.Capacity = track.Capacity;
            oldTrack.SuperID = track.SuperID;
            oldTrack.ProgramID = track.ProgramID;
            db.SaveChanges();
        }

        public List<Instructor> NotSuperVisor() {
            var InsNotSuperVisor = db.Instructor.Where(a=>a.SupTrack==null).ToList();
            return InsNotSuperVisor;
        }

        public void AddInstructorToTrack(List<int> AddedIns,int TrackId)
        {
            var Track = db.Track.Include(a=>a.Instructors).SingleOrDefault(a=>a.Id == TrackId);
            var TrackIns = Track.Instructors.ToList();
            var AllIns = db.Instructor.ToList();
            foreach (var item in AddedIns)
            {
                var instructor = AllIns.SingleOrDefault(a => a.Id == item);
                Track.Instructors.Add(instructor);
            }
            db.SaveChanges();
        }

        public void RemoveInsFromTrack(List<int> RemovedIns, int TrackId)
        {
            var Track = db.Track.Include(a => a.Instructors).SingleOrDefault(a => a.Id == TrackId);
            var TrackIns = Track.Instructors.ToList();
            foreach (var item in RemovedIns)
            {
                var instructor = TrackIns.SingleOrDefault(a => a.Id == item);
                Track.Instructors.Remove(instructor);
            }
            db.SaveChanges();
        }

        public void AddTrack(Track _track)
        {
           db.Track.Add(_track);
           db.SaveChanges();
        }
    }
}
