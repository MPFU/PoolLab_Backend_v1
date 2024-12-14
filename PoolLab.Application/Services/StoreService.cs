using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

namespace PoolLab.Application.Interface
{
    public class StoreService : IStoreService
    {
        private readonly IUnitOfWork _unitOfWork ;
        private readonly IMapper _mapper ;

        public StoreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string?> AddNewStore(NewStoreDTO newStoreDTO)
        {
            try
            {               
                var store = _mapper.Map<Store>(newStoreDTO);
                if(store.TimeStart >= store.TimeEnd)
                {
                    return "Thời gian mở cửa và đóng cửa không hợp lệ!";
                }
                var check = await _unitOfWork.StoreRepo.GetStoreByName(store.Name);
                if(check != null)
                {
                    return "Tên chi nhánh đã bị trùng!";
                }
                store.Id = Guid.NewGuid();
                store.Rated = 5;
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                store.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                store.Status = "Hoạt Động";
                await _unitOfWork.StoreRepo.AddAsync(_mapper.Map<Store>(store));
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Thêm mới cửa hàng thất bại!";
                }
                return null;
            }
            catch (Exception ex )
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<string?> CheckStoreIsActive(Guid Id)
        {
            var store = await _unitOfWork.StoreRepo.GetByIdAsync(Id);

            return store == null ? "Không tìm thấy chi nhánh này!" : (store.Status.Equals("Dừng Hoạt Động")) ? "Chi nhánh này không còn hoạt động!" : null;
        }

        public async Task<string?> DeleteStore(Guid Id)
        {
            try
            {
                var store = await _unitOfWork.StoreRepo.GetByIdAsync(Id);
                if (store != null)
                {
                    _unitOfWork.StoreRepo.Delete(store);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Xoá cửa hàng thất bại!";
                    }
                    return null;
                }
                return "Không tìm thấy cửa tiệm này!";
            }catch(DbUpdateException )
            {
                throw;
            }
        }

        public async Task<PageResult<StoreDTO>?> GetAllStore(StoreFilter storeFilter)
        {
            var storeList = _mapper.Map<IEnumerable<StoreDTO>>(await _unitOfWork.StoreRepo.GetAllAsync());
            IQueryable<StoreDTO> query = storeList.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(storeFilter.Name))
                query = query.Where(x => x.Name.Contains(storeFilter.Name, StringComparison.OrdinalIgnoreCase));

            if (storeFilter.CompanyId != null)
                query = query.Where(x => x.CompanyId.Equals(storeFilter.CompanyId));

            if (!string.IsNullOrEmpty(storeFilter.Status))
                query = query.Where(x => x.Status.Contains(storeFilter.Status, StringComparison.OrdinalIgnoreCase));

            //Sorting
            if (!string.IsNullOrEmpty(storeFilter.SortBy))
            {
                switch (storeFilter.SortBy)
                {
                    case "createdDate":
                        query = storeFilter.SortAscending ?
                            query.OrderBy(x => x.CreatedDate) :
                            query.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "updatedDate":
                        query = storeFilter.SortAscending ?
                            query.OrderBy(x => x.UpdatedDate) :
                            query.OrderByDescending(x => x.UpdatedDate);
                        break;
                    case "rated":
                        query = storeFilter.SortAscending ?
                            query.OrderBy(x => x.Rated) :
                            query.OrderByDescending(x => x.Rated);
                        break;
                }
            }

            //Paging
            var pageItems = query
                .Skip((storeFilter.PageNumber - 1) * storeFilter.PageSize)
                .Take(storeFilter.PageSize)
                .ToList();

            return new PageResult<StoreDTO>
            {
                Items = pageItems,
                PageNumber = storeFilter.PageNumber,
                PageSize = storeFilter.PageSize,
                TotalItem = query.Count(),
                TotalPages = (int)Math.Ceiling((decimal)query.Count() / (decimal)storeFilter.PageSize)
            };
        }

        public async Task<StoreDTO?> GetStoreById(Guid id)
        {
            return _mapper.Map<StoreDTO>(await _unitOfWork.StoreRepo.GetByIdAsync(id));
        }

        public async Task<string?> UpdateStore(Guid Id, NewStoreDTO newStore)
        {
            try
            {
                var store = await _unitOfWork.StoreRepo.GetByIdAsync(Id);
                if (store == null)
                {
                    return "Không tìm thấy cửa hàng này!";
                }
                if(!string.IsNullOrEmpty(newStore.Name))
                {
                    var check = await _unitOfWork.StoreRepo.CheckNameDuplicate(Id,store.Name);
                    if (check)
                    {
                        return "Tên chi nhánh đã bị trùng!";
                    }
                    else
                    {
                        store.Name = newStore.Name;
                    }
                }
                store.Address = !string.IsNullOrEmpty(newStore.Address) ? newStore.Address : store.Address;
                store.StoreImg = !string.IsNullOrEmpty(newStore.StoreImg) ? newStore .StoreImg : store.StoreImg;
                store.PhoneNumber = !string.IsNullOrEmpty(newStore.PhoneNumber) ? newStore .PhoneNumber : store.PhoneNumber;
                store.Descript = !string.IsNullOrEmpty(newStore.Descript) ? newStore.Descript : store.Descript;
                if(!string.IsNullOrEmpty(newStore.TimeStart) || !string.IsNullOrEmpty(newStore.TimeEnd))
                {
                    var timeStart = !string.IsNullOrEmpty(newStore.TimeStart) ? TimeOnly.Parse(newStore.TimeStart) : store.TimeStart;
                    var timeEnd = !string.IsNullOrEmpty(newStore.TimeEnd) ? TimeOnly.Parse(newStore.TimeEnd) : store.TimeEnd;
                    if (timeStart >= timeEnd)
                    {
                        return "Thời gian mở cửa và đóng cửa không hợp lệ!";
                    }
                }
                store.TimeStart = !string.IsNullOrEmpty(newStore.TimeStart) ? TimeOnly.Parse(newStore.TimeStart) : store.TimeStart;
                store.TimeEnd = !string.IsNullOrEmpty(newStore.TimeEnd) ? TimeOnly.Parse(newStore.TimeEnd) : store.TimeEnd;
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                store.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                _unitOfWork.StoreRepo.Update(store);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật thất bại!";
                }
                return null;
            }catch(Exception e )
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string?> UpdateStoreRating(Guid Id, int rate)
        {
            try
            {
                var store = await _unitOfWork.StoreRepo.GetByIdAsync(Id);
                if (store == null)
                {
                    return "Không tìm thấy chi nhánh này!";
                }

                store.Rated = (store.Rated + rate) / 2;
                store.Rated = Math.Round((decimal)store.Rated, 1);
                _unitOfWork.StoreRepo.Update(store);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật thất bại!";
                }
                return null;
            }catch(Exception e )
            {
                throw new Exception(e.Message);
            }
        }

     
    }
}
