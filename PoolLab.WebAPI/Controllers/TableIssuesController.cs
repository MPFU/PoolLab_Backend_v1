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
    public class TableIssuesController : ControllerBase
    {
        private readonly ITableIssuesService _tableIssuesService;

        public TableIssuesController(ITableIssuesService tableIssuesService)
        {
            _tableIssuesService = tableIssuesService;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTableIssuesById(Guid id)
        {
            try
            {
                var trans = await _tableIssuesService.GetTableIssuesById(id);
                if (trans == null)
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

        [HttpGet]
        public async Task<IActionResult> GetAllTableIssues([FromQuery] TableIssuesFilter filter)
        {
            try
            {
                var trans = await _tableIssuesService.GetAllTableIssues(filter);
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

        [HttpPost]
        public async Task<IActionResult> CreateTableIssues([FromBody] CreateTableIssuesDTO createDTO)
        {
            try
            {
                var create = await _tableIssuesService.CreateNewTableIssue(createDTO);
                if (create != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = create
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Tạo báo cáo hư hỏng thành công."
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
