using System;
using System.Diagnostics.CodeAnalysis;
using Drive.Utilities.Interface;

namespace Drive.Utilities
{
    [ExcludeFromCodeCoverage]
    public class ConsoleWrapper : IConsole
    {
        public void Write(string message)
        {
            Console.Write(message);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }
    }
}
