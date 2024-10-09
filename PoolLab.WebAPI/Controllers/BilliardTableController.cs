using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.WebAPI.ResponseModel;

namespace PoolLab.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BilliardTableController : ControllerBase
    {
        private readonly IBilliardTableService _billiardTableService;

        public BilliardTableController(IBilliardTableService billiardTableService)
        {
            _billiardTableService = billiardTableService;
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
    }
}
