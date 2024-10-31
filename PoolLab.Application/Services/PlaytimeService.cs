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

        public PlaytimeService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string?> AddNewPlaytime(AddNewPlayTimeDTO addNewPlayTimeDTO)
        {
            try
            {
                var play = _mapper.Map<PlayTime>(addNewPlayTimeDTO);
                play.Id = Guid.NewGuid();
                play.Name = "Giờ chơi";
                var bidaPrice = await _unitOfWork.BilliardTableRepo.GetPriceOfTable(play.BilliardTableId);
                if (bidaPrice == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }
                var timePlay = ConvertTimeOnlyToDecimal((TimeOnly)play.TimeTotal);
                play.TotalPrice = timePlay * bidaPrice;
                await _unitOfWork.PlaytimeRepo.AddAsync(play);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo giờ chơi thất bại";
                }
                var order = await _unitOfWork.OrderRepo.GetByIdAsync((Guid)play.OrderId);
                if (order == null)
                {
                    return "Không tìm thấy hoá đơn của giờ chơi này!";
                }
                order.TotalPrice += play.TotalPrice;
                order.Status = "Chờ Thanh Toán";
                _unitOfWork.OrderRepo.Update(order);
                var result1 = await _unitOfWork.SaveAsync() > 0;
                if (!result1)
                {
                    return "Cập nhật hoá đơn trên giờ chơi thất bại";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
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
