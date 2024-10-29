using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
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
        private readonly IBidaTypeAreaService _bidaTypeAreaService;
        private readonly IConfigTableService _configTableService;
        private readonly IBookingService _bookingService;

        public BilliardTableService(IMapper mapper, IUnitOfWork unitOfWork, IQRCodeGenerate qRCodeGenerate, IAzureBlobService azureBlobService, IBidaTypeAreaService bidaTypeAreaService, IConfigTableService configTableService, IBookingService bookingService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _qRCodeGenerate = qRCodeGenerate;
            _azureBlobService = azureBlobService;
            _bidaTypeAreaService = bidaTypeAreaService;
            _configTableService = configTableService;
            _bookingService = bookingService;
        }


        public async Task<string?> ActivateTable(ActiveTable activeTable)
        {
            try
            {
                var now = DateTime.Now;
                var Time = TimeOnly.FromDateTime(now);
                var check = await _unitOfWork.BilliardTableRepo.CheckTableAvailable(activeTable.BilliardTableID, now);
                if (check != null)
                {
                    var config = await _configTableService.GetConfigTableByName();
                    if (config == null)
                    {
                        return "Không tìm thấy cấu hình!";
                    }
                    if (check.CustomerId == activeTable.CustomerID)
                    {
                        if (Time >= check.TimeStart && Time <= check.TimeStart.Value.AddMinutes((double)config.TimeDelay))
                        {
                            var book = await _bookingService.UpdateStatusBooking(check.Id, "Hoàn Thành");
                            if (book != null)
                            {
                                return book;
                            }
                        }
                        else if (Time < check.TimeStart)
                        {
                            return "Bạn đến quá sớm, hãy quay lại khi tới giờ!";
                        }
                        else
                        {
                            var book = await _bookingService.UpdateStatusBooking(check.Id, "Đã Huỷ");
                            if (book != null)
                            {
                                return book;
                            }
                        }
                    }
                    else
                    {
                        if (Time <= check.TimeStart && Time >= check.TimeStart.Value.AddMinutes((double)-config.TimeHold))
                        {
                            return "Bàn chơi này đã được đặt trước!";
                        }
                        else
                        {
                            return check.TimeStart.ToString();
                        }
                    }
                }
                var table = await _unitOfWork.BilliardTableRepo.GetByIdAsync(activeTable.BilliardTableID);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }
                table.Status = "Có Khách";
                table.UpdatedDate = DateTime.Now;
                _unitOfWork.BilliardTableRepo.Update(table);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Kích hoạt thất bại!";
                }

                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> ActivateTableForGuest(Guid activeTable)
        {
            try
            {
                var now = DateTime.Now;
                var Time = TimeOnly.FromDateTime(now);
                var check = await _unitOfWork.BilliardTableRepo.CheckTableAvailable(activeTable, now);
                if(check != null)
                {
                    var config = await _configTableService.GetConfigTableByName();
                    if (config == null)
                    {
                        return "Không tìm thấy cấu hình!";
                    }

                    if (Time <= check.TimeStart && Time >= check.TimeStart.Value.AddMinutes((double)-config.TimeHold))
                    {
                        return "Bàn chơi này đã được đặt trước!";
                    }
                    else
                    { 
                        TimeOnly no = new TimeOnly(9,0);
                        TimeSpan timeOnly = (TimeSpan)(check.TimeStart - no);
                        return timeOnly.ToString(@"hh\:mm");
                    }
                }
                var table = await _unitOfWork.BilliardTableRepo.GetByIdAsync(activeTable);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }
                table.Status = "Có Khách";
                table.UpdatedDate = DateTime.Now;
                _unitOfWork.BilliardTableRepo.Update(table);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Kích hoạt thất bại!";
                }

                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<string?> AddNewTable(NewBilliardTableDTO newBilliardTableDTO)
        {
            try
            {
                if (!string.IsNullOrEmpty(newBilliardTableDTO.Name))
                {
                    var check = await _unitOfWork.BilliardTableRepo.CheckDuplicateName(null, newBilliardTableDTO.Name, newBilliardTableDTO.StoreId);
                    if (check)
                    {
                        return "Tên bàn chơi bị trùng!";
                    }
                }
                var bida = _mapper.Map<BilliardTable>(newBilliardTableDTO);
                bida.Id = Guid.NewGuid();
                bida.CreatedDate = DateTime.Now;
                bida.Status = "Bàn trống";
                NewTypeAreaDTO bidaTypeDTO = new NewTypeAreaDTO();
                bidaTypeDTO.AreaID = bida.AreaId;
                bidaTypeDTO.BilliardTypeID = bida.BilliardTypeId;
                bidaTypeDTO.StoreID = bida.StoreId;
                var dup = await _bidaTypeAreaService.AddNewBidaTypeArea(bidaTypeDTO);
                if (dup != null)
                {
                    return dup;
                }
                var qrcode = await _qRCodeGenerate.GenerateQRCode(bida.Id);
                if (qrcode == null)
                {
                    return "Tạo QRCode thất bại!";
                }
                bida.Qrcode = await _azureBlobService.UploadQRCodeImageAsync("qrcode", qrcode);
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

        public async Task<string?> DeleteTable(Guid Id)
        {
            try
            {
                var table = await _unitOfWork.BilliardTableRepo.GetByIdAsync(Id);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }
                var typeArea = await _bidaTypeAreaService.DeleteBidaTypeArea(table.BilliardTypeId, table.AreaId, table.StoreId);
                if (typeArea != null)
                {
                    return typeArea;
                }
                _unitOfWork.BilliardTableRepo.Delete(table);
                var result = await _unitOfWork.SaveAsync() > 0;
                if (!result)
                {
                    return "Xoá thất bại!";
                }
                return null;
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public async Task<PageResult<BilliardTableDTO>> GetAllBidaTable(BidaTableFilter bidaTableFilter)
        {
            var bidaList = _mapper.Map<IEnumerable<BilliardTableDTO>>(await _unitOfWork.BilliardTableRepo.GetAllAsync());
            IQueryable<BilliardTableDTO> query = bidaList.AsQueryable();

            //Filter
            if (!string.IsNullOrEmpty(bidaTableFilter.Name))
                query = query.Where(x => x.Name.Contains(bidaTableFilter.Name, StringComparison.OrdinalIgnoreCase));

            if (bidaTableFilter.StroreID != null)
                query = query.Where(x => x.StoreId.Equals(bidaTableFilter.StroreID));

            if (bidaTableFilter.AreaID != null)
                query = query.Where(x => x.AreaId.Equals(bidaTableFilter.AreaID));

            if (bidaTableFilter.BilliardTypeId != null)
                query = query.Where(x => x.BilliardTypeId.Equals(bidaTableFilter.BilliardTypeId));

            if (bidaTableFilter.PriceId != null)
                query = query.Where(x => x.PriceId.Equals(bidaTableFilter.PriceId));

            if (!string.IsNullOrEmpty(bidaTableFilter.Status))
                query = query.Where(x => x.Status.Contains(bidaTableFilter.Status, StringComparison.OrdinalIgnoreCase));

            //Sorting
            if (!string.IsNullOrEmpty(bidaTableFilter.SortBy))
            {
                switch (bidaTableFilter.SortBy)
                {
                    case "createdDate":
                        query = bidaTableFilter.SortAscending ?
                            query.OrderBy(x => x.CreatedDate) :
                            query.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "updatedDate":
                        query = bidaTableFilter.SortAscending ?
                            query.OrderBy(x => x.UpdatedDate) :
                            query.OrderByDescending(x => x.UpdatedDate);
                        break;
                }
            }

            //Paging
            var pageItems = query
                .Skip((bidaTableFilter.PageNumber - 1) * bidaTableFilter.PageSize)
                .Take(bidaTableFilter.PageSize)
                .ToList();

            return new PageResult<BilliardTableDTO>
            {
                Items = pageItems,
                PageNumber = bidaTableFilter.PageNumber,
                PageSize = bidaTableFilter.PageSize,
                TotalItem = query.Count(),
                TotalPages = (int)Math.Ceiling((decimal)query.Count() / (decimal)bidaTableFilter.PageSize)
            };
        }

        public async Task<GetBilliardTableDTO?> GetBilliardTableByID(Guid Id)
        {
            return _mapper.Map<GetBilliardTableDTO?>(await _unitOfWork.BilliardTableRepo.GetBidaTableByID(Id));
        }

        public async Task<string?> UpdateInfoTable(Guid Id, UpdateInfoTableDTO updateInfoTableDTO)
        {
            try
            {
                var table = await _unitOfWork.BilliardTableRepo.GetByIdAsync(Id);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }
                if (!string.IsNullOrEmpty(updateInfoTableDTO.Name))
                {
                    var check = await _unitOfWork.BilliardTableRepo.CheckDuplicateName(Id, updateInfoTableDTO.Name, table.StoreId);
                    if (check)
                    {
                        return "Tên bàn chơi bị trùng!";
                    }
                }
                table.Name = !string.IsNullOrEmpty(updateInfoTableDTO.Name) ? updateInfoTableDTO.Name : table.Name;
                table.Descript = !string.IsNullOrEmpty(updateInfoTableDTO.Descript) ? updateInfoTableDTO.Descript : table.Descript;
                table.Image = !string.IsNullOrEmpty(updateInfoTableDTO.Image) ? updateInfoTableDTO.Image : table.Image;
                table.AreaId = updateInfoTableDTO.AreaId != null && updateInfoTableDTO.AreaId != Guid.Empty ? updateInfoTableDTO.AreaId : table.AreaId;
                table.PriceId = updateInfoTableDTO.PriceId != null && updateInfoTableDTO.PriceId != Guid.Empty ? updateInfoTableDTO.PriceId : table.PriceId;
                table.BilliardTypeId = updateInfoTableDTO.BilliardTypeId != null && updateInfoTableDTO.BilliardTypeId != Guid.Empty ? updateInfoTableDTO.BilliardTypeId : table.AreaId;
                table.UpdatedDate = DateTime.Now;
                _unitOfWork.BilliardTableRepo.Update(table);
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

        public async Task<string?> UpdateStatusTable(Guid Id, UpdateStatusTableDTO statusTableDTO)
        {
            try
            {
                var table = await _unitOfWork.BilliardTableRepo.GetByIdAsync(Id);
                if (table == null)
                {
                    return "Không tìm thấy bàn chơi này!";
                }
                table.Status = statusTableDTO.Status;
                _unitOfWork.BilliardTableRepo.Update(table);
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
