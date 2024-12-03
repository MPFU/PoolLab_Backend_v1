using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Application.Services;
using PoolLab.WebAPI.ResponseModel;

namespace PoolLab.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VNPayController : ControllerBase
    {
        private readonly IVNPayService _vNPayService;

        public VNPayController(IVNPayService vNPayService)
        {
            _vNPayService = vNPayService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] CreateVNPayDTO request)
        {
            var paymentUrl = await _vNPayService.CreatePaymentUrl(request, HttpContext);
            return Ok(new SucceededRespone()
            {
                Status = Ok().StatusCode,
                Data = paymentUrl
            });
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCallbackVnpay([FromQuery] VNPayReturnQuery vNPayReturn)
        {
            var response = await _vNPayService.PaymentExecute(vNPayReturn);

            if (response.Success == false)
            {
                if(response.VnPayResponseCode == "24")
                {
                    return BadRequest(new FailResponse
                    {
                        Status = BadRequest().StatusCode,
                        Message = "Giao dịch đã bị huỷ!"
                    });
                }
                return BadRequest(new FailResponse
                {
                    Status = BadRequest().StatusCode,
                    Message = "Nạp tiền thất bại!"
                });
            }
            return Ok(new SucceededRespone{
                Status = Ok().StatusCode,
                Message= "Nạp tiền thành công"
            });
        }

    }
}
