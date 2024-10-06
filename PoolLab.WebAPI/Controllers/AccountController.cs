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
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetAccountByID(Guid id)
        {
            try
            {
                var roleList = await _accountService.GetAccountById(id);
                if (roleList == null)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không tìm thấy tài khoản này!"
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Data = roleList
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
        public async Task<IActionResult> GetAllAccount([FromQuery] AccountFilter accountFilter)
        {
            try
            {
                var accList = await _accountService.GetAllAccount(accountFilter);
                if (accList == null)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không có tài khoản nào !"
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

        [HttpPut("id")]
        public async Task<IActionResult> UpdateInfoUser(Guid id, [FromBody] UpdateAccDTO updateAcc)
        {
            try
            {
                var acc = await _accountService.UpdateAccountInfo(id, updateAcc);
                if (acc != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = acc
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Cập nhật thành công!"
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
        public async Task<IActionResult> UpdatePassword(Guid id, [FromBody] UpdatePassDTO updatePass)
        {
            try
            {
                var acc = await _accountService.UpdatePassword(id, updatePass);
                if (acc != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = acc
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Cập nhật mật khẩu thành công!"
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
        public async Task<IActionResult> CreateNewAccount([FromBody] CreateAccDTO accDTO)
        {
            try
            {

                var requestResult = await _accountService.CreateAccount(accDTO);
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
                    Message = "Tạo tài khoản thành công."
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
