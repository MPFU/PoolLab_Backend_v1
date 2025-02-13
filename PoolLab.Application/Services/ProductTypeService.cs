﻿using AutoMapper;
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
    public class ProductTypeService : IProductTypeService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductTypeService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ProductTypeDTO?>> GetAllProductTypes()
        {
            return _mapper.Map<IEnumerable<ProductTypeDTO?>>(await _unitOfWork.ProductTypeRepo.GetAllAsync());
        }

        public async Task<string?> CreateNewProductType(CreateProductTypeDTO createProductTypeDTO)
        {
            try
            {
                var check = _mapper.Map<ProductType>(createProductTypeDTO);
                check.Id = Guid.NewGuid();
                //var namecheck = await _unitOfWork.ProductTypeRepo.SearchByNameAsync(check.Name);
                var namecheck = await GetProductTypeByName(check.Name);
                if (namecheck != null)
                {
                    return "Tên loại sản phẩm bị trùng.";
                }
                else
                {
                    await _unitOfWork.ProductTypeRepo.AddAsync(check);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Tạo mới loại sản phẩm thất bại.";
                    }
                    return null;
                }
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
        public async Task<ProductTypeDTO?> GetProductTypeByName(string? name)
        {
            var check = await _unitOfWork.ProductTypeRepo.SearchByNameAsync(name);
            return _mapper.Map<ProductTypeDTO?>(check);
        }
        public async Task<string?> UpdateProductType(Guid id, CreateProductTypeDTO createProductTypeDTO)
        {
            try
            {
                var check = await _unitOfWork.ProductTypeRepo.GetByIdAsync(id);
                if (check == null)
                {
                    return "Không tìm thấy loại sản phẩm này.";
                }
                if (createProductTypeDTO.Name.Trim() != null && createProductTypeDTO.Descript.Trim() == null ||
                            createProductTypeDTO.Name.Trim() != null && createProductTypeDTO.Descript.Length == 0)
                {
                    check.Name = createProductTypeDTO.Name;
                    createProductTypeDTO.Descript = check.Descript;
                }
                else if (createProductTypeDTO.Descript.Trim() != null && createProductTypeDTO.Name.Trim() == null ||
                            createProductTypeDTO.Descript.Trim() != null && createProductTypeDTO.Name.Length == 0)
                {
                    createProductTypeDTO.Name = check.Name;
                    check.Descript = createProductTypeDTO.Descript;                  
                }
                else if (createProductTypeDTO.Name.Trim() == null && createProductTypeDTO.Descript.Trim() == null || 
                            createProductTypeDTO.Name.Length == 0 && createProductTypeDTO.Descript.Length == 0)
                {
                    createProductTypeDTO.Name = check.Name;
                    createProductTypeDTO.Descript = check.Descript;
                }
                else
                {
                    check.Name = createProductTypeDTO.Name;
                    check.Descript = createProductTypeDTO.Descript;
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
        public async Task<string?> DeleteProductType(Guid id)
        {
            try
            {
                var check = await _unitOfWork.ProductTypeRepo.GetByIdAsync(id);
                if (check == null)
                {
                    return "Không tìm thấy loại sản phẩm này.";
                }
                _unitOfWork.ProductTypeRepo.Delete(check);
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
