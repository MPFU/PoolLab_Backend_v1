using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PoolLab.Application.Interface;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Interface;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Services
{
    public class ConfigTableService : IConfigTableService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ConfigTableService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string?> CreateConfigTable(NewConfigDTO newConfigDTO)
        {
            try
            {
                var check = await _unitOfWork.ConfigTableRepo.GetAllAsync();
                if (check.Count() > 0)
                {
                    return "Cấu hình hệ thống đã được tạo!";
                }
                var config = _mapper.Map<ConfigTable>(newConfigDTO);
                config.Id = Guid.NewGuid();
                config.Name = "Cài Đặt";
                config.Status = "Hoạt Động";
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                config.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                await _unitOfWork.ConfigTableRepo.AddAsync(config);
                var result = await _unitOfWork.SaveAsync() > 0;
                if(!result)
                {
                    return "Tạo mới thất bại!";
                }
                return null;
            }catch(DbUpdateException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ConfigTableDTO?>> GetAllConfig()
        {
            return _mapper.Map<IEnumerable<ConfigTableDTO?>>(await _unitOfWork.ConfigTableRepo.GetAllAsync());
        }

        public async Task<ConfigTableDTO?> GetConfigTableByName()
        {
            return _mapper.Map<ConfigTableDTO?>(await _unitOfWork.ConfigTableRepo.GetConfigTableByNameAsync("Cài Đặt"));
        }

        public async Task<string?> UpdateConfig(NewConfigDTO configDTO)
        {
            try
            {
                var check = await _unitOfWork.ConfigTableRepo.GetConfigTableByNameAsync("Cài Đặt");
                if (check == null)
                {
                    return "Không tìm thấy cấu hình!";
                }
                check.TimeDelay = configDTO.TimeDelay != null ? configDTO.TimeDelay : check.TimeDelay;
                check.TimeHold = configDTO.TimeHold != null ? configDTO.TimeHold : check.TimeHold;
                check.Deposit = configDTO.Deposit != null ? configDTO.Deposit : check.Deposit;
                DateTime utcNow = DateTime.UtcNow;
                TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                check.UpdateDate = TimeZoneInfo.ConvertTimeFromUtc(utcNow, localTimeZone);
                _unitOfWork.ConfigTableRepo.Update(check);
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
