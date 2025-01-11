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
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly IAzureBlobService _azureBlobService;

        public StoreController(IStoreService store, IAzureBlobService azureBlobService)
        {
            _storeService = store;
            _azureBlobService = azureBlobService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoreByID(Guid id)
        {
            try
            {
                var roleList = await _storeService.GetStoreById(id);
                if (roleList == null)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không tìm thấy cửa tiệm này trong danh sách!"
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
        public async Task<IActionResult> GetAllStore([FromQuery] StoreFilter storeFilter)
        {
            try
            {
                var area = await _storeService.GetAllStore(storeFilter);
                if (area == null || area.Items.Count() <= 0)
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
        public async Task<IActionResult> AddNewStore([FromBody] NewStoreDTO store)
        {
            try
            {
                var newStore = await _storeService.AddNewStore(store);
                if (newStore != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = newStore
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Tạo thành công"
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
        public async Task<IActionResult> UploadStoreImg(IFormFile file)
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
                var containerName = "store"; // replace with your container name
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
                    Message = "Tải ảnh thất bại!",
                    Errors =$"An error occurred while uploading the file: {ex.Message}"
                });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInfoStore(Guid id, [FromBody] NewStoreDTO newStoreDTO)
        {
            try
            {
                var newStore = await _storeService.UpdateStore(id, newStoreDTO);
                if (newStore != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = newStore
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
        public async Task<IActionResult> InActiveStore(Guid id)
        {
            try
            {
                var newStore = await _storeService.InActiveStore(id);
                if (newStore != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = newStore
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
        public async Task<IActionResult> ReActiveStore(Guid id)
        {
            try
            {
                var newStore = await _storeService.ReActiveStore(id);
                if (newStore != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = newStore
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(Guid id)
        {
            try
            {
                var newStore = await _storeService.DeleteStore(id);
                if (newStore != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = newStore
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
