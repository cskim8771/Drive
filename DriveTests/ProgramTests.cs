using Drive;
using Drive.BusinessObjects;
using Drive.Logic.Interfaces;
using Drive.Utilities.Interface;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace DriveTests
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void Run_writes_FileNotFoundMessage_to_console_when_FileNotFound()
        {
            // arrange
            IConsole console = NSubstitute.Substitute.For<IConsole>();
            ITripLogic tripLogic = Substitute.For<ITripLogic>();
            IStreamReader streamReader = Substitute.For<IStreamReader>();

            console.ReadLine().Returns("file");

            streamReader.FileExists(Arg.Any<string>()).Returns(false, true);
            
            tripLogic.CalculateResults(new List<Driver>(), new List<Trip>()).Returns(new List<string>());

            // act
            Program.Run(tripLogic, console, streamReader);

            // assert
            console.Received().WriteLine("File not found. Please try again.\n");
        }

        [Test]
        public void Run_writes_MaxNumberExceeded_to_console_when_FileNotFound3Times()
        {
            // arrange
            IConsole console = Substitute.For<IConsole>();
            ITripLogic tripLogic = Substitute.For<ITripLogic>();
            IStreamReader streamReader = Substitute.For<IStreamReader>();

            console.ReadLine().Returns("file");

            streamReader.FileExists(Arg.Any<string>()).Returns(false, false, false);
            
            // act
            Program.Run(tripLogic, console, streamReader);

            // assert
            console.Received().WriteLine("You have exceeded the maximum number of tries.\n");
        }

        [Test]
        public void PromptForFile_return_FilePath()
        {
            // arrange
            IConsole console = Substitute.For<IConsole>();

            console.ReadLine().Returns("file.txt");
            // act
            string result = Program.PromptForFile(console);

            // assert
            Assert.AreEqual("Data/file.txt", result);
        }

        [Test]
        public void Run_writes_DriverResults_to_console_when_DriverResultsExist()
        {
            // arrange
            IConsole console = Substitute.For<IConsole>();
            ITripLogic tripLogic = Substitute.For<ITripLogic>();
            IStreamReader streamReader = Substitute.For<IStreamReader>();

            var driverResults = new List<string>()
            {
                "test string",
            };

            console.ReadLine().Returns("file");

            streamReader.FileExists(Arg.Any<string>()).Returns(true);
            tripLogic.CalculateResults(new List<Driver>(), new List<Trip>()).ReturnsForAnyArgs(driverResults);

            // act
            Program.Run(tripLogic, console, streamReader);

            // assert
            console.Received().WriteLine(driverResults[0]);
        }
    }
}