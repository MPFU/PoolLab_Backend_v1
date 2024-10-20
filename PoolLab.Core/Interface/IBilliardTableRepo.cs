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

        Task<string?> CheckTableAvailable(Guid id, Guid? CusID, DateTime dateTime);


    }
}
