﻿using Attendance_Tracking_System.Data;

namespace Attendance_Tracking_System.Repositories
{
    public class TrackRepo : ITrackRepo
    {
        private readonly ITISysContext db;

       public TrackRepo(ITISysContext db)
        {
            this.db = db;
        }
    }
}
