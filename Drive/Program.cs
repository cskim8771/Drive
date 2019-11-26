using Drive.BusinessObjects;
using Drive.Logic;
using Drive.Logic.Interfaces;
using Drive.Utilities;
using Drive.Utilities.Interface;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Drive
{
    public class Program
    {
        [ExcludeFromCodeCoverage]
        public static void Main(string[] args)
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<ITripLogic, TripLogic>()
                .AddSingleton<IConsole, ConsoleWrapper>()
                .AddSingleton<IStreamReader, StreamReaderWrapper>()
                .BuildServiceProvider();

            ITripLogic tripLogic = serviceProvider.GetService<ITripLogic>();

            IConsole console = serviceProvider.GetService<IConsole>();

            IStreamReader reader = serviceProvider.GetService<IStreamReader>();

            Run(tripLogic, console, reader);
            
            console.WriteLine("Press any key to exit.");
            console.ReadKey();
        }

        public static void Run(ITripLogic tripLogic, IConsole console, IStreamReader streamReader)
        {
            string filePath = PromptForFile(console);

            int attempts = 1;

            while (!streamReader.FileExists(filePath))
            {
                if (attempts < 3)
                {
                    console.WriteLine("File not found. Please try again.\n");

                    filePath = PromptForFile(console);
                }
                else
                {
                    console.WriteLine("You have exceeded the maximum number of tries.\n");
                    return;
                }

                attempts++;
            }
            
            streamReader.CreateReader(filePath);

            List<Driver> drivers = tripLogic.RegisterDrivers(streamReader);

            List<Trip> trips = tripLogic.RegisterTrips(streamReader, drivers);

            List<string> driverResults = tripLogic.CalculateResults(drivers, trips);

            if (driverResults != null && driverResults.Any())
            {
                driverResults.ForEach(console.WriteLine);
            }
        }

        public static string PromptForFile(IConsole console)
        {
            console.Write("Enter file name: ");
            string file = console.ReadLine();
            return $"Data/{file}";
        }
    }
}