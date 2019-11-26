using System;

namespace Drive.Utilities.Interface
{
    public interface IConsole
    {
        void Write(string message);
        void WriteLine(string message);
        string ReadLine();
        ConsoleKeyInfo ReadKey();
    }
}