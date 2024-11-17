using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
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
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageResult<ProductDTO>> GetAllProducts(ProductFilter productFilter)
        {
            var productList = _mapper.Map<IEnumerable<ProductDTO>>(await _unitOfWork.ProductRepo.GetAllProducts());
            IQueryable<ProductDTO> result = productList.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(productFilter.Name))
                result = result.Where(x => x.Name.Contains(productFilter.Name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(productFilter.Descript))
                result = result.Where(x => x.Descript.Contains(productFilter.Descript, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(productFilter.ProductImg))
                result = result.Where(x => x.ProductImg.Contains(productFilter.ProductImg, StringComparison.OrdinalIgnoreCase));
           
            if (!string.IsNullOrEmpty(productFilter.Status))
                result = result.Where(x => x.Status.Contains(productFilter.Status, StringComparison.OrdinalIgnoreCase));

            if (productFilter.ProductTypeId != null)
                result = result.Where(x => x.ProductTypeId == productFilter.ProductTypeId);

            if (productFilter.ProductGroupId != null)
                result = result.Where(x => x.ProductGroupId == productFilter.ProductGroupId);

            if (productFilter.StoreId != null)
                result = result.Where(x => x.StoreId == productFilter.StoreId);

            if (productFilter.UnitId != null)
                result = result.Where(x => x.UnitId == productFilter.UnitId);

            //Sorting
            if (!string.IsNullOrEmpty(productFilter.SortBy))
            {
                switch (productFilter.SortBy)
                {
                    case "CreatedDate":
                        result = productFilter.SortAscending ?
                            result.OrderBy(x => x.CreatedDate) :
                            result.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "UpdatedDate":
                        result = productFilter.SortAscending ?
                            result.OrderBy(x => x.UpdatedDate) :
                            result.OrderByDescending(x => x.UpdatedDate);
                        break;
                }
            }

            //Paging
            var pageItems = result
                .Skip((productFilter.PageNumber - 1) * productFilter.PageSize)
                .Take(productFilter.PageSize)
                .ToList();

            return new PageResult<ProductDTO>
            {
                Items = pageItems,
                PageNumber = productFilter.PageNumber,
                PageSize = productFilter.PageSize,
                TotalItem = result.Count(),
                TotalPages = (int)Math.Ceiling((decimal)result.Count() / (decimal)productFilter.PageSize)
            };
        }

        public async Task<string?> CreateProduct(CreateProductDTO create)
        {
            try
            {
                var check = _mapper.Map<Product>(create);
                check.Id = Guid.NewGuid();
                check.CreatedDate = DateTime.Now;
                check.UpdatedDate = DateTime.Now;
                check.Status = "Đang hoạt động";
                var namecheck = await GetProductByName(check.Name);
                if (namecheck != null)
                {
                    return "Tên sản phẩm bị trùng.";
                }
                else
                {
                    await _unitOfWork.ProductRepo.AddAsync(check);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Tạo mới sản phẩm thất bại.";
                    }
                    return null;
                }

            }
            catch (DbException)
            {
                throw;
            }
        }

        public async Task<ProductDTO?> GetProductByName(string? name)
        {
            var check = await _unitOfWork.ProductRepo.SearchByNameAsync(name);
            return _mapper.Map<ProductDTO?>(check);
        }

        public async Task<string?> UpdateProduct(Guid id, CreateProductDTO create)
        {
            try
            {
                var check = await _unitOfWork.ProductRepo.GetByIdAsync(id);
                if (check == null)
                {
                    return "Không tìm thấy sản phẩm này.";
                }
                check.Name = create.Name;
                check.Descript = create.Descript;
                check.Quantity = create.Quantity;
                check.Price = create.Price;
                check.ProductGroupId = create.ProductGroupId;
                check.UnitId = create.UnitId;
                check.ProductType = check.ProductType;
                check.ProductImg = create.ProductImg;
                check.Status = create.Status;
                check.UpdatedDate = DateTime.Now;

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

        public async Task<string?> DeleteProduct(Guid id)
        {
            try
            {
                var check = await _unitOfWork.ProductRepo.GetByIdAsync(id);
                if (check == null)
                {
                    return "Không tìm thấy sản phẩm này.";
                }
                _unitOfWork.ProductRepo.Delete(check);
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

        public async Task<ProductDTO?> SearchProductById(Guid id)
        {
            return _mapper.Map<ProductDTO>(await _unitOfWork.ProductRepo.GetByIdAsync(id));
        }      
    }
}
