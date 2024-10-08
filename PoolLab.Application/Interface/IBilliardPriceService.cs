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
    public interface IBilliardPriceService
    {
        Task<string?> AddNewBidaPrice(NewBilliardPriceDTO newBilliard);
        Task<string?> DeleteBidaPrice(Guid Id);
        Task<BilliardPriceDTO?> GetBidaPriceById(Guid id);
        Task<IEnumerable<BilliardPriceDTO>?> GetAllBidaPrice(BidaPriceFilter bidaPriceFilter);
        Task<string?> UpdateBilliardPrice(Guid Id, NewBilliardPriceDTO newBilliardPrice);
        Task<string?> UpdateBilliardPriceStatus(Guid id, string status);
    }
}
