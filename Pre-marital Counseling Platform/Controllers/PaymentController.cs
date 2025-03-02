using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SWP391.DTO.VnPay;
using SWP391.Service;
using System.Security.Claims;

namespace SWP391.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpPost("Get_Payment_Url")]
        public async Task<IActionResult> GetVNPayUrl([FromBody] VnPayRequestDTO vnPayRequestDTO)
        {
            var currentUser = HttpContext.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var url = HttpContext.Request.GetDisplayUrl();

            return await _paymentService.HandleCreateVNPayUrl(HttpContext, vnPayRequestDTO, currentUserId, url);
        }

        [AllowAnonymous]
        [HttpPost("Process_Payment")]
        public async Task<IActionResult> ProcessVNPay([FromBody] VnPayProcessDTO vnPayProcessDTO)
        {
            return await _paymentService.HandleVNPay(vnPayProcessDTO);
        }
    }
}
