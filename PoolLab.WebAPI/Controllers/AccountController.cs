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
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAzureBlobService _azureBlobService;

        public AccountController(IAccountService accountService, IAzureBlobService azureBlobService)
        {
            _accountService = accountService;
            _azureBlobService = azureBlobService;
        }

        [HttpGet("{id}")]
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
                if (accList == null || accList.Items.Count() <= 0)
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

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Member,Manager,Super Manager")]
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

        [HttpPut("{id}")]
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

        [HttpPost]
        public async Task<IActionResult> UploadFileAvatar(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = "Tệp không được chọn hoặc trống."
                    });

                var imageExtensions = new[] { ".jpg", ".png" };
                if (imageExtensions.Any(e => file.FileName.EndsWith(e, StringComparison.OrdinalIgnoreCase)) == false)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = "Tệp không phải là hình ảnh."
                    });
                }
                var containerName = "avatar"; // replace with your container name
                var uri = await _azureBlobService.UploadFileImageAsync(containerName, file);

                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Tệp được tải lên thành công.",
                    Data = uri
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new FailResponse()
                {
                    Status = 500,
                    Message = $"An error occurred while uploading the file: {ex.Message}"
                });
            }
        }
    }
}
