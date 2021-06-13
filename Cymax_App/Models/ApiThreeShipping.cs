using System.Collections.Generic;

namespace TaylorFerens_Cymax_Application.Models
{
    public class ApiThreeShipping
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public List<Package> Packages { get; set; }
    }

    public class Package
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Length { get; set; }
    }
}
