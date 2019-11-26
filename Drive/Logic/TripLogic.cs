using Drive.BusinessObjects;
using Drive.Logic.Interfaces;
using Drive.Utilities.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Drive.Logic
{
    public class TripLogic: ITripLogic
    {
        public List<Driver> RegisterDrivers(IStreamReader reader)
        {
            const string driverCommand = "driver";

            string line;
            string[] input;
            List<Driver> drivers = new List<Driver>();

            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();

                if (!string.IsNullOrEmpty(line) && line.ToLower().Contains(driverCommand))
                {
                    input = Regex.Split(line, @"\s+");

                    string driverName = input[1];

                    Driver driver = new Driver()
                    {
                        Name = driverName
                    };

                    drivers.Add(driver);
                }
            }

            return drivers;
        }

        public List<Trip> RegisterTrips(IStreamReader reader, List<Driver> drivers)
        {
            const string tripCommand = "trip";

            string line;
            string[] input;
            List<Trip> trips = new List<Trip>();

            reader.SetPosition(1);

            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();

                if (!string.IsNullOrEmpty(line) && line.ToLower().Contains(tripCommand))
                {
                    input = Regex.Split(line, @"\s+");

                    string driverName = input[1];
                    TimeSpan startTime = TimeSpan.Parse(input[2], CultureInfo.CurrentCulture);
                    TimeSpan endTime = TimeSpan.Parse(input[3], CultureInfo.CurrentCulture);
                    decimal milesDriven = decimal.Parse(input[4]);

                    Driver currentDriver = drivers.FirstOrDefault(x =>
                        x.Name != null && x.Name.Equals(driverName, StringComparison.CurrentCultureIgnoreCase));

                    if (currentDriver == null || (startTime > endTime))
                    {
                        continue;
                    }

                    Trip trip = new Trip()
                    {
                        Driver = currentDriver,
                        StartTime = startTime,
                        EndTime = endTime,
                        MilesDriven = milesDriven
                    };

                    trips.Add(trip);
                }
            }

            return trips;
        }

        public List<string> CalculateResults(List<Driver> drivers, List<Trip> trips)
        {
            List<string> driverResults = new List<string>();

            IEnumerable<DriverTotals> driverTotals = trips.GroupBy(trip => trip.Driver.Name)
                .Select(tripGroup => new DriverTotals()
                {
                    Name = tripGroup.Key,
                    TotalMilesDriven = tripGroup.Sum(x => x.MilesDriven),
                    TotalMinutesDriven = tripGroup.Sum(x => x.Duration.TotalMinutes)
                });

            IEnumerable<DriverAverages> driverAverages = driverTotals.Select(x => new DriverAverages
            {
                Name = x.Name,
                TotalMilesDriven = Math.Round(x.TotalMilesDriven),
                AverageMph = Math.Round((double)x.TotalMilesDriven / (x.TotalMinutesDriven / 60))
            });

            List<DriverAverages> validAverages = driverAverages
                .Where(x => x.AverageMph >= 5 && x.AverageMph <= 100).OrderByDescending(x => x.TotalMilesDriven).ToList();

            validAverages.ForEach(x =>
            {
                driverResults.Add($"{x.Name}: {x.TotalMilesDriven} miles @ {x.AverageMph} mph");
            });

            driverResults.AddRange(drivers.OrderBy(x => x.Name)
                .Where(driver => trips.All(x => x.Driver != driver))
                .Select(driver => $"{driver.Name}: 0 miles"));

            return driverResults;
        }
    }
}