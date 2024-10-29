using PoolLab.Application.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IBidaTypeAreaService
    {
        Task<string?> AddNewBidaTypeArea(NewTypeAreaDTO newType );

        Task<IEnumerable<GetBidaTypeAreaDTO>?> GetAllBidaTypeAre(BidaTypeAreFilter filter);

        Task<string?> DeleteBidaTypeArea(Guid? typeID, Guid? areaID, Guid? storeID);
    }
}
