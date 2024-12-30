using PoolLab.Application.ModelDTO;
using PoolLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Interface
{
    public interface IPlaytimeService 
    {
        Task<string?> AddNewPlaytime(AddNewPlayTimeDTO addNewPlayTimeDTO);

        Task<string?> StopPlayTime(StopTimeDTO timeDTO);

        Task<PlaytimeDTO> GetPlaytimeByTableIdOrId(Guid id);
    }
}
