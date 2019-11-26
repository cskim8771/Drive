using System.Diagnostics.CodeAnalysis;
using Drive.BusinessObjects.Interfaces;

namespace Drive.BusinessObjects
{
    [ExcludeFromCodeCoverage]
    public class Driver : IDriver
    {
        public string Name { get; set; }
    }
}