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

namespace PoolLab.Application.Interface
{
    public class BilliardPriceService : IBilliardPriceService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BilliardPriceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }

        public async Task<string?> AddNewBidaPrice(NewBilliardPriceDTO newBilliard)
        {
            try
            {
                var bida = _mapper.Map<BilliardPrice>(newBilliard);
                bida.Id = Guid.NewGuid();
                bida.Status = "Kích hoạt";
                await _unitOfWork.BilliardPriceRepo.AddAsync(bida);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo mới thất bại!";
                }
                return null;
            }catch (DbUpdateException) 
            {
                throw;
            }
        }

        public async Task<string?> DeleteBidaPrice(Guid Id)
        {
            try
            {
                var area = await _unitOfWork.BilliardPriceRepo.GetByIdAsync(Id);
                if (area == null)
                {
                    return "Không tìm thấy!";
                }
                _unitOfWork.BilliardPriceRepo.Delete(area);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Xoá thất bại!";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<BilliardPriceDTO>?> GetAllBidaPrice(BidaPriceFilter bidaPriceFilter)
        {
            var priceList = _mapper.Map<IEnumerable<BilliardPriceDTO>>(await _unitOfWork.BilliardPriceRepo.GetAllAsync());
            IQueryable<BilliardPriceDTO> result = priceList.AsQueryable();

            if (!string.IsNullOrEmpty(bidaPriceFilter.Name))
                result = result.Where(x => x.Name.Contains(bidaPriceFilter.Name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(bidaPriceFilter.Status))
                result = result.Where(x => x.Status.Contains(bidaPriceFilter.Status, StringComparison.OrdinalIgnoreCase));

            return result.ToList();
        }

        public async Task<BilliardPriceDTO?> GetBidaPriceById(Guid id)
        {
            return _mapper.Map<BilliardPriceDTO?>(await _unitOfWork.BilliardPriceRepo.GetByIdAsync(id)); 
        }

        public async Task<string?> UpdateBilliardPrice(Guid Id, NewBilliardPriceDTO newBilliardPrice)
        {
            try
            {
                var are = await _unitOfWork.BilliardPriceRepo.GetByIdAsync(Id);
                if (are == null)
                {
                    return "Không tìm thấy!";
                }
                are.Name = !string.IsNullOrEmpty(newBilliardPrice.Name) ? newBilliardPrice.Name : are.Name;
                are.Descript = !string.IsNullOrEmpty(newBilliardPrice.Descript) ? newBilliardPrice.Descript : are.Descript;
                are.OldPrice = newBilliardPrice.OldPrice != null ? newBilliardPrice.OldPrice : are.OldPrice;
                are.NewPrice = newBilliardPrice.NewPrice != null ? newBilliardPrice.NewPrice : are.NewPrice;
                are.TimeStart = newBilliardPrice.TimeStart != null ? newBilliardPrice.TimeStart : are.TimeStart;
                are.TimeEnd = newBilliardPrice.TimeEnd != null ? newBilliardPrice.TimeEnd : are.TimeEnd;
                _unitOfWork.BilliardPriceRepo.Update(are);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật thất bại!";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> UpdateBilliardPriceStatus(Guid Id,string status)
        {
            try
            {
                var are = await _unitOfWork.BilliardPriceRepo.GetByIdAsync(Id);
                if (are == null)
                {
                    return "Không tìm thấy!";
                }
                are.Status = status;
                _unitOfWork.BilliardPriceRepo.Update(are);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật thất bại!";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
    }
}
