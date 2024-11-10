using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public class GroupProductService : IGroupProductService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GroupProductService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GroupProductDTO?>> GetAllGroupProduct()
        {
            return _mapper.Map<IEnumerable<GroupProductDTO?>>(await _unitOfWork.GroupProductRepo.GetAllAsync());
        }

        public async Task<string?> CreateGroupProduct(CreateGroupProductDTO create)
        {
            try
            {
                var check = _mapper.Map<GroupProduct>(create);
                check.Id = Guid.NewGuid();
                var namecheck = await GetGroupProductByName(check.Name);
                if (namecheck != null)
                {
                    return "Tên nhóm sản phẩm bị trùng.";
                }
                else
                {
                    await _unitOfWork.GroupProductRepo.AddAsync(check);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Tạo mới nhóm sản phẩm thất bại.";
                    }
                    return null;
                }

            }
            catch (DbException)
            {
                throw;
            }
        }

        public async Task<GroupProductDTO?> GetGroupProductByName(string? name)
        {
            var check = await _unitOfWork.GroupProductRepo.SearchByNameAsync(name);
            return _mapper.Map<GroupProductDTO?>(check);
        }

        public async Task<string?> UpdateGroupProduct(Guid id, CreateGroupProductDTO createGroupProductDTO)
        {
            try
            {
                var check = await _unitOfWork.GroupProductRepo.GetByIdAsync(id);
                if (check == null)
                {
                    return "Không tìm thấy nhóm sản phẩm này.";
                }
                if (createGroupProductDTO.Name.Trim() != null && createGroupProductDTO.Descript.Trim() == null ||
                        createGroupProductDTO.Name.Trim() != null && createGroupProductDTO.Descript.Length == 0)
                {
                    check.Name = createGroupProductDTO.Name;
                    createGroupProductDTO.Descript = check.Descript;
                }
                else if (createGroupProductDTO.Descript.Trim() != null && createGroupProductDTO.Name.Trim() == null ||
                            createGroupProductDTO.Descript.Trim() != null && createGroupProductDTO.Name.Length == 0)
                {
                    check.Descript = createGroupProductDTO.Descript;
                    createGroupProductDTO.Name = check.Name;
                }
                else if (createGroupProductDTO.Name.Trim() == null && createGroupProductDTO.Descript.Trim() == null ||
                            createGroupProductDTO.Name.Length == 0 && createGroupProductDTO.Descript.Length == 0)
                {
                    createGroupProductDTO.Name = check.Name;
                    createGroupProductDTO.Descript = check.Descript;
                }
                else
                {
                    check.Name = createGroupProductDTO.Name;
                    check.Descript = createGroupProductDTO.Descript;
                }
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Lưu thất bại.";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> DeleteGroupProduct(Guid id)
        {
            try
            {
                var check = await _unitOfWork.GroupProductRepo.GetByIdAsync(id);
                if (check == null)
                {
                    return "Không tìm thấy nhóm sản phẩm này.";
                }
                _unitOfWork.GroupProductRepo.Delete(check);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Lưu thất bại.";
                }
                return null;
            }
            catch (DbException)
            {
                throw;
            }
        }
    }
}
