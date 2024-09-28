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
    public class AreaService : IAreaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AreaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<string?> AddNewArea(NewAreaDTO newAreaDTO)
        {
            try
            {
                var area = _mapper.Map<Area>(newAreaDTO);
                area.Id = Guid.NewGuid();
                await _unitOfWork.AreaRepo.AddAsync(area);
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

        public async Task<string?> DeleteArea(Guid Id)
        {
            try
            {
                var area = await _unitOfWork.AreaRepo.GetByIdAsync(Id);
                if (area == null)
                {
                    return "Không tìm thấy!";
                }
                _unitOfWork.AreaRepo.Delete(area);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Xoá thất bại!";
                }
                return null;
            }catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AreaDTO>?> GetAllArea()
        {
            return _mapper.Map<IEnumerable<AreaDTO>?>(await _unitOfWork.AreaRepo.GetAllAsync());
        }

        public async Task<AreaDTO?> GetAreaById(Guid id)
        {
           return _mapper.Map<AreaDTO?>(await _unitOfWork.AreaRepo.GetByIdAsync(id));
        }

        public async Task<string?> UpdateArea(Guid Id, NewAreaDTO newAreaDTO)
        {
            try
            {
                var are = await _unitOfWork.AreaRepo.GetByIdAsync(Id);
                if(are == null)
                {
                    return "Không tìm thấy!";
                }
                are.Name = newAreaDTO.Name;
                are.Descript = newAreaDTO.Descript;
                _unitOfWork.AreaRepo.Update(are);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Cập nhật thất bại!";
                }
                return null;
            }catch (DbUpdateException)
            {
                throw;
            }
        }
    }
}
