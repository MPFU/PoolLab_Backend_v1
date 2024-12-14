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
    public interface IEventService 
    {
        Task<string?> AddNewEvent(CreateEventDTO newEventDTO);
        Task<string?> DeleteEvent(Guid Id);
        Task<GetEventDTO?> GetEventById(Guid id);
        Task<PageResult<GetEventDTO>?> GetAllEvent(EventFilter eventFilter);
        Task<string?> UpdateEventInfo(Guid Id, CreateEventDTO newAreaDTO);
        Task<string?> UpdateStatusEvent(Guid Id, UpdateStatusEventDTO eventDTO);
    }
}
