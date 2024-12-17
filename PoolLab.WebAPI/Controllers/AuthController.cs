using Azure.Core;
using FirebaseAdmin.Auth;
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
        private readonly IFirebaseAuthService _firebaseAuthService;

        public AuthController(IAuthService authService, IAccountService accountService, IFirebaseAuthService firebaseAuthService)
        {
            _accountService = accountService;
            _authService = authService;
            _firebaseAuthService = firebaseAuthService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginData)
        {
            try
            {
                var emailVerify = await _firebaseAuthService.VerifyEmailAsync(loginData.Email);

                if (!emailVerify)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = "Bạn chưa xác thực email!"
                    });
                }
                var loginRequest = await _authService.LoginAsync(loginData);
                if (loginRequest == null)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Sai email hoặc password (hoặc tài khoản của bạn chưa được kích hoạt)!"
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
                var loginRequest = await _authService.LoginStaffAsync(loginData);
                if (loginRequest == null)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Sai email hoặc mật khẩu (hoặc tài khoản của bạn chưa được kích hoạt)!"
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
                var userRecordArgs = new UserRecordArgs()
                {
                    Email = registerRequest.Email,
                    EmailVerified = false,
                    DisplayName = registerRequest.UserName
                };                

                var requestResult = await _authService.RegisterAsync(registerRequest);
                if (requestResult != null)
                {
                    return StatusCode(400, new FailResponse()
                    {
                        Status = 400,
                        Message = requestResult
                    });
                }

                var userRecord = await _firebaseAuthService.CreateAccFirebase(userRecordArgs);

                var emailVerificationLink = await _firebaseAuthService.SendEmailVerificationAsync(registerRequest.Email);

                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Tạo tài khoản thành công.",
                    Data = emailVerificationLink
                });

            }
            catch (FirebaseAuthException ex)
            {
                return Conflict(new FailResponse()
                {
                    Status = Conflict().StatusCode,
                    Message = "Đăng ký thất bại!",
                    Errors = ex.Message
                });
            }
            catch (Exception ex)
            {
                return Conflict(new FailResponse()
                {
                    Status = Conflict().StatusCode,
                    Message = "Đăng ký thất bại!",
                    Errors = ex.Message
                });
            }
        }

        [HttpPost("verify-phone-otp")]
        public async Task<IActionResult> VerifyPhoneOTP([FromBody] string idToken)
        {
            var result = await _firebaseAuthService.VerifyPhoneOTPAsync(idToken);
            return Ok(new { message = "OTP verified", result });
        }

        [HttpPost("send-email-verification")]
        public async Task<IActionResult> SendEmailVerification([FromBody] string email)
        {
            var link = await _firebaseAuthService.SendEmailVerificationAsync(email);
            return Ok(new SucceededRespone()
            {
                Message = "Email verification link sent",
                Data = link
            });
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] AccVerifyDTO accVerifyDTO)
        {
            var result = await _firebaseAuthService.VerifyEmailAsync(accVerifyDTO.Email);

            if (result)
            {
                UpdateAccStatusDTO updateAccStatusDTO = new UpdateAccStatusDTO();
                updateAccStatusDTO.Status = "Kích Hoạt";
                var upAcc = await _accountService.UpdateAccStatus(accVerifyDTO.Id, updateAccStatusDTO);
                if (upAcc != null)
                {
                    return BadRequest(new FailResponse
                    {
                        Status = BadRequest().StatusCode,
                        Message = upAcc,

                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Xác thực tài khoản thành công.",
                });

            }
            return BadRequest(new FailResponse
            {
                Status = BadRequest().StatusCode,
                Message = "Email vẫn chưa được xác thực!",

            });
        }
    }
}
