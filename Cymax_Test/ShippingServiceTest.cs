using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TaylorFerens_Cymax_Application.Interfaces;
using TaylorFerens_Cymax_Application.Models;
using static TaylorFerens_Cymax_Application.Classes.ApiCommunication;

namespace cymx_api_test
{
    class ShippingServiceTest : IShippingService
    {
        #region Constants

        private const string ERROR_DETERMINE_SHIPPING = "ERROR: DetermineShipping(): an error occured while determining best shipping price. ";
        private const string ERROR_DETERMINE_SHIPPING_API_ONE = "ERROR: DeterminePriceAPIOne(): an error occured while determining the price from api one. ";
        private const string ERROR_DETERMINE_SHIPPING_API_TWO = "ERROR: DeterminePriceAPITwo(): an error occured while determining the price from api two. ";
        private const string ERROR_DETERMINE_SHIPPING_API_THREE = "ERROR: DeterminePriceAPIThree(): an error occured while determining the price from api three. ";
        private const string JSON_MEDIA_TYPE = "application/json";
        private const string XML_MEDIA_TYPE = "application/xml";
        private const string API_ONE_KEY = "API_ONE_KEY";
        private const string API_TWO_KEY = "API_TWO_KEY";
        private const string API_THREE_KEY = "API_THREE_KEY";

        #endregion
        #region Instance Variables

        private readonly Dictionary<string, string> _config;

        #endregion
        #region Constructor

        public ShippingServiceTest(Dictionary<string, string> config)
        {
            _config = config;
        }

        #endregion
        #region Public Methods

        public async Task<Shipping> DetermineBestShipping(Shipping shipping)
        {
            double priceAPIOne = -1;
            double priceAPITwo = -1;
            double? priceAPIThree = -1;

            double? lowestPrice = 0;

            try
            {
                // Determine the price from all three apis
                priceAPIOne =  DeterminePriceAPIOne(shipping);
                priceAPITwo =  DeterminePriceAPITwo(shipping);
                priceAPIThree =  DeterminePriceAPIThree(shipping);

                // Determine the lowest price
                lowestPrice = priceAPIOne;
                shipping.ShippingService = "ApiOneShipping";
                if (priceAPITwo < lowestPrice)
                {
                    lowestPrice = priceAPITwo;
                    shipping.ShippingService = "ApiTwoShipping";
                }

                if (priceAPIThree < priceAPITwo)
                {
                    lowestPrice = priceAPIThree;
                    shipping.ShippingService = "ApiThreeShipping";
                }

                shipping.Price = (double)lowestPrice;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ERROR_DETERMINE_SHIPPING + ex);
            }
            return shipping;
        }

        #endregion
        #region Private Methods

        private double DeterminePriceAPIOne(Shipping shipping)
        {
            double total = -1;
            ApiOneShipping apiOneShipping = null;
            string requestJson = null;
            HttpRequestMessage request = null;
            string jsonResponse = null;
            string apiKey = null;

            try
            {

                if (shipping != null)
                {
                    // Convert our object to api one's format
                    apiOneShipping = new ApiOneShipping
                    {
                        ContactAddress = shipping.SourceAddress,
                        WareHouseAddress = shipping.DestinationAddress,
                        PackageDimensions = { shipping.Cartons[0].Height, shipping.Cartons[0].Width, shipping.Cartons[0].Length }
                    };

                    // Convert the object to json, and prepare request
                    requestJson = JsonConvert.SerializeObject(apiOneShipping);
                    _config.TryGetValue(API_ONE_KEY, out apiKey);

                    request = MakeHttpRequest(requestJson, "http://apiOneShippingQuotes.com/GetQuote/", JSON_MEDIA_TYPE, apiKey);

                    // Send Request
                    //jsonResponse = await SendHttpRequest(request);

                    if (!string.IsNullOrEmpty(jsonResponse))
                    {
                        total = JsonConvert.DeserializeObject<double>(jsonResponse);
                    }
                    total = (shipping.Cartons[0].Height * shipping.Cartons[0].Width * shipping.Cartons[0].Length) * RandomShippingRate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ERROR_DETERMINE_SHIPPING_API_ONE + ex);
            }

            return total;
        }

        private double DeterminePriceAPITwo(Shipping shipping)
        {
            double amount = -1;
            ApiTwoShipping apiTwoShipping = null;
            string requestJson = null;
            HttpRequestMessage request = null;
            string jsonResponse = null;
            string apiKey = null;

            try
            {
                // Convert our object to api two's format
                apiTwoShipping = new ApiTwoShipping
                {
                    Consignee = shipping.SourceAddress,
                    Consignor = shipping.DestinationAddress,
                    Cartons = { shipping.Cartons[0].Height, shipping.Cartons[0].Width, shipping.Cartons[0].Length }
                };

                // Convert the object to json, and prepare request
                requestJson = JsonConvert.SerializeObject(apiTwoShipping);
                _config.TryGetValue(API_TWO_KEY, out apiKey);
                request = MakeHttpRequest(requestJson, "http://apiTwoShippingQuotes.com/GetQuote/", JSON_MEDIA_TYPE, apiKey);

                // Send Request
                //jsonResponse = await SendHttpRequest(request);

                if (shipping != null)
                {
                    amount = (shipping.Cartons[0].Height * shipping.Cartons[0].Width * shipping.Cartons[0].Length) * RandomShippingRate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ERROR_DETERMINE_SHIPPING_API_TWO + ex);
            }

            return amount;
        }

        private double? DeterminePriceAPIThree(Shipping shipping)
        {
            double? quote = -1;
            ApiThreeShipping apiThreeShipping = null;
            string requestXML = null;
            HttpRequestMessage request = null;
            XmlSerializer serializer = null;
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            string apiKey = null;

            try
            {
                // Convert our object to api three's format
                apiThreeShipping = new ApiThreeShipping
                {
                    Source = shipping.SourceAddress,
                    Destination = shipping.DestinationAddress,
                    Packages = { new Package { Height = shipping.Cartons[0].Height,
                                                   Width = shipping.Cartons[0].Width,
                                                   Length = shipping.Cartons[0].Length }
                        }
                };

                // Convert the object to xml, and prepare request
                serializer = new XmlSerializer(apiThreeShipping.GetType());
                serializer.Serialize(stringWriter, apiThreeShipping);
                requestXML = stringWriter.ToString();

                _config.TryGetValue(API_TWO_KEY, out apiKey);
                request = MakeHttpRequest(requestXML, "http://apiThreeShippingQuotes.com/GetQuote/", XML_MEDIA_TYPE, apiKey);

                // Send Request
                //tring xmlResponse = await SendHttpRequest(request);

                if (shipping != null)
                {
                    quote = (shipping.Cartons[0].Height * shipping.Cartons[0].Width * shipping.Cartons[0].Length) * RandomShippingRate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ERROR_DETERMINE_SHIPPING_API_THREE + ex);
            }

            return quote;
        }

        private double RandomShippingRate()
        {
            return new Random().NextDouble() * 10;
        }

        #endregion
    }
}
