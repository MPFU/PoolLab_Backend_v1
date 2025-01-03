﻿using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface ICompanyService 
    {
        Task<string?> AddNewCompany(CreateCompanyDTO companyDTO);
        Task<CompanyDTO?> GetCompanyById(Guid id);
        Task<IEnumerable<CompanyDTO>?> GetAllCompany();
        Task<string?> UpdateCompany(Guid Id, CreateCompanyDTO companyDTO);
    }
}
