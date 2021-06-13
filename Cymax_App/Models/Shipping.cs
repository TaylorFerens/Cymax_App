using System.Collections.Generic;

namespace TaylorFerens_Cymax_Application.Models
{
    public class Shipping
    {
        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
        public List<Carton> Cartons { get; set; }
        public double Price { get; set; }
        public string ShippingService { get; set; }
    }
}
