using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public class PlaytimeService : IPlaytimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IPaymentService _paymentService;

        public PlaytimeService(IMapper mapper, IUnitOfWork unitOfWork, IAccountService accountService, IPaymentService paymentService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _paymentService = paymentService;
        }

        public async Task<string?> AddNewPlaytime(AddNewPlayTimeDTO addNewPlayTimeDTO)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

                var play = _mapper.Map<PlayTime>(addNewPlayTimeDTO);
                play.Id = Guid.NewGuid();
                play.Name = "Giờ chơi";
                play.TimeStart = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                await _unitOfWork.PlaytimeRepo.AddAsync(play);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo giờ chơi thất bại";
                }
                return play.Id.ToString();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> StopPlayTime(StopTimeDTO timeDTO)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                TimeSpan timeNow = now.TimeOfDay;

                var table = await _unitOfWork.BilliardTableRepo.GetBidaTableByID(timeDTO.BilliardTableID);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }

                var play = await _unitOfWork.PlaytimeRepo.GetPlayTimeByOrderOrTable(table.Id);
                if (play == null)
                {
                    return "Không tìm thấy phiên chơi này!";
                }

                if (timeDTO.CustomerID != null)
                {
                    var cus = await _unitOfWork.AccountRepo.GetByIdAsync((Guid)timeDTO.CustomerID);
                    if (cus == null)
                    {
                        return "Không tìm thấy tài khoản này!";
                    }

                    if (!string.IsNullOrEmpty(timeDTO.CustomerTime))
                    {
                        TimeSpan timeCus = TimeSpan.Parse(timeDTO.CustomerTime);

                        if (timeCus == TimeSpan.Zero)
                        {
                            var order = await _unitOfWork.OrderRepo.GetOrderByPlayTime(play.Id);
                            if (order == null)
                            {
                                return "Không tìm thấy hoá đơn này!";
                            }

                            play.Status = "Hoàn Thành";
                            _unitOfWork.PlaytimeRepo.Update(play);
                            var upPlay = await _unitOfWork.SaveAsync() > 0;
                            if (!upPlay)
                            {
                                return "Cập nhật phiên chơi thất bại";
                            }

                            order.TotalPrice = order.TotalPrice + play.TotalPrice;
                            order.Status = "Hoàn Thành";
                            _unitOfWork.OrderRepo.Update(order);
                            var upOrder = await _unitOfWork.SaveAsync() > 0;
                            if (!upOrder)
                            {
                                return "Cập nhật hoá đơn thất bại";
                            }

                            table.Status = "Bàn Trống";
                            _unitOfWork.BilliardTableRepo.Update(table);
                            var UpTable = await _unitOfWork.SaveAsync() > 0;
                            if (!UpTable)
                            {
                                return "Cập nhật bàn thất bại!";
                            }
                        }
                        else
                        {
                            var order = await _unitOfWork.OrderRepo.GetOrderByPlayTime(play.Id);
                            if (order == null)
                            {
                                return "Không tìm thấy hoá đơn này!";
                            }

                            decimal timeStop = (decimal)timeCus.TotalHours;
                            timeStop = Math.Round(timeStop,5);

                            var priceStop = table.Price.OldPrice * timeStop;
                            priceStop = Math.Round((decimal)priceStop,0,MidpointRounding.AwayFromZero);

                            play.Status = "Hoàn Thành";
                            play.TotalPrice = play.TotalPrice - priceStop;
                            play.TotalTime = play.TotalTime - timeStop;
                            _unitOfWork.PlaytimeRepo.Update(play);
                            var upPlay = await _unitOfWork.SaveAsync() > 0;
                            if (!upPlay)
                            {
                                return "Cập nhật phiên chơi thất bại";
                            }

                            order.TotalPrice = order.TotalPrice + play.TotalPrice;
                            order.Status = "Hoàn Thành";
                            _unitOfWork.OrderRepo.Update(order);
                            var upOrder = await _unitOfWork.SaveAsync() > 0;
                            if (!upOrder)
                            {
                                return "Cập nhật hoá đơn thất bại";
                            }

                            var refund = cus.Balance + priceStop;
                            var upCus = await _accountService.UpdateBalance(cus.Id, (decimal)refund);
                            if(upCus != null)
                            {
                                return upCus;
                            }

                            PaymentBookingDTO payment = new PaymentBookingDTO();
                            payment.AccountId = cus.Id;
                            payment.OrderId = order.Id;
                            payment.PaymentMethod = "Qua Ví";
                            payment.PaymentInfo = "Hoàn Tiền Dư";
                            payment.TypeCode = 1;
                            payment.Amount = priceStop;
                            var pay = await _paymentService.CreateTransactionBooking(payment);
                            if(pay != null)
                            {
                                return pay;
                            }

                            table.Status = "Bàn Trống";
                            _unitOfWork.BilliardTableRepo.Update(table);
                            var UpTable = await _unitOfWork.SaveAsync() > 0;
                            if (!UpTable)
                            {
                                return "Cập nhật bàn thất bại!";
                            }
                        }
                    }
                    else
                    {
                        var order = await _unitOfWork.OrderRepo.GetOrderByPlayTime(play.Id);
                        if (order == null)
                        {
                            return "Không tìm thấy hoá đơn này!";
                        }

                        play.Status = "Hoàn Thành";
                        _unitOfWork.PlaytimeRepo.Update(play);
                        var upPlay = await _unitOfWork.SaveAsync() > 0;
                        if (!upPlay)
                        {
                            return "Cập nhật phiên chơi thất bại";
                        }

                        order.TotalPrice = order.TotalPrice + play.TotalPrice;
                        order.Status = "Hoàn Thành";
                        _unitOfWork.OrderRepo.Update(order);
                        var upOrder = await _unitOfWork.SaveAsync() > 0;
                        if (!upOrder)
                        {
                            return "Cập nhật hoá đơn thất bại";
                        }

                        table.Status = "Bàn Trống";
                        _unitOfWork.BilliardTableRepo.Update(table);
                        var UpTable = await _unitOfWork.SaveAsync() > 0;
                        if (!UpTable)
                        {
                            return "Cập nhật bàn thất bại!";
                        }
                    }
                }
                else
                {
                    var order = await _unitOfWork.OrderRepo.GetOrderByPlayTime(play.Id);
                    if (order == null)
                    {
                        return "Không tìm thấy hoá đơn này!";
                    }

                    TimeSpan timeStart = play.TimeStart.Value.TimeOfDay;

                    TimeSpan timePlay = timeNow - timeStart;

                    decimal totalTime = (decimal)timePlay.TotalHours;

                    decimal totalPrice = (decimal)(totalTime * table.Price.OldPrice);
                    totalPrice = Math.Round(totalPrice, 0,MidpointRounding.AwayFromZero);

                    play.TotalPrice = totalPrice;
                    play.TotalTime = totalTime;
                    play.TimeEnd = now;
                    play.Status = "Hoàn Thành";
                    _unitOfWork.PlaytimeRepo.Update(play);
                    var upPlay = await _unitOfWork.SaveAsync() > 0;
                    if (!upPlay)
                    {
                        return "Cập nhật phiên chơi thất bại";
                    }

                    order.TotalPrice = order.TotalPrice + play.TotalPrice;
                    order.Status = "Hoàn Thành";
                    _unitOfWork.OrderRepo.Update(order);
                    var upOrder = await _unitOfWork.SaveAsync() > 0;
                    if (!upOrder)
                    {
                        return "Cập nhật hoá đơn thất bại";
                    }

                    table.Status = "Bàn Trống";
                    _unitOfWork.BilliardTableRepo.Update(table);
                    var UpTable = await _unitOfWork.SaveAsync() > 0;
                    if (!UpTable)
                    {
                        return "Cập nhật bàn thất bại!";
                    }
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            };
        }

        public decimal ConvertTimeOnlyToDecimal(TimeOnly time)
        {
            int hours = time.Hour;
            int minutes = time.Minute;
            int seconds = time.Second;

            return hours + (minutes / 60m) + (seconds / 3600m);
        }


    }
}
