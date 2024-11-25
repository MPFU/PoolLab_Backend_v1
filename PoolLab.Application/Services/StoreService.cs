using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
                store.Id = Guid.NewGuid();
                store.Rated = 5;
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                store.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                store.Status = "Đang hoạt động";
                await _unitOfWork.StoreRepo.AddAsync(_mapper.Map<Store>(store));
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Thêm mới cửa hàng thất bại!";
                }
                return null;
            }
            catch (DbUpdateException )
            {
                throw;
            }
            
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

        public async Task<IEnumerable<StoreDTO>?> GetAllStore()
        {
            return _mapper.Map<IEnumerable<StoreDTO>?>(await _unitOfWork.StoreRepo.GetAllAsync());
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
                store.Name = newStore.Name != null ? newStore.Name : store.Name;
                store.Address = newStore.Address != null ? newStore.Address : store.Address;
                store.StoreImg = newStore.StoreImg != null ? newStore .StoreImg : store.StoreImg;
                store.PhoneNumber = newStore.PhoneNumber != null ? newStore .PhoneNumber : store.PhoneNumber;
                store.Descript = newStore.Descript != null ? newStore.Descript : store.Descript;
                store.TimeStart = newStore.TimeStart != null ? TimeOnly.Parse(newStore.TimeStart) : store.TimeStart;
                store.TimeEnd = newStore.TimeEnd != null ? TimeOnly.Parse(newStore.TimeEnd) : store.TimeEnd;
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
            }catch(DbUpdateException )
            {
                throw;
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
            }catch(DbUpdateException )
            {
                throw;
            }
        }
    }
}
