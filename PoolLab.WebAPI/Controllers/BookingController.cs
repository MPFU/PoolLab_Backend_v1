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
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IBilliardTableService _billiardTableService;

        public BookingController(IBookingService bookingService, IBilliardTableService billiardTableService)
        {
            _bookingService = bookingService;
            _billiardTableService = billiardTableService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingByID(Guid id)
        {
            try
            {
                var book = await _bookingService.GetBookingById(id);
                if (book == null)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không tìm thấy lịch đặt nào!"
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Data = book
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
        public async Task<IActionResult> GetBookingRecurringByID(Guid id)
        {
            try
            {
                var book = await _bookingService.GetRecurringBookingById(id);
                if (book == null)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không tìm thấy lịch đặt nào!"
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Data = book
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
        public async Task<IActionResult> GetAllBooking([FromQuery] BookingFilter booking)
        {
            try
            {
                var book = await _bookingService.GetAllBooking(booking);
                if (book == null || book.Items.Count() <= 0)
                {
                    return NotFound(new FailResponse()
                    {
                        Status = NotFound().StatusCode,
                        Message = "Không tìm thấy lịch đặt nào !"
                    });
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Data = book
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
        public async Task<IActionResult> CreateNewBooking([FromBody] NewBookingDTO bookingDTO)
        {
            try
            {

                var requestResult = await _bookingService.AddNewBooking(bookingDTO);

                if (requestResult != null)
                {
                    if (Guid.TryParse(requestResult, out Guid result))
                    {
                        return Ok(new SucceededRespone()
                        {
                            Status = Ok().StatusCode,
                            Message = "Đặt bàn thành công.",
                            Data = await _bookingService.GetBookingById(Guid.Parse(requestResult))
                        });
                    }
                    else
                    {
                        return StatusCode(400, new FailResponse()
                        {
                            Status = 400,
                            Message = requestResult
                        });
                    }

                }
                return StatusCode(400, new FailResponse()
                {
                    Status = 400,
                    Message = "Đặt bàn thất bại!"
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

        [HttpPost]
        public async Task<IActionResult> CreateRecurringBooking([FromBody] BookingReqDTO bookingDTO)
        {
            try
            {

                var requestResult = await _bookingService.CreateBookingForMonth(bookingDTO);

                if (requestResult != null)
                {

                    return BadRequest(new FailResponse()
                    {
                        Status = BadRequest().StatusCode,
                        Message = requestResult
                    });

                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Đặt bàn định kì thành công!"
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
        public async Task<IActionResult> CancelBooking(Guid id, [FromBody] AnswerBookingDTO? answer = null)
        {
            try
            {

                var requestResult = await _bookingService.CancelBookingForMem(id, answer);
                if (requestResult != null)
                {
                    if (requestResult.Contains("."))
                    {
                        return Ok(new SucceededRespone()
                        {
                            Status = 202,
                            Message = requestResult
                        });
                    }
                    else
                    {
                        return StatusCode(400, new FailResponse()
                        {
                            Status = 400,
                            Message = requestResult
                        });
                    }
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Huỷ đặt lịch thành công."
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
        public async Task<IActionResult> CancelBookingRecurring(Guid id, [FromBody] AnswerBookingDTO answer)
        {
            try
            {

                var requestResult = await _bookingService.CancelBookingForMonth(id, answer);
                if (requestResult != null)
                {
                    if (requestResult.Contains("."))
                    {
                        return Ok(new SucceededRespone()
                        {
                            Status = 202,
                            Message = requestResult
                        });
                    }
                    else
                    {
                        return BadRequest(new FailResponse()
                        {
                            Status = BadRequest().StatusCode,
                            Message = requestResult
                        });
                    }
                }
                return Ok(new SucceededRespone()
                {
                    Status = Ok().StatusCode,
                    Message = "Huỷ đặt lịch định kì thành công."
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
        public async Task<IActionResult> UpdateStatusBooking(Guid id, [FromBody] UpdateBookingStatusDTO statusDTO)
        {
            try
            {

                var requestResult = await _bookingService.UpdateStatusBooking(id, statusDTO);
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
