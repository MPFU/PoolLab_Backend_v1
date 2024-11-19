using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.WebAPI.ResponseModel;

namespace PoolLab.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PlaytimeController : ControllerBase
    {
        private readonly IPlaytimeService _playtimeService;

        public PlaytimeController(IPlaytimeService playtimeService)
        {
            _playtimeService = playtimeService;
        }

        [HttpPut]
        public async Task<IActionResult> StopPlayTime([FromBody] StopTimeDTO stopTimeDTO)
        {
            try
            {

                var requestResult = await _playtimeService.StopPlayTime(stopTimeDTO);
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
    }

}
