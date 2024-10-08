using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoolLab.Application.FilterModel;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.WebAPI.ResponseModel;

namespace PoolLab.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BilliardPriceController : ControllerBase
    {
        private readonly IBilliardPriceService _billiardPriceService;

        public BilliardPriceController(IBilliardPriceService billiardPriceService)
        {
            _billiardPriceService = billiardPriceService;
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetBilliardPriceByID(Guid id)
        {
            try
            {
                var area = await _billiardPriceService.GetBidaPriceById(id);
                if (area == null )
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
                    Data = area
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

        [HttpGet]
        public async Task<IActionResult> GetAllBilliardPrice([FromQuery]BidaPriceFilter bidaPriceFilter)
        {
            try
            {
                var area = await _billiardPriceService.GetAllBidaPrice(bidaPriceFilter);
                if (area == null || area.Count() <= 0)
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
                    Data = area
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

        [HttpPost]
        public async Task<IActionResult> AddNewBilliardPrice([FromBody] NewBilliardPriceDTO areaDTO)
        {
            try
            {
                var newRole = await _billiardPriceService.AddNewBidaPrice(areaDTO);
                if (newRole != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = newRole
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Tạo mới thành công."
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

        [HttpPut("id")]
        public async Task<IActionResult> UpdateBilliardPrice(Guid id, [FromBody] NewBilliardPriceDTO areaDTO)
        {
            try
            {
                var newRole = await _billiardPriceService.UpdateBilliardPrice(id, areaDTO);
                if (newRole != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = newRole
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
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateBilliardPriceStatus(Guid id, [FromQuery] string status)
        {
            try
            {
                var newRole = await _billiardPriceService.UpdateBilliardPriceStatus(id, status);
                if (newRole != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = newRole
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
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = ex.Message
                });
            }
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteBilliardPrice(Guid id)
        {
            try
            {
                var newRole = await _billiardPriceService.DeleteBidaPrice(id);
                if (newRole != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = newRole
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Xoá thành công."
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


    }
}
