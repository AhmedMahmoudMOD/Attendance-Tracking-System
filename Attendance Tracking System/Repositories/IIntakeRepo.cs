﻿using Attendance_Tracking_System.Models;

namespace Attendance_Tracking_System.Repositories
{
    public interface IIntakeRepo
    {
        Intake GetCurrentIntake();
    }
}
