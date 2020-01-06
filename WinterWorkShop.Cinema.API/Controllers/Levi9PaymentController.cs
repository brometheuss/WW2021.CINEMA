using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Services;

namespace WinterWorkShop.Cinema.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class Levi9PaymentController : ControllerBase
    {
        private readonly ILevi9PaymentService _paymentService;

        public Levi9PaymentController(ILevi9PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<PaymentResponseModel>> Post()
        {
            var result = await _paymentService.MakePayment();

            PaymentResponseModel model = new PaymentResponseModel
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message
            };

            if (!model.IsSuccess)
            {
                return BadRequest(model);
            }

            return Ok(model);
        }
    }
}