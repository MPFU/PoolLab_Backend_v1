using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using PoolLab.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public class PlaytimeService : IPlaytimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IPaymentService _paymentService;
        private readonly ISignalRNotifier _signalRNotifier;

        public PlaytimeService(IMapper mapper, IUnitOfWork unitOfWork, IAccountService accountService, IPaymentService paymentService, ISignalRNotifier signalRNotifier)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _paymentService = paymentService;
            _signalRNotifier = signalRNotifier;
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
                var timeNow = TimeOnly.FromDateTime(now);

                //Lấy thông tin Bàn
                var table = await _unitOfWork.BilliardTableRepo.GetBidaTableByID(timeDTO.BilliardTableID);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }

                //Lấy thông tin giờ chơi
                var play = await _unitOfWork.PlaytimeRepo.GetPlayTimeByOrderOrTable(table.Id);
                if (play == null)
                {
                    return "Không tìm thấy phiên chơi này!";
                }

                //Lấy thông tin hoá đơn
                var order = await _unitOfWork.OrderRepo.GetOrderByPlayTime(play.Id);
                if (order == null)
                {
                    return "Không tìm thấy hoá đơn này!";
                }

                //Đối với member
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

                        decimal timeStop = (decimal)timeCus.TotalHours;
                        timeStop = Math.Round(timeStop, 5);

                        //Khách chơi hết giờ
                        if (play.TotalTime == timeStop)
                        {                           

                            play.Status = "Hoàn Thành";
                            play.TimeEnd = now;
                            _unitOfWork.PlaytimeRepo.Update(play);
                            var upPlay = await _unitOfWork.SaveAsync() > 0;
                            if (!upPlay)
                            {
                                return "Cập nhật phiên chơi thất bại";
                            }

                            //Cộng điểm khuyến mãi, cập nhật tổng bill
                            if (order.Discount > 0)
                            {
                                var totalPrice = order.TotalPrice + play.TotalPrice;
                                var point = (int)Math.Round((decimal)(totalPrice / 1000));
                                cus.Point = cus.Point + point;
                                var upPoint = await _accountService.UpdateAccPoint(cus.Id, (int)cus.Point);
                                if(upPoint != null)
                                {
                                    return upPoint;
                                }
                                var change = (order.TotalPrice + play.TotalPrice) / 100 * order.Discount;

                                var refund = cus.Balance + change;
                                var upCus = await _accountService.UpdateBalance(cus.Id, (decimal)refund);
                                if (upCus != null)
                                {
                                    return upCus;
                                }

                                PaymentBookingDTO payment = new PaymentBookingDTO();
                                payment.AccountId = cus.Id;
                                payment.OrderId = order.Id;
                                payment.PaymentMethod = "Qua Ví";
                                payment.PaymentInfo = "Hoàn Tiền Khuyến Mãi";
                                payment.TypeCode = 1;
                                payment.Amount = change;
                                var pay = await _paymentService.CreateTransactionBooking(payment);
                                if (pay != null)
                                {
                                    return pay;
                                }

                                order.TotalPrice = Math.Round((decimal)totalPrice, 0, MidpointRounding.AwayFromZero);
                                order.CustomerPay = order.FinalPrice + play.TotalPrice;
                                order.Discount = change;
                                order.FinalPrice = (order.FinalPrice + play.TotalPrice) - change;
                                order.ExcessCash = change;
                            }
                            else
                            {
                                var totalPrice = order.TotalPrice + play.TotalPrice;
                                var point = (int)Math.Round((decimal)(totalPrice / 1000));
                                cus.Point = cus.Point + point;
                                var upPoint = await _accountService.UpdateAccPoint(cus.Id, (int)cus.Point);
                                if (upPoint != null)
                                {
                                    return upPoint;
                                }
                                order.TotalPrice = Math.Round((decimal)totalPrice, 0, MidpointRounding.AwayFromZero);                               
                                order.FinalPrice = order.FinalPrice + play.TotalPrice;
                                order.CustomerPay = order.FinalPrice;
                            }
                            
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

                            string message = $"{table.Name} đã dừng chơi.";

                            // Gửi thông báo qua SignalR
                            await _signalRNotifier.NotifyTableActivationAsync((Guid)table.StoreId, message);
                        }
                        // Khách còn dư giờ
                        else
                        {                                                    
                            var priceStop = table.Price.OldPrice * timeStop;
                            priceStop = Math.Round((decimal)priceStop,0,MidpointRounding.AwayFromZero);

                            var principal = play.TotalPrice;

                            play.Status = "Hoàn Thành";
                            play.TotalPrice = priceStop; 
                            play.TotalTime = timeStop;
                            play.TimeEnd = now;
                            _unitOfWork.PlaytimeRepo.Update(play);
                            var upPlay = await _unitOfWork.SaveAsync() > 0;
                            if (!upPlay)
                            {
                                return "Cập nhật phiên chơi thất bại";
                            }

                            //Cộng điểm khuyến mãi, cập nhật tổng bill
                            if (order.Discount > 0)
                            {
                                var totalPrice = order.TotalPrice + priceStop;
                                var point = (int)Math.Round((decimal)(totalPrice / 1000));
                                cus.Point = cus.Point + point;
                                var upPoint = await _accountService.UpdateAccPoint(cus.Id, (int)cus.Point);
                                if (upPoint != null)
                                {
                                    return upPoint;
                                }

                                var change = (order.TotalPrice + priceStop) / 100 * order.Discount;

                                var refund1 = cus.Balance + change;

                                var upCus1 = await _accountService.UpdateBalance(cus.Id, (decimal)refund1);
                                if (upCus1 != null)
                                {
                                    return upCus1;
                                }
                                order.TotalPrice = Math.Round((decimal)totalPrice, 0, MidpointRounding.AwayFromZero);
                                order.CustomerPay = order.FinalPrice + principal;
                                order.FinalPrice = (order.FinalPrice + priceStop) - change;                               
                                order.Discount = change;
                                order.ExcessCash = change + (principal - priceStop);
                            }
                            else
                            {
                                var totalPrice = order.TotalPrice + priceStop;
                                var point = (int)Math.Round((decimal)(totalPrice / 1000));
                                cus.Point = cus.Point + point;
                                var upPoint = await _accountService.UpdateAccPoint(cus.Id, (int)cus.Point);
                                if (upPoint != null)
                                {
                                    return upPoint;
                                }
                                order.CustomerPay = order.FinalPrice + principal;
                                order.TotalPrice = Math.Round((decimal)totalPrice, 0, MidpointRounding.AwayFromZero);
                                order.FinalPrice = order.FinalPrice + priceStop;                              
                                order.ExcessCash += (principal - priceStop);
                            }

                            order.Status = "Hoàn Thành";
                            _unitOfWork.OrderRepo.Update(order);
                            var upOrder = await _unitOfWork.SaveAsync() > 0;
                            if (!upOrder)
                            {
                                return "Cập nhật hoá đơn thất bại";
                            }

                            var refund = cus.Balance + (principal - priceStop);
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
                            payment.Amount = order.ExcessCash ;
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

                            string message = $"{table.Name} đã dừng chơi.";

                            // Gửi thông báo qua SignalR
                            await _signalRNotifier.NotifyTableActivationAsync((Guid)table.StoreId, message);
                        }
                    }
                }
                //Đối vơi khách vãng lai
                else
                {                   
                    if(order.CustomerId != null)
                    {
                        return "Đây là bàn chơi của member bạn không thể dừng chơi bàn này!";
                    }

                    var timeStart = TimeOnly.FromDateTime((DateTime)play.TimeStart);

                    TimeSpan timePlay = timeNow - timeStart;

                    TimeSpan timeCus = new TimeSpan(timePlay.Hours, timePlay.Minutes, timePlay.Seconds);

                    if ((int)timePlay.TotalHours == 0 && timePlay.Minutes < 30)
                    {
                        timeCus = new TimeSpan(0, 30, 0);
                    }
                    else if (timePlay.Minutes > 30 && timePlay.Minutes <= 59)
                    {
                        timeCus = new TimeSpan((int)timePlay.TotalHours, 30, 0);
                    }
                    else
                    {
                        timeCus = new TimeSpan((int)timePlay.TotalHours, 0, 0);
                    }

                    decimal totalTime = Math.Abs((decimal)timeCus.TotalHours);

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
                    order.FinalPrice = order.TotalPrice + play.TotalPrice;
                    order.Status = "Đang Thanh Toán";
                    _unitOfWork.OrderRepo.Update(order);
                    var upOrder = await _unitOfWork.SaveAsync() > 0;
                    if (!upOrder)
                    {
                        return "Cập nhật hoá đơn thất bại";
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            };
        }

        public decimal ConvertTimeOnlyToDecimal(TimeOnly time)
        {
            int hours = time.Hour;
            int minutes = time.Minute;
            int seconds = time.Second;

            return hours + (minutes / 60m) + (seconds / 3600m);
        }

        public async Task<PlaytimeDTO?> GetPlaytimeByTableIdOrId(Guid id)
        {
            return _mapper.Map<PlaytimeDTO?> (await _unitOfWork.PlaytimeRepo.GetPlayTimeByOrderOrTable(id));
        }
    }
}
