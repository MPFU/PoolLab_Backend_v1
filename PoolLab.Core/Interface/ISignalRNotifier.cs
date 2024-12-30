using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Core.Interface
{
    public interface ISignalRNotifier
    {
        Task NotifyTableActivationAsync(Guid storeId, string message);
    }
}
