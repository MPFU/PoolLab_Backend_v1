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
    public class BilliardTypeService : IBilliardTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BilliardTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<string?> AddNewBidaType(NewBidaTypeDTO newBidaTypeDTO)
        {
            try
            {
                var area = _mapper.Map<BilliardType>(newBidaTypeDTO);
                area.Id = Guid.NewGuid();
                await _unitOfWork.BilliardTypeRepo.AddAsync(area);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Tạo mới thất bại!";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> DeleteBidaType(Guid Id)
        {
            try
            {
                var area = await _unitOfWork.BilliardTypeRepo.GetByIdAsync(Id);
                if (area == null)
                {
                    return "Không tìm thấy!";
                }
                _unitOfWork.BilliardTypeRepo.Delete(area);
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

        public async Task<IEnumerable<BidaTypeDTO?>> GetAllBidaType()
        {
            return _mapper.Map<IEnumerable<BidaTypeDTO?>>(await _unitOfWork.BilliardTypeRepo.GetAllAsync());
        }

        public async Task<BidaTypeDTO?> GetBidaTypeById(Guid id)
        {
            return _mapper.Map<BidaTypeDTO?>(await _unitOfWork.BilliardTypeRepo.GetByIdAsync(id));
        }

        public async Task<string?> UpdateBidaTye(Guid Id, NewBidaTypeDTO newBidaTypeDTO)
        {
            try
            {
                var are = await _unitOfWork.BilliardTypeRepo.GetByIdAsync(Id);
                if (are == null)
                {
                    return "Không tìm thấy!";
                }
                are.Name = newBidaTypeDTO.Name;
                are.Descript = newBidaTypeDTO.Descript;
                _unitOfWork.BilliardTypeRepo.Update(are);
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
