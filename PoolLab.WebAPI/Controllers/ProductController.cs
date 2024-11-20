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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IAzureBlobService _azureBlobService;

        public ProductController(IProductService productService, IAzureBlobService azureBlobService)
        {
            _productService = productService;
            _azureBlobService = azureBlobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductFilter productFilter)
        {
            try
            {
                var productList = await _productService.GetAllProducts(productFilter);
                if (productList == null || productList.Items.Count() <= 0)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không có sản phẩm nào."
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Data = productList
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
        public async Task<IActionResult> CreateProduct(CreateProductDTO create)
        {
            try
            {
                var newType = await _productService.CreateProduct(create);
                if (newType != null)
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = newType
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Tạo sản phẩm thành công."
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
        public async Task<IActionResult> UploadFileProductImg(IFormFile file)
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
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductInfo createProductDTO)
        {
            try
            {
                var update = await _productService.UpdateProduct(id, createProductDTO);
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
                    Message = "Cập nhật sản phẩm thành công."
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
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var check = await _productService.DeleteProduct(id);
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
                    Message = "Xoá sản phẩm thành công."
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByID(Guid id)
        {
            try
            {
                var check = await _productService.SearchProductById(id);
                if (check == null)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không tìm thấy sản phẩm này."
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Data = check
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
