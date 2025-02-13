﻿using AutoMapper;
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
                var check = await _unitOfWork.AreaRepo.CheckDuplicate((Guid)newAreaDTO.StoreId, newAreaDTO.Name,null);
                if (check)
                {
                    return "Tên khu vực trong quán đã bị trùng!";
                }
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

        public async Task<IEnumerable<AreaDTO>?> GetAllArea(AreaFilter areaFilter)
        {
            var priceList = _mapper.Map<IEnumerable<AreaDTO>>(await _unitOfWork.AreaRepo.GetAllAsync());
            IQueryable<AreaDTO> result = priceList.AsQueryable();

            if (areaFilter.StoreId != null)
                result = result.Where(x => x.StoreId == areaFilter.StoreId);

            return result.ToList();
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

                if(!string.IsNullOrEmpty(newAreaDTO.Name))
                {
                    var check = await _unitOfWork.AreaRepo.CheckDuplicate((Guid)newAreaDTO.StoreId, newAreaDTO.Name, Id);
                    if (check)
                    {
                        return "Tên khu vực trong quán đã bị trùng!";
                    }
                    else
                    {
                        are.Name = newAreaDTO.Name;
                    }
                }
                are.Descript = !string.IsNullOrEmpty(newAreaDTO.Descript) ? newAreaDTO.Descript : are.Descript;
                are.AreaImg = !string.IsNullOrEmpty(newAreaDTO.AreaImg) ? newAreaDTO.AreaImg : are.AreaImg;
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
