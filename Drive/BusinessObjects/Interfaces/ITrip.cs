using System;

namespace Drive.BusinessObjects.Interfaces
{
    public interface ITrip
    {
        TimeSpan StartTime { get; set; }
        TimeSpan EndTime { get; set; }
        decimal MilesDriven { get; set; }
    }
}