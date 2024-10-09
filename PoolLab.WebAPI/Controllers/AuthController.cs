using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.WebAPI.ResponseModel;

namespace PoolLab.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, IAccountService accountService)
        {
            _accountService = accountService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginData)
        {
            try
            {
                var loginRequest = await _authService.LoginAsync(loginData);
                if (loginRequest == null)
                {
                    return Ok(new SucceededRespone()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Sai email hoặc password!"
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Đăng nhập thành công.",
                    Data = loginRequest
                });
            }
            catch (NullReferenceException ex)
            {
                return NotFound(new FailResponse()
                {
                    Status = NotFound().StatusCode,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginStaff(LoginAccDTO loginData)
        {
            try
            {
                if (loginData.StoreId != Guid.Empty && loginData.CompanyId != Guid.Empty)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = "Chỉ được phép nhập 1 trong 2 (store hoặc company)!"
                    });
                }
                var loginRequest = await _authService.LoginStaffAsync(loginData);
                if (loginRequest == null)
                {
                    return Ok(new SucceededRespone()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Sai email hoặc password!"
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Đăng nhập thành công.",
                    Data = loginRequest
                });
            }
            catch (NullReferenceException ex)
            {
                return NotFound(new FailResponse()
                {
                    Status = NotFound().StatusCode,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerRequest)
        {
            try
            {
               
                var requestResult = await _authService.RegisterAsync(registerRequest);
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
