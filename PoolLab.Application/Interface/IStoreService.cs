using PoolLab.Application.FilterModel.Helper;
using PoolLab.Application.FilterModel;
using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IStoreService 
    {
        Task<string?> AddNewStore(NewStoreDTO newStoreDTO);
        Task<string?> DeleteStore(Guid Id);
        Task<StoreDTO?> GetStoreById(Guid id);
        Task<PageResult<StoreDTO>?> GetAllStore(StoreFilter storeFilter);
        Task<string?> UpdateStore(Guid Id, NewStoreDTO newStore);
        Task<string?> UpdateStoreRating(Guid Id, int rate);
        Task<string?> CheckStoreIsActive(Guid Id);
    }
}
