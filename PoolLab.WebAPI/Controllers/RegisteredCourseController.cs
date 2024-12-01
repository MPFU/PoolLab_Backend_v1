using Microsoft.AspNetCore.Mvc;
using PoolLab.Application.FilterModel;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.WebAPI.ResponseModel;

namespace PoolLab.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegisteredCourseController : Controller
    {

        private readonly IRegisterCourseService _registeredCourseService;

        public RegisteredCourseController(IRegisterCourseService registeredCourseService)
        {
            _registeredCourseService = registeredCourseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegisteredCourses([FromQuery] RegisteredCourseFilter registeredCourseFilter)
        {
            try
            {
                var registeredCourseList = await _registeredCourseService.GetAllRegisteredCourses(registeredCourseFilter);
                if (registeredCourseList == null || registeredCourseList.Items.Count() <= 0)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không có đăng kí khoá học nào."
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Data = registeredCourseList
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
        public async Task<IActionResult> CreateRegisteredCourse(CreateRegisteredCourseDTO create)
        {
            try
            {
                var newRegisteredCourse = await _registeredCourseService.CreateRegisteredCourse(create);
                if (newRegisteredCourse != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = newRegisteredCourse
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Tạo đăng kí khoá học thành công."
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
