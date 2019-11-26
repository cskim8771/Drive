using Drive.BusinessObjects;
using Drive.Utilities.Interface;
using System.Collections.Generic;

namespace Drive.Logic.Interfaces
{
    public interface ITripLogic
    {
        List<Driver> RegisterDrivers(IStreamReader reader);
        List<Trip> RegisterTrips(IStreamReader reader, List<Driver> drivers);
        List<string> CalculateResults(List<Driver> drivers, List<Trip> trips);
    }
}