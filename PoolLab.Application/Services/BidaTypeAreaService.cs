using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PoolLab.Application.FilterModel;
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
    public class BidaTypeAreaService : IBidaTypeAreaService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BidaTypeAreaService(IMapper mapper , IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        public async Task<string?> AddNewBidaTypeArea(NewTypeAreaDTO newType)
        {
            try
            {
                var type =  _mapper.Map<BilliardTypeArea>(newType);
                if(await _unitOfWork.BidaTypeAreaRepo.CheckDuplicate(type.AreaID, newType.BilliardTypeID))
                {
                    return null;
                }
                else
                {
                    type.Id = Guid.NewGuid();
                    await _unitOfWork.BidaTypeAreaRepo.AddAsync(type);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Thất bại";
                    }
                }
                
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> DeleteBidaTypeArea(Guid? typeID, Guid? areaID, Guid? storeID)
        {
            try
            {
                var type = await _unitOfWork.BidaTypeAreaRepo.GetBidaTypeArea(typeID, areaID, storeID);

                if(type == null)
                {
                    return "Không tìm thấy loại bàn và khu vực này!";
                }
                var count = await _unitOfWork.BilliardTableRepo.CountTableTypeArea(typeID, areaID, storeID);
                if(count == 1)
                {
                    _unitOfWork.BidaTypeAreaRepo.Delete(type);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Thất bại";
                    }
                }
                
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<GetBidaTypeAreaDTO>?> GetAllBidaTypeAre(BidaTypeAreFilter filter)
        {
            var bidaList = _mapper.Map<IEnumerable<GetBidaTypeAreaDTO>>(await _unitOfWork.BidaTypeAreaRepo.GetAllBidaTypeAre());
            IQueryable<GetBidaTypeAreaDTO> result = bidaList.AsQueryable();

            //Filter
            if (filter.Id != null)
                result = result.Where(x => x.Id == filter.Id);

            if (filter.AreaID != null)
                result = result.Where(x => x.AreaID == filter.AreaID);

            if (filter.BilliardTypeID != null)
                result = result.Where(x => x.BilliardTypeID == filter.BilliardTypeID);

            if (filter.StoreID != null)
                result = result.Where(x => x.StoreID == filter.StoreID);

            return result.ToList();
        }
    }
}
