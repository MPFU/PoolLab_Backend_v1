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
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoleDTO?> GetRoleById(Guid id)
        {
            return _mapper.Map<RoleDTO?>(await _unitOfWork.RoleRepo.GetByIdAsync(id));
        }

        public async Task<IEnumerable<RoleDTO>?> GetAllRole()
        {
            return _mapper.Map<IEnumerable<RoleDTO>?>(await _unitOfWork.RoleRepo.GetAllAsync());
        }

        public async Task<string?> AddNewRole(NewRoleDTO roleDTO)
        {
            try
            {
                var check = await _unitOfWork.RoleRepo.GetRoleByName(roleDTO.Name);
                if (check != null)
                {
                    return "Chức vụ này đã tồn tại!";
                }
                var role1 = _mapper.Map<Role>(roleDTO);
                role1.Id = Guid.NewGuid();
                await _unitOfWork.RoleRepo.AddAsync(role1);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Lưu thất bại!";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }

        }

        public async Task<string?> UpdateRole(Guid roleId, NewRoleDTO newRoleDTO)
        {
            try
            {
                var check = await _unitOfWork.RoleRepo.GetByIdAsync(roleId);
                if (check == null)
                {
                    return "Không tìm thấy chức vụ này!";
                }
                check.Name = newRoleDTO.Name;
                _unitOfWork.RoleRepo.Update(check);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Lưu thất bại!";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> DeleteRole(Guid roleId)
        {
            try
            {
                var check = await _unitOfWork.RoleRepo.GetByIdAsync(roleId);
                if (check == null)
                {
                    return "Không tìm thấy chức vụ này!";
                }
                _unitOfWork.RoleRepo.Delete(check);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Lưu thất bại!";
                }
                return null;
            }catch (DbUpdateException)
            {
                throw;
            }
        }
    }
}
