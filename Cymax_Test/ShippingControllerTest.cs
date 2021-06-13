using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TaylorFerens_Cymax_Application.Controllers;
using TaylorFerens_Cymax_Application.Interfaces;
using TaylorFerens_Cymax_Application.Models;
using Xunit;

namespace cymx_api_test
{
    public class ShippingControllerTest
    {
        ShippingController _controller;
        IShippingService _service;

        public ShippingControllerTest()
        {
            Dictionary<string, string> testConfig = new Dictionary<string, string> { { "API_ONE_KEY", "123jb1hjbkj23423421"},
                                                                                     {"API_TWO_KEY", "123j34bn12kj3b1jh3v"},
                                                                                     {"API_THREE_KEY", "234jb23kjh4vb23j"}};

            _service = new ShippingServiceTest(testConfig);
            _controller = new ShippingController(_service);
        }

        [Fact]
        public void Get_Best_Price()
        {
            Shipping shipping = new Shipping
            {
                SourceAddress = "123 Fake St, Winnipeg, Manitoba, Canada, 0H0H0H",
                DestinationAddress = "456 Real St, Vancouver, British Columbia, Canada, 3F4F5F",
                Cartons = new List<Carton> { new Carton { Height = 5, Length = 8, Width = 9 } }

            };

            var result = _controller.Get(shipping);

            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}
