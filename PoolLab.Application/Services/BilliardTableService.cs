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
        private readonly IQRCodeGenerate _qRCodeGenerate;
        private readonly IAzureBlobService _azureBlobService;

        public BilliardTableService(IMapper mapper, IUnitOfWork unitOfWork, IQRCodeGenerate qRCodeGenerate, IAzureBlobService azureBlobService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _qRCodeGenerate = qRCodeGenerate;
            _azureBlobService = azureBlobService;
        }

        public async Task<string?> AddNewTable(NewBilliardTableDTO newBilliardTableDTO)
        {
            try
            {
                var bida = _mapper.Map<BilliardTable>(newBilliardTableDTO);
                bida.Id = Guid.NewGuid();
                bida.CreatedDate = DateTime.UtcNow;
                bida.Status = "Bàn trống";
                var qrcode = await _qRCodeGenerate.GenerateQRCode(bida.Id);
                if (qrcode == null)
                {
                    return "Tạo QRCode thất bại!";
                }
                bida.Qrcode = await _azureBlobService.UploadQRCodeImageAsync("qrcode",qrcode);
                if (bida.Qrcode == null)
                {
                    return "Tệp tải lên thất bại!";
                }
                await _unitOfWork.BilliardTableRepo.AddAsync(bida);
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
    }
}
