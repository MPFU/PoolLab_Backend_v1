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
    public class BilliardTableService : IBilliardTableService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BilliardTableService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string?> AddNewTable(NewBilliardTableDTO newBilliardTableDTO)
        {
            try
            {
                var bida = _mapper.Map<BilliardTable>(newBilliardTableDTO);
                bida.Id = Guid.NewGuid();
                bida.CreatedDate = DateTime.UtcNow;
                bida.Status = "Bàn trống";
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
    }
}
