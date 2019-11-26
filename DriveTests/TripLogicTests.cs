using Drive.BusinessObjects;
using Drive.Logic;
using Drive.Utilities.Interface;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DriveTests
{
    [TestFixture]
    public class TripLogicTests
    {
        [Test]
        public void RegisterDriver_returns_NonEmptyDrivers()
        {
            // arrange
            IStreamReader reader = Substitute.For<IStreamReader>();

            reader.CreateReader(Arg.Any<string>());
            reader.EndOfStream.Returns(false, true);
            reader.ReadLine().Returns("Driver Test");

            TripLogic tripLogic = new TripLogic();

            // act
            List<Driver> drivers = tripLogic.RegisterDrivers(reader);

            // assert
            Assert.IsTrue(drivers.Any());
        }

        [Test]
        public void RegisterTrips_returns_NonEmptyTrips()
        {
            // arrange
            List<Driver> drivers = new List<Driver>
            {
                new Driver()
                {
                    Name = "Dan"
                },
                new Driver()
                {
                    Name = "Lauren"
                },
                new Driver()
                {
                    Name = "Kumi"
                }
            };

            IStreamReader reader = Substitute.For<IStreamReader>();

            reader.CreateReader(Arg.Any<string>());
            reader.EndOfStream.Returns(false, true);
            reader.ReadLine().Returns("Trip Dan 07:15 07:45 17.3");

            TripLogic tripLogic = new TripLogic();

            // act
            List<Trip> trips = tripLogic.RegisterTrips(reader, drivers);

            // assert
            Assert.IsTrue(trips.Any());
        }

        [Test]
        public void RegisterTrips_skips_TripEntry_if_DriverDoesNotExist()
        {
            // arrange
            List<Driver> drivers = new List<Driver>
            {
                new Driver()
                {
                    Name = "Dan"
                },
                new Driver()
                {
                    Name = "Lauren"
                },
                new Driver()
                {
                    Name = "Kumi"
                }
            };

            IStreamReader reader = Substitute.For<IStreamReader>();

            reader.CreateReader(Arg.Any<string>());
            reader.EndOfStream.Returns(false, true);
            reader.ReadLine().Returns("Trip Bob 07:15 07:45 17.3");

            TripLogic tripLogic = new TripLogic();

            // act
            List<Trip> trips = tripLogic.RegisterTrips(reader, drivers);

            // assert
            Assert.IsFalse(trips.Any());
        }

        [Test]
        public void RegisterTrips_skips_TripEntry_if_StartTimeGreaterEndTime()
        {
            // arrange
            List<Driver> drivers = new List<Driver>
            {
                new Driver()
                {
                    Name = "Dan"
                },
                new Driver()
                {
                    Name = "Lauren"
                },
                new Driver()
                {
                    Name = "Kumi"
                }
            };

            IStreamReader reader = Substitute.For<IStreamReader>();

            reader.CreateReader(Arg.Any<string>());
            reader.EndOfStream.Returns(false, true);
            reader.ReadLine().Returns("Trip Dan 07:45 07:15  17.3");

            TripLogic tripLogic = new TripLogic();

            // act
            List<Trip> trips = tripLogic.RegisterTrips(reader, drivers);

            // assert
            Assert.IsFalse(trips.Any());
        }

        [Test]
        public void RegisterTrips_returns_CorrectTripDataForDriver()
        {
            // arrange
            List<Driver> drivers = new List<Driver>
            {
                new Driver()
                {
                    Name = "Dan"
                }
            };

            IStreamReader reader = Substitute.For<IStreamReader>();

            reader.CreateReader(Arg.Any<string>());

            reader.EndOfStream.Returns(false, true);
            reader.ReadLine().Returns("Trip Dan 07:15 07:45 17.3");

            var expectedTripData = new Trip()
            {
                Driver = drivers[0],
                StartTime = TimeSpan.Parse("07:15"),
                EndTime = TimeSpan.Parse("07:45"),
                MilesDriven = (decimal)17.3
            };

            TripLogic tripLogic = new TripLogic();

            // act
            List<Trip> trips = tripLogic.RegisterTrips(reader, drivers);

            // assert
            Assert.AreEqual(expectedTripData.Driver.Name, trips[0].Driver.Name);
            Assert.AreEqual(expectedTripData.StartTime, trips[0].StartTime);
            Assert.AreEqual(expectedTripData.EndTime, trips[0].EndTime);
            Assert.AreEqual(expectedTripData.MilesDriven, trips[0].MilesDriven);
        }

        [Test]
        public void CalculateResults_returns_NonEmptyResults()
        {
            // arrange
            List<Driver> drivers = new List<Driver>()
            {
                new Driver()
                {
                    Name = "Dan"
                },
                new Driver()
                {
                    Name = "Lauren"
                },
                new Driver()
                {
                    Name = "Kumi"
                }
            };

            List<Trip> trips = new List<Trip>()
            {
                new Trip()
                {
                    Driver = new Driver()
                    {
                        Name = "Dan"
                    },
                    StartTime = TimeSpan.Parse("07:15"),
                    EndTime = TimeSpan.Parse("07:45"),
                    MilesDriven = (decimal) 17.3
                },
                new Trip()
                {
                    Driver = new Driver()
                    {
                        Name = "Dan"
                    },
                    StartTime = TimeSpan.Parse("06:12"),
                    EndTime = TimeSpan.Parse("06:32"),
                    MilesDriven = (decimal) 21.8
                },
                new Trip()
                {
                    Driver = new Driver()
                    {
                        Name = "Lauren"
                    },
                    StartTime = TimeSpan.Parse("12:01"),
                    EndTime = TimeSpan.Parse("13:16"),
                    MilesDriven = (decimal) 42.0
                }
            };

            TripLogic tripLogic = new TripLogic();

            // act
            List<string> result = tripLogic.CalculateResults(drivers, trips);

            // assert
            Assert.IsTrue(result.Any());
        }

        [Test]
        public void CalculateResults_returns_CorrectResultForDriver()
        {
            // arrange
            List<Driver> drivers = new List<Driver>()
            {
                new Driver()
                {
                    Name = "Dan"
                }
            };

            List<Trip> trips = new List<Trip>()
            {
                new Trip()
                {
                    Driver = new Driver()
                    {
                        Name = "Dan"
                    },
                    StartTime = TimeSpan.Parse("07:15"),
                    EndTime = TimeSpan.Parse("07:45"),
                    MilesDriven = (decimal) 17.3
                },
                new Trip()
                {
                    Driver = new Driver()
                    {
                        Name = "Dan"
                    },
                    StartTime = TimeSpan.Parse("06:12"),
                    EndTime = TimeSpan.Parse("06:32"),
                    MilesDriven = (decimal) 21.8
                }
            };

            string expectedResult = "Dan: 39 miles @ 47 mph";

            TripLogic tripLogic = new TripLogic();

            // act
            List<string> result = tripLogic.CalculateResults(drivers, trips);

            // assert
            Assert.AreEqual(expectedResult, result[0]);
        }

        [Test]
        public void CalculateResults_LessThanFiveTotalMiles_returns_ZeroMilesDriven()
        {
            // arrange
            List<Driver> drivers = new List<Driver>()
            {
                new Driver()
                {
                    Name = "Dan"
                }
            };

            List<Trip> trips = new List<Trip>()
            {
                new Trip()
                {
                    Driver = new Driver()
                    {
                        Name = "Dan"
                    },
                    StartTime = TimeSpan.Parse("07:15"),
                    EndTime = TimeSpan.Parse("07:45"),
                    MilesDriven = 1
                },
                new Trip()
                {
                    Driver = new Driver()
                    {
                        Name = "Dan"
                    },
                    StartTime = TimeSpan.Parse("06:12"),
                    EndTime = TimeSpan.Parse("06:32"),
                    MilesDriven = 2
                }
            };

            string expectedResult = "Dan: 0 miles";

            TripLogic tripLogic = new TripLogic();

            // act
            List<string> result = tripLogic.CalculateResults(drivers, trips);

            // assert
            Assert.AreEqual(expectedResult, result[0]);
        }

        [Test]
        public void CalculateResults_GreaterThanOneHundredTotalMiles_returns_ZeroMilesDriven()
        {
            // arrange
            List<Driver> drivers = new List<Driver>()
            {
                new Driver()
                {
                    Name = "Dan"
                }
            };

            List<Trip> trips = new List<Trip>()
            {
                new Trip()
                {
                    Driver = new Driver()
                    {
                        Name = "Dan"
                    },
                    StartTime = TimeSpan.Parse("07:15"),
                    EndTime = TimeSpan.Parse("07:45"),
                    MilesDriven = 50
                },
                new Trip()
                {
                    Driver = new Driver()
                    {
                        Name = "Dan"
                    },
                    StartTime = TimeSpan.Parse("06:12"),
                    EndTime = TimeSpan.Parse("06:32"),
                    MilesDriven = 34
                }
            };

            string expectedResult = "Dan: 0 miles";

            TripLogic tripLogic = new TripLogic();

            // act
            List<string> result = tripLogic.CalculateResults(drivers, trips);

            // assert
            Assert.AreEqual(expectedResult, result[0]);
        }

        [Test]
        public void CalculateResults_returns_ResultsSortedByMilesDrivenDesc()
        {
            // arrange
            List<Driver> drivers = new List<Driver>()
            {
                new Driver()
                {
                    Name = "Dan"
                },
                new Driver()
                {
                    Name = "Lauren"
                },
                new Driver()
                {
                    Name = "Kumi"
                }
            };

            List<Trip> trips = new List<Trip>()
            {
                new Trip()
                {
                    Driver = new Driver()
                    {
                        Name = "Kumi"
                    },
                    StartTime = TimeSpan.Parse("07:15"),
                    EndTime = TimeSpan.Parse("07:45"),
                    MilesDriven = (decimal) 17.3
                },
                new Trip()
                {
                    Driver = new Driver()
                    {
                        Name = "Dan"
                    },
                    StartTime = TimeSpan.Parse("06:12"),
                    EndTime = TimeSpan.Parse("06:32"),
                    MilesDriven = (decimal) 21.8
                },
                new Trip()
                {
                    Driver = new Driver()
                    {
                        Name = "Lauren"
                    },
                    StartTime = TimeSpan.Parse("12:01"),
                    EndTime = TimeSpan.Parse("13:16"),
                    MilesDriven = (decimal) 42.0
                }
            };

            TripLogic tripLogic = new TripLogic();

            // act
            List<string> result = tripLogic.CalculateResults(drivers, trips);

            // assert
            Assert.AreEqual(0, result.FindIndex(x => x.Contains("Lauren")));
            Assert.AreEqual(1, result.FindIndex(x => x.Contains("Dan")));
            Assert.AreEqual(2, result.FindIndex(x => x.Contains("Kumi")));
        }
    }
}
