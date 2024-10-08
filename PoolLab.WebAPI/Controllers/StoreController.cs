using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.WebAPI.ResponseModel;

namespace PoolLab.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService store)
        {
            _storeService = store;
        }

        [HttpGet("id")]
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
        public async Task<IActionResult> GetAllStore()
        {
            try
            {
                var area = await _storeService.GetAllStore();
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

        [HttpPut("id")]
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

        [HttpDelete("id")]
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
