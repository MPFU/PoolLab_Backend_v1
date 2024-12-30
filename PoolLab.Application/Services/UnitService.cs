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
    public class UnitService : IUnitService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UnitService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UnitDTO?>> GetAllUnit()
        {
            return _mapper.Map<IEnumerable<UnitDTO?>>(await _unitOfWork.UnitRepo.GetAllAsync());
        }

        public async Task<string?> CreateUnit(CreateUnitDTO create)
        {
            try
            {
                var check = _mapper.Map<Unit>(create);
                check.Id = Guid.NewGuid();
                var namecheck = await GetUnitByName(check.Name);
                if (namecheck != null)
                {
                    return "Tên đơn vị bị trùng.";
                }
                else
                {
                    await _unitOfWork.UnitRepo.AddAsync(check);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Tạo mới đơn vị thất bại.";
                    }
                    return null;
                }

            }
            catch (DbException)
            {
                throw;
            }
        }

        public async Task<UnitDTO?> GetUnitByName(string? name)
        {
            var check = await _unitOfWork.UnitRepo.SearchByNameAsync(name);
            return _mapper.Map<UnitDTO?>(check);
        }

        public async Task<string?> UpdateUnit(Guid id, CreateUnitDTO create)
        {
            try
            {
                var check = await _unitOfWork.UnitRepo.GetByIdAsync(id);
                if (check == null)
                {
                    return "Không tìm thấy đơn vị này.";
                }

                //if (create.Name.Trim() != null && create.Descript.Trim() == null ||
                //        create.Name.Trim() != null && create.Descript.Length == 0)
                //{
                //    check.Name = create.Name;
                //    create.Descript = check.Descript;
                //}
                //else if (create.Descript.Trim() != null && create.Name.Trim() == null ||
                //            create.Descript.Trim() != null && create.Name.Length == 0)
                //{
                //    check.Descript = create.Descript;
                //    create.Name = check.Name;
                //}
                //else if (create.Name.Trim() == null && create.Descript.Trim() == null ||
                //            create.Name.Length == 0 && create.Descript.Length == 0)
                //{
                //    create.Name = check.Name;
                //    create.Descript = check.Descript;
                //}
                //else
                //{
                //    check.Name = create.Name;
                //    check.Descript = create.Descript;
                //}

                if(!string.IsNullOrEmpty(create.Name))
                {
                    var namecheck = await GetUnitByName(check.Name);
                    if (namecheck != null)
                    {
                        return "Tên đơn vị bị trùng.";
                    }
                    check.Name = create.Name;
                }

                check.Descript = !string.IsNullOrEmpty(create.Descript) ? create.Descript : check.Descript;

                _unitOfWork.UnitRepo.Update(check);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Lưu thất bại.";
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string?> DeleteUnit(Guid id)
        {
            try
            {
                var check = await _unitOfWork.UnitRepo.GetByIdAsync(id);
                if (check == null)
                {
                    return "Không tìm thấy đơn vị này.";
                }
                _unitOfWork.UnitRepo.Delete(check);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Lưu thất bại.";
                }
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
