using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface IBilliardTableRepo : IGenericRepo<BilliardTable>
    {
        Task<BilliardTable?> GetBidaTableByID(Guid id);  

        Task<bool> CheckDuplicateName(Guid? id, string name, Guid? storeID);

        Task<Booking?> CheckTableBooking(Guid id, DateTime dateTime);

        Task<int> CountTableTypeArea(Guid? typeID, Guid? areaID, Guid? storeID);

        Task<decimal?> GetPriceOfTable(Guid? id);

        Task<IEnumerable<BilliardTable>?> GetAllBidaTable();

        Task<List<BilliardTable>?> GetAllBidaTableForRecurring(Guid? StoreId, Guid? AreaId, Guid? BilliardTypeId);
    }
}
