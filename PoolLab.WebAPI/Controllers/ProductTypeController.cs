using Microsoft.AspNetCore.Mvc;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.WebAPI.ResponseModel;

namespace PoolLab.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductTypes()
        {
            try
            {
                var productType = await _productTypeService.GetAllProductTypes();
                if (productType == null || productType.Count() <= 0)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không có loại sản phẩm này."
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Data = productType
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
        public async Task<IActionResult> CreateProductType(CreateProductTypeDTO create)
        {
            try
            {
                var newType = await _productTypeService.CreateNewProductType(create);
                if(newType != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = newType
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status= Ok().StatusCode,
                    Message = "Tạo loại sản phẩm thành công."
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
