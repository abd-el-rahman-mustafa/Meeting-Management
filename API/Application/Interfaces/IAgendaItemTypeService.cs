using API.Application.Common;
using API.Application.DTOs;

namespace API.Application.Interfaces;

public interface IAgendaItemTypeService
{
    Task<ServiceResult<List<AgendaItemTypeDto>>> GetAllAsync();
    Task<ServiceResult<AgendaItemTypeDto>> GetByIdAsync(int id);
    Task<ServiceResult<AgendaItemTypeDto>> CreateAsync(UpsertAgendaItemTypeDto payload);
    Task<ServiceResult<AgendaItemTypeDto>> UpdateAsync(int id, UpsertAgendaItemTypeDto payload);
    Task<ServiceResult<string>> DeleteAsync(int id);
}