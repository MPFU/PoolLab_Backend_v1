using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.WebAPI.ResponseModel;

namespace PoolLab.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncomeOfStore(Guid id)
        {
            try
            {
                var request = await _service.TotalIncomeInStore(id);
                if (decimal.TryParse(request, out _))
                {
                    return Ok(new SucceededRespone()
                    {
                        Status = Ok().StatusCode,
                        Message = "Thành công.",
                        Data = request
                    });
                }
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = request
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
        public async Task<IActionResult> GetAllOrderOfStore(Guid id)
        {
            try
            {
                var request = await _service.TotalOrderInStore(id);
                if (decimal.TryParse(request, out _))
                {
                    return Ok(new SucceededRespone()
                    {
                        Status = Ok().StatusCode,
                        Message = "Thành công.",
                        Data = request
                    });
                }
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = request
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
        public async Task<IActionResult> GetAllStaffOfStore(Guid id)
        {
            try
            {
                var request = await _service.TotalStaffInStore(id);
                if (decimal.TryParse(request, out _))
                {
                    return Ok(new SucceededRespone()
                    {
                        Status = Ok().StatusCode,
                        Message = "Thành công.",
                        Data = request
                    });
                }
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = request
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
        public async Task<IActionResult> GetAllBookingOfStore(Guid id)
        {
            try
            {
                var request = await _service.TotalBookingInStore(id);
                if (decimal.TryParse(request, out _))
                {
                    return Ok(new SucceededRespone()
                    {
                        Status = Ok().StatusCode,
                        Message = "Thành công.",
                        Data = request
                    });
                }
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = request
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
        public async Task<IActionResult> GetAllMemberOfStore(Guid id)
        {
            try
            {
                var request = await _service.TotalMemberInStore(id);
                if (decimal.TryParse(request, out _))
                {
                    return Ok(new SucceededRespone()
                    {
                        Status = Ok().StatusCode,
                        Message = "Thành công.",
                        Data = request
                    });
                }
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = request
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
        public async Task<IActionResult> GetAllReviewOfStore(Guid id)
        {
            try
            {
                var request = await _service.TotalReviewInStore(id);
                if (decimal.TryParse(request, out _))
                {
                    return Ok(new SucceededRespone()
                    {
                        Status = Ok().StatusCode,
                        Message = "Thành công.",
                        Data = request
                    });
                }
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = request
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
        public async Task<IActionResult> GetTopSellingProductByStore(Guid id)
        {
            try
            {
                var request = await _service.GetTopSellingProductByStore(id);
                if (request != null)
                {
                    return Ok(new SucceededRespone()
                    {
                        Status = Ok().StatusCode,
                        Message = "Thành công.",
                        Data = request
                    });
                }
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = "Chưa có thống kê!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = "Không thống kê được sản phẩm bán chạy của chi nhánh này!",
                    Errors = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetIncomeOfStoreByFilter([FromQuery] IncomeStoreDTO incomeStoreDTO)
        {
            try
            {
                if((incomeStoreDTO.StoreId == null || incomeStoreDTO.StoreId == Guid.Empty) || incomeStoreDTO.year == 0)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = "Cần nhập đầy đủ chi nhánh và năm thống kê!"
                    });
                }
                var request = await _service.GetInComeOfStoreByFilter(incomeStoreDTO.StoreId,incomeStoreDTO.year,incomeStoreDTO.month);
                if (request != null)
                {
                    return Ok(new SucceededRespone()
                    {
                        Status = Ok().StatusCode,
                        Message = "Thành công.",
                        Data = request
                    });
                }
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = "Không thể thống kê chi nhánh này!"
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
