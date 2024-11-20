using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.WebAPI.ResponseModel;

namespace PoolLab.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewOrderDetail([FromBody] AddNewOrderDetailDTO orderDetailDTO)
        {
            try
            {

                var requestResult = await _orderDetailService.AddNewOrderDetail(orderDetailDTO);
                if (requestResult != null)
                {
                    return StatusCode(400, new FailResponse()
                    {
                        Status = 400,
                        Message = requestResult
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Tạo đơn hàng sản phẩm thành công."
                });

            }
            catch (Exception ex)
            {
                return Conflict(new FailResponse()
                {
                    Status = Conflict().StatusCode,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddNewProductToOrder(Guid id,[FromBody] List<AddOrderDetailDTO> orderDetailDTO)
        {
            try
            {

                var requestResult = await _orderDetailService.AddNewProductToOrder(id,orderDetailDTO);
                if (requestResult != null)
                {
                    return (Decimal.TryParse(requestResult, out _) )
                        ?
                        Ok(new SucceededRespone()
                        {
                            Status = Ok().StatusCode,
                            Message = "Đặt sản phẩm thành công.",
                            Data = Decimal.Parse(requestResult)
                        })
                        :
                        StatusCode(400, new FailResponse()
                        {
                            Status = 400,
                            Message = requestResult
                        })
                        ;

                }
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = "Mã bàn hoặc danh sách các sản phẩm bị lỗi!"
                }) ;

            }
            catch (Exception ex)
            {
                return Conflict(new FailResponse()
                {
                    Status = Conflict().StatusCode,
                    Message = ex.Message
                });
            }
        }
    }
}
