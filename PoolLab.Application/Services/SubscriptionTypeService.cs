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
    public class SubscriptionTypeService : ISubscriptionTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubscriptionTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string?> AddSubType(AddNewSubTypeDTO subTypeDTO)
        {
            try
            {
                var check = await _unitOfWork.SubscriptionTypeRepo.CheckDuplicate(null, subTypeDTO.Name);
                if (check)
                {
                    return "Tên loại gói đã bị trùng!";
                }
                var sub = _mapper.Map<SubscriptionType>(subTypeDTO);
                sub.Id = Guid.NewGuid();
                await _unitOfWork.SubscriptionTypeRepo.AddAsync(sub);
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

        public async Task<string?> DeleteSubType(Guid Id)
        {
            try
            {
                var sub = await _unitOfWork.SubscriptionTypeRepo.GetByIdAsync(Id);
                if (sub == null)
                {
                    return "Không tìm thấy!";
                }
                _unitOfWork.SubscriptionTypeRepo.Delete(sub);
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

        public async Task<IEnumerable<SubscriptionTypeDTO>?> GetAllSubType()
        {
            return _mapper.Map<IEnumerable<SubscriptionTypeDTO>?>(await _unitOfWork.SubscriptionTypeRepo.GetAllAsync());
        }

        public async Task<SubscriptionTypeDTO?> GetSubTypeById(Guid id)
        {
            return _mapper.Map<SubscriptionTypeDTO>(await _unitOfWork.SubscriptionTypeRepo.GetByIdAsync(id));
        }

        public async Task<string?> UpdateSupType(Guid Id, AddNewSubTypeDTO subTypeDTO)
        {
            try
            {
               

                var sub = await _unitOfWork.SubscriptionTypeRepo.GetByIdAsync(Id);
                if (sub == null)
                {
                    return "Không tìm thấy!";
                }

                if(sub.Name != null)
                {
                    var check = await _unitOfWork.SubscriptionTypeRepo.CheckDuplicate(Id, sub.Name);
                    if (check)
                    {
                        return "Tên loại gói đã bị trùng!";
                    }
                    else
                    {
                        sub.Name = subTypeDTO.Name;
                    }
                }
                sub.Descript = !string.IsNullOrEmpty(subTypeDTO.Descript) ? subTypeDTO.Descript : subTypeDTO.Descript;              
                _unitOfWork.SubscriptionTypeRepo.Update(sub);
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
