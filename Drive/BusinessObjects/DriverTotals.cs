using System.Diagnostics.CodeAnalysis;

namespace Drive.BusinessObjects
{
    [ExcludeFromCodeCoverage]
    public class DriverTotals
    {
        public string Name {get; set;}
        public decimal TotalMilesDriven {get; set;}
        public double TotalMinutesDriven{get; set;}
    }
}