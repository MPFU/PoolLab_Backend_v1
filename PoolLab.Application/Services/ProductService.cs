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

        public async Task<PageResult<GetAllProduct>> GetAllProducts(ProductFilter productFilter)
        {
            var productList = _mapper.Map<IEnumerable<GetAllProduct>>(await _unitOfWork.ProductRepo.GetAllProducts());
            IQueryable<GetAllProduct> result = productList.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(productFilter.Name))
                result = result.Where(x => x.Name.Contains(productFilter.Name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(productFilter.ProductTypeName))
                result = result.Where(x => x.ProductTypeName.Contains(productFilter.ProductTypeName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(productFilter.ProductGroupName))
                result = result.Where(x => x.GroupName.Contains(productFilter.ProductGroupName, StringComparison.OrdinalIgnoreCase));

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
                    case "createdDate":
                        result = productFilter.SortAscending ?
                            result.OrderBy(x => x.CreatedDate) :
                            result.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "updatedDate":
                        result = productFilter.SortAscending ?
                            result.OrderBy(x => x.UpdatedDate) :
                            result.OrderByDescending(x => x.UpdatedDate);
                        break;
                    case "price":
                        result = productFilter.SortAscending ?
                            result.OrderBy(x => x.Price) :
                            result.OrderByDescending (x => x.Price);
                        break;
                    case "quantity":
                        result = productFilter.SortAscending ?
                            result.OrderBy(x => x.Quantity) :
                            result.OrderByDescending(x => x.Quantity);
                        break;
                }
            }

            //Paging
            var pageItems = result
                .Skip((productFilter.PageNumber - 1) * productFilter.PageSize)
                .Take(productFilter.PageSize)
                .ToList();

            return new PageResult<GetAllProduct>
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
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); 
                var check = _mapper.Map<Product>(create);
                check.Id = Guid.NewGuid();
                check.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                check.ProductImg = !string.IsNullOrEmpty(create.ProductImg) ? create.ProductImg : "https://capstonepoollab.blob.core.windows.net/product/5ed65cfb-38a4-4720-ae10-5b68a5d10578.jpg";
                check.Status = "Còn Hàng";
                var namecheck = await _unitOfWork.ProductRepo.CheckProductNameDup(check.Name, (Guid)check.StoreId);
                if (namecheck)
                {
                    return "Tên sản phẩm bị trùng!";
                }
                else
                {
                    await _unitOfWork.ProductRepo.AddAsync(check);
                    var result = await _unitOfWork.SaveAsync() > 0;
                    if (!result)
                    {
                        return "Tạo mới sản phẩm thất bại!";
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

        public async Task<string?> UpdateProduct(Guid id, UpdateProductInfo create)
        {
            try
            {
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

                var check = await _unitOfWork.ProductRepo.GetByIdAsync(id);
                if (check == null)
                {
                    return "Không tìm thấy sản phẩm này.";
                }
                var dup = await _unitOfWork.ProductRepo.CheckProductNameDup(create.Name, (Guid)check.StoreId, check.Id);
                if (dup)
                {
                    return "Tên sản phẩm bị trùng!";
                }
                check.Name = !string.IsNullOrEmpty(create.Name) ? create.Name : check.Name;
                check.Descript = !string.IsNullOrEmpty(create.Descript) ? create.Descript : check.Descript;
                check.Quantity = check.Quantity != null ? create.Quantity : check.Quantity;
                check.MinQuantity = check.MinQuantity != null ? create.MinQuantity : check.MinQuantity;
                check.Price = check.Price != null ? create.Price : check.Price;
                check.ProductGroupId = check.ProductGroupId != null ? create.ProductGroupId : check.ProductGroupId;
                check.UnitId = check.UnitId != null ? create.UnitId : check.UnitId;
                check.ProductTypeId = check.ProductType != null ? create.ProductTypeId : check.ProductTypeId;
                check.ProductImg = !string.IsNullOrEmpty(create.ProductImg) ? create.ProductImg : check.ProductImg;
                check.Status = !string.IsNullOrEmpty(create.Status) ? create.Status : check.Status;
                check.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);

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

        public async Task<string?> UpdateQuantityProduct(Guid id, int quantity, bool check)
        {
            try
            {
                var product = await _unitOfWork.ProductRepo.GetByIdAsync(id);
                if (product == null)
                {
                    return "Không tìm thấy sản phẩm này!";
                }

                if (product.Quantity != null)
                {
                    if (check)
                    {
                        if(product.Quantity == 0)
                        {
                            product.Status = "Còn Hàng";
                        }
                        product.Quantity += quantity;
                        _unitOfWork.ProductRepo.Update(product);
                        var result = await _unitOfWork.SaveAsync() > 0;
                        if (!result)
                        {
                            return $"Cập nhật số lượng {product.Name} thất bại!";
                        }
                    }
                    else
                    {
                        if(quantity > product.Quantity)
                        {
                            return $"Số lượng sản phẩm {product.Name} không đủ để phục vụ!";
                        }
                        product.Quantity -= quantity;
                        if (product.Quantity == 0)
                        {
                            product.Status = "Hết Hàng";
                        }
                        _unitOfWork.ProductRepo.Update(product);
                        var result = await _unitOfWork.SaveAsync() > 0;
                        if (!result)
                        {
                            return $"Cập nhật số lượng {product.Name} thất bại!";
                        }
                    }
                }
                return null;
            }catch(DbUpdateException)
            {
                throw;
            }
        }
    }
}
