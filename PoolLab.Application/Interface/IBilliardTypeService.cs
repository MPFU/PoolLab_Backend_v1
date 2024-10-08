using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IBilliardTypeService 
    {
        Task<string?> AddNewBidaType(NewBidaTypeDTO newBidaTypeDTO);
        Task<string?> DeleteBidaType(Guid Id);
        Task<BidaTypeDTO?> GetBidaTypeById(Guid id);
        Task<IEnumerable<BidaTypeDTO?>> GetAllBidaType();
        Task<string?> UpdateBidaTye(Guid Id, NewBidaTypeDTO newBidaTypeDTO);
    }
}
