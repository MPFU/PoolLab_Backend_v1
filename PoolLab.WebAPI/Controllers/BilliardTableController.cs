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
    public class BilliardTableController : ControllerBase
    {
        private readonly IBilliardTableService _billiardTableService;
        private readonly IAzureBlobService _azureBlobService;
        private readonly IBidaTypeAreaService _bidaTypeAreaService;

        public BilliardTableController(IBilliardTableService billiardTableService, IAzureBlobService azureBlobService, IBidaTypeAreaService bidaTypeAreaService)
        {
            _billiardTableService = billiardTableService;
            _azureBlobService = azureBlobService;
            _bidaTypeAreaService = bidaTypeAreaService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBilliardTableByID(Guid id)
        {
            try
            {
                var roleList = await _billiardTableService.GetBilliardTableByID(id);
                if (roleList == null)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không tìm thấy bàn chơi này!"
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
        public async Task<IActionResult> GetBilliardTableByQRCode([FromQuery] GetByQRCode getByQR)
        {
            try
            {
                var table = await _billiardTableService.GetTableByQRCode(getByQR);
                if (table != null)
                {
                    if (table.Contains("|"))
                    {
                        string[] parts = table.Split('|');

                        return Ok(new SucceededRespone()
                        {
                            Status = 202,
                            Message = $"Bàn có lịch đặt lúc {parts[1]}.",
                            Data = new
                            {
                                BidaTable = await _billiardTableService.GetBilliardTableByID(getByQR.BilliardTableID),
                                TimeCus = parts[0],
                                BookingTime = parts[1]
                            }
                        });
                    }
                    else if (table.Contains("&")){
                        string[] parts = table.Split('&');

                        return Ok(new SucceededRespone()
                        {
                            Status = 202,
                            Message = $"Bàn có lịch đặt lúc {parts[1]}. \n Bạn chỉ có thể chơi {parts[0]} nếu kích hoạt!",
                            Data = new
                            {
                                BidaTable = await _billiardTableService.GetBilliardTableByID(getByQR.BilliardTableID),
                                TimeCus = parts[0],
                                BookingTime = parts[1]
                            }
                        });
                    }
                    else if (table.Contains(":"))
                    {
                        return Ok(new SucceededRespone()
                        {
                            Status = 200,
                            Message = "Thời gian chơi của người chơi.",
                            Data = new
                            {
                                BidaTable = await _billiardTableService.GetBilliardTableByID(getByQR.BilliardTableID),
                                TimeCus = table
                            }
                        });
                    }
                    else
                    {
                        return BadRequest(new FailResponse()
                        {
                            Status = BadRequest().StatusCode,
                            Message = table
                        });
                    }

                }
                else
                {
                    return Ok(new SucceededRespone()
                    {
                        Status = 203,
                        Message = "Người đặt bàn.",
                        Data = await _billiardTableService.GetBilliardTableByID(getByQR.BilliardTableID)
                    });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = "Thất Bại!",
                    Errors = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBilliardTable([FromQuery] BidaTableFilter accountFilter)
        {
            try
            {
                var accList = await _billiardTableService.GetAllBidaTable(accountFilter);
                if (accList == null || accList.Items.Count() <= 0)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không có bàn chơi nào !"
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Data = accList
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = "Thất Bại",
                    Errors = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBilliardTypeArea([FromQuery] BidaTypeAreFilter filter)
        {
            try
            {
                var bidaList = await _bidaTypeAreaService.GetAllBidaTypeAre(filter);
                if (bidaList == null || bidaList.Count() <= 0)
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
                    Data = bidaList
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
                    Message = "Tạo bàn thất bại.",
                    Errors = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileBidaTable(IFormFile file)
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
        public async Task<IActionResult> UpdateInfoTable(Guid id, [FromBody] UpdateInfoTableDTO tableDTO)
        {
            try
            {

                var requestResult = await _billiardTableService.UpdateInfoTable(id, tableDTO);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatusTable(Guid id, [FromBody] UpdateStatusTableDTO status)
        {
            try
            {
                if (string.IsNullOrEmpty(status.Status))
                {
                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = "Yêu cầu nhập trạng thái thay đổi!"
                    });
                }
                var requestResult = await _billiardTableService.UpdateStatusTable(id, status);
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

        [HttpPut]
        public async Task<IActionResult> ActivateTable([FromBody] ActiveTable tableDTO)
        {
            try
            {

                var requestResult = await _billiardTableService.ActivateTable(tableDTO);
                if (requestResult != null)
                {
                    if (requestResult.Contains(":") && !requestResult.Contains("!"))
                    {
                        return Ok(new SucceededRespone()
                        {
                            Status = Ok().StatusCode,
                            Message = "Kích hoạt bàn thành công.",
                            Data = requestResult
                        });
                    }
                    else
                    {
                        return BadRequest(new FailResponse()
                        {
                            Status = 400,
                            Message = requestResult
                        });
                    }

                    
                }
                return BadRequest(new FailResponse()
                {
                    Status = BadRequest().StatusCode,
                    Message = "Kích hoạt thất bại",
                    Errors = "Dữ liệu nhập vào bị sai!"
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

        [HttpPut("{id}")]
        public async Task<IActionResult> ActivateTableForGuest(Guid id, [FromBody] ActiveTableForGuest activeTableFor)
        {
            try
            {

                var requestResult = await _billiardTableService.ActivateTableForGuest(id, activeTableFor);
                if (requestResult != null)
                {
                    if (TimeOnly.TryParse(requestResult, out TimeOnly time))
                    {
                        return Ok(new SucceededRespone()
                        {
                            Status = Ok().StatusCode,
                            Message = $"Còn {requestResult} tới thời gian đặt bàn!",
                            Data = requestResult
                        });
                    }

                    return StatusCode(400, new FailResponse()
                    {
                        Status = 400,
                        Message = requestResult
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Kích hoạt thành công."
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(Guid id)
        {
            try
            {

                var requestResult = await _billiardTableService.DeleteTable(id);
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
                    Message = "Xoá thành công."
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
