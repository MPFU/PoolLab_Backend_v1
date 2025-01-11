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
    public class TableMaintenanceController : ControllerBase
    {
        private readonly ITableMaintenanceService _tableMaintenanceService;

        public TableMaintenanceController(ITableMaintenanceService tableMaintenanceService)
        {
            _tableMaintenanceService = tableMaintenanceService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTableMaintenanceByIssues([FromBody] CreateTableMainDTO tableMainDTO)
        {
            try
            {

                var requestResult = await _tableMaintenanceService.CreateTableMaintenanceByTableIssues(tableMainDTO);
                if (requestResult != null)
                {

                    return BadRequest(new FailResponse()
                    {
                        Status = 400,
                        Message = requestResult
                    });


                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Tạo mới thành công."
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

        [HttpGet]
        public async Task<IActionResult> GetAllTableMaintenance([FromQuery] TableMainFilter filter)
        {
            try
            {
                var trans = await _tableMaintenanceService.GetAllTableMain(filter);
                if (trans == null || trans.Items.Count() <= 0)
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
                    Data = trans
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
