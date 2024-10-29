using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface IBidaTypeAreRepo : IGenericRepo<BilliardTypeArea>
    {
        Task<bool> CheckDuplicate(Guid? areaID, Guid? bidaTye);

        Task<IEnumerable<BilliardTypeArea>?> GetAllBidaTypeAre();

        Task<BilliardTypeArea?> GetBidaTypeArea(Guid? typeID, Guid? areaID, Guid? storeID);
    }
}
