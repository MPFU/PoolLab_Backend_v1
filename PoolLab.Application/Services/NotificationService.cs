using AutoMapper;
using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string?> CreateNewNotification(CreateNotificationDTO notificationDTO)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

                var noti = _mapper.Map<Notification>(notificationDTO);
                noti.Id = Guid.NewGuid();
                noti.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);

                await _unitOfWork.NotificationRepo.AddAsync(noti);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo mới thất bại!";
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<NotificationDTO?> GetNotiById(Guid id)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

                var noti = await _unitOfWork.NotificationRepo.GetByIdAsync(id);
                if (noti == null)
                {
                    return null;
                }

                noti.ReadAt = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                noti.IsRead = true;
                noti.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                noti.Status = "Đã Nhận";
                _unitOfWork.NotificationRepo.Update(noti);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (result) { 
                    return _mapper.Map<NotificationDTO>(noti); 
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<int> CountNotiNotRead(Guid cusId)
        {
            return await _unitOfWork.NotificationRepo.CountNotiNotRead(cusId);
        }

        public async Task<string?> MessageInActiveTableBooking(string tableName, string status)
        {
            return $"Thông báo, hiện tại {tableName} đang {status}. " +
                $"\n Các lịch đặt trước của bạn với {tableName} sẽ bị huỷ và sẽ được hoàn lại toàn bộ tiền cọc." +
                $"\n Chúng tôi chân thành xin lỗi vì sự bất tiện này.";
        }

        public async Task<string?> MessageChangeTableBooking(string oldTable, string newTable, string status)
        {
            return $"Thông báo, hiện tại {oldTable} đang {status}. " +
                $"\n Chúng tôi sẽ chuyện bạn sang bàn mới là {newTable} với giá cả và thuộc tính tương đương." +
                $"\n Chúng tôi chân thành xin lỗi vì sự bất tiện này.";
        }

        public async Task<PageResult<GetAllNotificationDTO>> GetAllNotification(NotificationFilter notifyFilters)
        {
            var list = _mapper.Map<IEnumerable<GetAllNotificationDTO>>(await _unitOfWork.NotificationRepo.GetAllNotification());
            IQueryable<GetAllNotificationDTO> result = list.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(notifyFilters.Username))
                result = result.Where(x => x.Username.Contains(notifyFilters.Username, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(notifyFilters.Status))
                result = result.Where(x => x.Status.Contains(notifyFilters.Status, StringComparison.OrdinalIgnoreCase));

            if (notifyFilters.IsRead != null)
                result = result.Where(x => x.IsRead.Equals(notifyFilters.IsRead));

            if (notifyFilters.CustomerId != null)
                result = result.Where(x => x.CustomerID.Equals(notifyFilters.CustomerId));


            //Sorting
            if (!string.IsNullOrEmpty(notifyFilters.SortBy))
            {
                switch (notifyFilters.SortBy)
                {

                    case "createdDate":
                        result = notifyFilters.SortAscending ?
                            result.OrderBy(x => x.CreatedDate) :
                            result.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "updatedDate":
                        result = notifyFilters.SortAscending ?
                            result.OrderBy(x => x.UpdatedDate) :
                            result.OrderByDescending(x => x.UpdatedDate);
                        break;
                    case "readAt":
                        result = notifyFilters.SortAscending ?
                            result.OrderBy(x => x.ReadAt) :
                            result.OrderByDescending(x => x.ReadAt);
                        break;
                }
            }

            //Paging
            var pageItems = result
                .Skip((notifyFilters.PageNumber - 1) * notifyFilters.PageSize)
                .Take(notifyFilters.PageSize)
                .ToList();

            return new PageResult<GetAllNotificationDTO>
            {
                Items = pageItems,
                PageNumber = notifyFilters.PageNumber,
                PageSize = notifyFilters.PageSize,
                TotalItem = result.Count(),
                TotalPages = (int)Math.Ceiling((decimal)result.Count() / (decimal)notifyFilters.PageSize)
            };
        }
    }
}
