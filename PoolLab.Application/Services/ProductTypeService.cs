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
                check.Name = createProductTypeDTO.Name;
                var namecheck = await GetProductTypeByName(check.Id, check.Name);
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
                        return "Tạo mới sản phẩm thất bại.";
                    }
                    return null;
                }
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
        public async Task<ProductTypeDTO?> GetProductTypeByName(Guid Id, string name)
        {
            var check = await _unitOfWork.ProductTypeRepo.GetByIdAsync(Id);
            name = check.Name;
            return _mapper.Map<ProductTypeDTO?>(name);
        }
    }
}
