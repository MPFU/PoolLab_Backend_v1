using PoolLab.Application.FilterModel;
using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IBilliardTableService 
    {
        Task<string?> AddNewTable ( NewBilliardTableDTO newBilliardTableDTO );

        Task<GetBilliardTableDTO?> GetBilliardTableByID(Guid Id);

        Task<PageResult<GetAllTableDTO>> GetAllBidaTable(BidaTableFilter bidaTableFilter);

        Task<string?> UpdateInfoTable(Guid Id, UpdateInfoTableDTO updateInfoTableDTO);

        Task<string?> UpdateStatusTable(Guid Id, UpdateStatusTableDTO statusTableDTO);

        Task<string?> DeleteTable(Guid Id);

        Task<string?> ActivateTable(ActiveTable activeTable);

        Task<string?> ActivateTableForGuest(Guid activeTable, ActiveTableForGuest activeTableFor);
    }
}
