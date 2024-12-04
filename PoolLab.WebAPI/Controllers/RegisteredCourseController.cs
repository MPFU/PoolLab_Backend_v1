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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegisteredCoursesById(Guid id)
        {
            try
            {
                var registeredCourseList = await _registeredCourseService.GetRegisterdCourseById(id);
                if (registeredCourseList == null)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> CancelRegisteredCourse(Guid id)
        {
            try
            {
                var update = await _registeredCourseService.CancelRegisteredCourse(id);
                if (update != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = update
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Huỷ khoá học thành công."
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
        public async Task<IActionResult> UpdateRegisteredCourse(Guid id, [FromBody] UpdateRegisteredCourseDTO updateRegisteredCourse)
        {
            try
            {
                var update = await _registeredCourseService.UpdateRegisteredCourse(id, updateRegisteredCourse);
                if (update != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = update
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Cập nhật đăng kí khoá học thành công."
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

        [HttpDelete]
        public async Task<IActionResult> DeleteRegisteredCourse(Guid id)
        {
            try
            {
                var check = await _registeredCourseService.DeleteRegisteredCourse(id);
                if (check != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = check
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Xoá đăng kí khoá học thành công."
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
