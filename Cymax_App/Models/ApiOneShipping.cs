using System.Collections.Generic;

namespace TaylorFerens_Cymax_Application.Models
{
    public class ApiOneShipping
    {
        public string ContactAddress { get; set; }
        public string WareHouseAddress { get; set; }
        public List<int> PackageDimensions { get; set; }
    }
}
