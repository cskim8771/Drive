using System;
using System.Diagnostics.CodeAnalysis;
using Drive.BusinessObjects.Interfaces;

namespace Drive.BusinessObjects
{
    [ExcludeFromCodeCoverage]
    public class Trip : ITrip
    {
        public Driver Driver { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal MilesDriven { get; set; }

        public TimeSpan Duration => EndTime - StartTime;
    }
}