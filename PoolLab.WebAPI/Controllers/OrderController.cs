using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoolLab.Application.FilterModel;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Application.Services;
using PoolLab.WebAPI.ResponseModel;

namespace PoolLab.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Member, Admin,Super Manager,Manager,Staff")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrder([FromQuery] OrderFilter filter)
        {
            try
            {
                var orderList = await _orderService.GetAllOrders(filter);
                if (orderList == null || orderList.Items.Count() <= 0)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không tìm thấy !"
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Data = orderList
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            try
            {
                var order = await _orderService.GetOrderBillForPay(id);
                if (order == null)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không tìm thấy !"
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Data = order
                }) ;
            }
            catch (Exception ex)
            {
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderBillForPay([FromBody] CreateOrderBill orderBill)
        {
            try
            {

                var requestResult = await _orderService.CreateOrderForPay(orderBill);
                if (requestResult != null)
                {
                    if(Guid.TryParse(requestResult, out _))
                    {
                        return Ok(new SucceededRespone()
                        {
                            Status = Ok().StatusCode,
                            Message = "Tạo Hoá Đơn thành công.",
                            Data = await _orderService.GetOrderBillForPay(Guid.Parse(requestResult))
                        });
                    }
                    else
                    {
                        return StatusCode(400, new FailResponse()
                        {
                            Status = 400,
                            Message = requestResult
                        });
                    }
                   
                }
                return StatusCode(400, new FailResponse()
                {
                    Status = 400,
                    Message = "Dữ liệu nhập vào bị sai!"
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCusPayOrder(Guid id, [FromBody] UpdateCusPayDTO orderDTO)
        {
            try
            {

                var requestResult = await _orderService.UpdateCusPayOrder(id, orderDTO);
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
                    Message = "Cập nhật thành công."
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
    }
}
