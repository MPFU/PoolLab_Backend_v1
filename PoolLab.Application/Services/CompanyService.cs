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
    public class CompanyService : ICompanyService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string?> AddNewCompany(CreateCompanyDTO companyDTO)
        {
            try
            {
                var company = _mapper.Map<Company>(companyDTO);
                company.Id = Guid.NewGuid();
                company.Status = "Hoạt động";
                await _unitOfWork.CompanyRepo.AddAsync(company);
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

        public async Task<IEnumerable<CompanyDTO>?> GetAllCompany()
        {
            return _mapper.Map<IEnumerable<CompanyDTO>?>(await _unitOfWork.CompanyRepo.GetAllAsync());
        }

        public async Task<CompanyDTO?> GetCompanyById(Guid id)
        {
            return _mapper.Map<CompanyDTO?>(await _unitOfWork.CompanyRepo.GetByIdAsync(id));
        }

        public async Task<string?> UpdateCompany(Guid Id, CreateCompanyDTO companyDTO)
        {
            try
            {
                var company = await _unitOfWork.CompanyRepo.GetByIdAsync(Id);
                if (company == null)
                {
                    return "Không tìm thấy!";
                }
                company.Name = companyDTO.Name;
                company.Descript = companyDTO.Descript;
                company.Address = companyDTO.Address;
                company.CompanyImg = companyDTO.CompanyImg;
                _unitOfWork.CompanyRepo.Update(company);
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
