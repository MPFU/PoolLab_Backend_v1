using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStoreService _storeService;

        public EventService(IMapper mapper, IUnitOfWork unitOfWork, IStoreService storeService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _storeService = storeService;

        }


        public async Task<string?> AddNewEvent(CreateEventDTO newEventDTO)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);


                EventDTO eventDTO = new EventDTO();
                
                if (!string.IsNullOrEmpty(newEventDTO.TimeStart) || !string.IsNullOrEmpty(newEventDTO.TimeEnd))
                {
                    var timeStart = DateTime.ParseExact(newEventDTO.TimeStart, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    var timeEnd = DateTime.ParseExact(newEventDTO.TimeEnd, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
                    
                    if (timeStart >= timeEnd)
                    {
                        return "Thời gian bắt đầu và kết thúc không hợp lệ!";
                    }

                    eventDTO.TimeStart = timeStart;
                    eventDTO.TimeEnd = timeEnd;
                }

                if(newEventDTO.StoreId != null)
                {
                    var store = await _storeService.CheckStoreIsActive((Guid)newEventDTO.StoreId);

                    if (store != null)
                    {
                        return store;
                    }
                    eventDTO.StoreId = newEventDTO.StoreId;
                }
                eventDTO.Id = Guid.NewGuid();
                eventDTO.Title = newEventDTO.Title;
                eventDTO.ManagerId = newEventDTO.ManagerId;
                eventDTO.Descript = newEventDTO.Descript;
                eventDTO.Thumbnail = newEventDTO.Thumbnail;
                eventDTO.Status = "Đã Tạo";
                eventDTO.CreatedDate = now;
                await _unitOfWork.EventRepo.AddAsync(_mapper.Map<Event>(eventDTO));
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo sự kiện thất bại!";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string?> DeleteEvent(Guid Id)
        {
            try
            {

                var events = await _unitOfWork.EventRepo.GetByIdAsync(Id);
                if (events == null)
                {
                    return "Không tìm thấy sự kiện này!";
                }

                _unitOfWork.EventRepo.Delete(events);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Xoá sự kiện thất bại!";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageResult<GetEventDTO>?> GetAllEvent(EventFilter eventFilter)
        {
            var eventList = _mapper.Map<IEnumerable<GetEventDTO>>(await _unitOfWork.EventRepo.GetAllEvent());
            IQueryable<GetEventDTO> query = eventList.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(eventFilter.Username))
                query = query.Where(x => x.Username.Contains(eventFilter.Username, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(eventFilter.FullName))
                query = query.Where(x => x.FullName.Contains(eventFilter.FullName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(eventFilter.Title))
                query = query.Where(x => x.Title.Contains(eventFilter.Title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(eventFilter.StoreName))
                query = query.Where(x => x.StoreName.Contains(eventFilter.StoreName, StringComparison.OrdinalIgnoreCase));

            if (eventFilter.StoreId != null)
                query = query.Where(x => x.StoreId.Equals(eventFilter.StoreId));

            if (eventFilter.ManagerId != null)
                query = query.Where(x => x.ManagerId.Equals(eventFilter.ManagerId));

            if (!string.IsNullOrEmpty(eventFilter.Status))
                query = query.Where(x => x.Status.Contains(eventFilter.Status, StringComparison.OrdinalIgnoreCase));

            //Sorting
            if (!string.IsNullOrEmpty(eventFilter.SortBy))
            {
                switch (eventFilter.SortBy)
                {
                    case "createdDate":
                        query = eventFilter.SortAscending ?
                            query.OrderBy(x => x.CreatedDate) :
                            query.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "updatedDate":
                        query = eventFilter.SortAscending ?
                            query.OrderBy(x => x.UpdatedDate) :
                            query.OrderByDescending(x => x.UpdatedDate);
                        break;
                }
            }

            //Paging
            var pageItems = query
                .Skip((eventFilter.PageNumber - 1) * eventFilter.PageSize)
                .Take(eventFilter.PageSize)
                .ToList();

            return new PageResult<GetEventDTO>
            {
                Items = pageItems,
                PageNumber = eventFilter.PageNumber,
                PageSize = eventFilter.PageSize,
                TotalItem = query.Count(),
                TotalPages = (int)Math.Ceiling((decimal)query.Count() / (decimal)eventFilter.PageSize)
            };
        }

        public async Task<GetEventDTO?> GetEventById(Guid id)
        {
            return _mapper.Map<GetEventDTO>(await _unitOfWork.EventRepo.GetEventById(id));
        }

        public async Task<string?> UpdateEventInfo(Guid Id, CreateEventDTO newEventDTO)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);



                var events = await _unitOfWork.EventRepo.GetEventById(Id);
                if (events == null)
                {
                    return "Không tìm thấy sự kiện này!";
                }

                if (!string.IsNullOrEmpty(newEventDTO.TimeStart) || !string.IsNullOrEmpty(newEventDTO.TimeEnd))
                {
                    var timeStart = !string.IsNullOrEmpty(newEventDTO.TimeStart) 
                        ? DateTime.ParseExact(newEventDTO.TimeStart, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
                        : events.TimeStart;

                    var timeEnd = !string.IsNullOrEmpty(newEventDTO.TimeEnd) 
                        ? DateTime.ParseExact(newEventDTO.TimeStart, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
                        : events.TimeEnd;

                    if (timeStart >= timeEnd)
                    {
                        return "Thời gian diễn ra sự kiện không hợp lệ!";
                    }
                }
                events.Title = !string.IsNullOrEmpty(newEventDTO.Title) ? newEventDTO.Title : events.Title;

                events.Descript = !string.IsNullOrEmpty(newEventDTO.Descript) ? newEventDTO.Descript : events.Descript;

                events.Thumbnail = !string.IsNullOrEmpty(newEventDTO.Thumbnail) ? newEventDTO.Title : events.Thumbnail;

                events.StoreId = newEventDTO.StoreId != null ? newEventDTO.StoreId : events.StoreId;

                events.ManagerId = newEventDTO.ManagerId != null ? newEventDTO.ManagerId : events.ManagerId;

                events.TimeStart = !string.IsNullOrEmpty(newEventDTO.TimeStart)
                        ? DateTime.ParseExact(newEventDTO.TimeStart, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
                        : events.TimeStart;

                events.TimeEnd = !string.IsNullOrEmpty(newEventDTO.TimeEnd)
                        ? DateTime.ParseExact(newEventDTO.TimeStart, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None)
                        : events.TimeEnd;

                events.UpdatedDate = now;

                _unitOfWork.EventRepo.Update(events);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật thất bại!";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string?> UpdateStatusEvent(Guid Id, UpdateStatusEventDTO eventDTO)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);

                var events = await _unitOfWork.EventRepo.GetEventById(Id);
                if (events == null)
                {
                    return "Không tìm thấy sự kiện này!";
                }

                events.Status = !string.IsNullOrEmpty(eventDTO.Status) ? eventDTO.Status : events.Status;
                events.UpdatedDate = now;
                _unitOfWork.EventRepo.Update(events);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật thất bại!";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
