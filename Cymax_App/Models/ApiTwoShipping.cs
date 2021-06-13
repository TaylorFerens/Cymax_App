using System.Collections.Generic;

namespace TaylorFerens_Cymax_Application.Models
{
    public class ApiTwoShipping
    {
        public string Consignee { get; set; }
        public string Consignor { get; set; }
        public List<int> Cartons { get; set; }
    }
}
