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
    public class AccountVoucherController : ControllerBase
    {
        private readonly IAccountVoucherService _accountVoucherService;

        public AccountVoucherController(IAccountVoucherService accountVoucherService)
        {
            _accountVoucherService = accountVoucherService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllAccountVoucherByVouOrAccID(Guid id)
        {
            try
            {
                var accList = await _accountVoucherService.GetAllAccountVoucherByVouOrCusID(id);
                if (accList == null)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không có khuyến mãi nào !"
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Data = accList
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
        public async Task<IActionResult> GetAllAccountVoucher([FromQuery] AccountVoucherFilter accountFilter)
        {
            try
            {
                var accList = await _accountVoucherService.GetAllAccountVoucher(accountFilter);
                if (accList == null || accList.Items.Count() <= 0)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không có khuyến mãi nào !"
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Data = accList
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
        public async Task<IActionResult> CreateNewAccountVoucher([FromBody] AddNewAccountVoucherDTO accDTO)
        {
            try
            {

                var requestResult = await _accountVoucherService.AddNewAccountVoucher(accDTO);
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
                    Message = "Đổi khuyến mãi thành công."
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
        public async Task<IActionResult> UseAccountVoucher(Guid id)
        {
            try
            {

                var requestResult = await _accountVoucherService.UseAccountVoucher(id);
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
                    Message = "Sử dụng khuyến mãi thành công."
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
