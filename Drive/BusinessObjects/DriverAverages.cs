using System.Diagnostics.CodeAnalysis;

namespace Drive.BusinessObjects
{
    [ExcludeFromCodeCoverage]
    public class DriverAverages
    {
        public string Name { get; set; }

        public decimal TotalMilesDriven { get; set; }
        public double AverageMph {get; set;}
    }
}