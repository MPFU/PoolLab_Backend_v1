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
    public class BilliardTableController : ControllerBase
    {
        private readonly IBilliardTableService _billiardTableService;
        private readonly IAzureBlobService _azureBlobService;

        public BilliardTableController(IBilliardTableService billiardTableService, IAzureBlobService azureBlobService)
        {
            _billiardTableService = billiardTableService;
            _azureBlobService = azureBlobService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBilliardTableByID(Guid id)
        {
            try
            {
                var roleList = await _billiardTableService.GetBilliardTableByID(id);
                if (roleList == null)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không tìm thấy bàn chơi này!"
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
        public async Task<IActionResult> GetAllBilliardTable([FromQuery] BidaTableFilter accountFilter)
        {
            try
            {
                var accList = await _billiardTableService.GetAllBidaTable(accountFilter);
                if (accList == null || accList.Items.Count() <= 0)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không có bàn chơi nào !"
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
        public async Task<IActionResult> CreateNewTable([FromBody] NewBilliardTableDTO accDTO)
        {
            try
            {

                var requestResult = await _billiardTableService.AddNewTable(accDTO);
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
                    Message = "Tạo bàn chơi mới thành công."
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
        public async Task<IActionResult> UploadFileBidaTable(IFormFile file)
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
                var containerName = "product"; // replace with your container name
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInfoTable(Guid id, [FromBody] UpdateInfoTableDTO tableDTO)
        {
            try
            {

                var requestResult = await _billiardTableService.UpdateInfoTable(id, tableDTO);
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateStatusTable(Guid id, [FromBody] string status)
        {
            try
            {
                if (string.IsNullOrEmpty(status))
                {
                    return BadRequest(new FailResponse ()
                    {
                        Status = BadRequest().StatusCode,
                        Message = "Yêu cầu nhập trạng thái thay đổi!"
                    });
                }
                var requestResult = await _billiardTableService.UpdateStatusTable(id, status);
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(Guid id)
        {
            try
            {

                var requestResult = await _billiardTableService.DeleteTable(id);
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
                    Message = "Xoá thành công."
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
