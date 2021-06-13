using Microsoft.AspNetCore.Mvc;
using System;
using TaylorFerens_Cymax_Application.Interfaces;
using TaylorFerens_Cymax_Application.Models;

namespace TaylorFerens_Cymax_Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShippingController : ControllerBase
    {

        #region Constants

        private const string GET_ERROR_MESSAGE = "ERROR: ShippingController.Get(): Error occured in Get controller for shipping";

        #endregion
        #region Instance Variables

        private readonly IShippingService _service;

        #endregion
        #region Public Methods

        public ShippingController(IShippingService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<string> Get([FromQuery] Shipping shipping)
        {
            ActionResult retval = BadRequest(shipping);

            try
            {
                if (!ModelState.IsValid)
                {
                    retval = BadRequest(shipping);
                }
                else
                {
                    retval = new OkObjectResult(_service.DetermineBestShipping(shipping));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(GET_ERROR_MESSAGE + ex);
            }

            return retval;
        }

        #endregion
    }
}
