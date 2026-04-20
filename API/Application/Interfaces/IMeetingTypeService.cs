using API.Application.Common;
using API.Application.DTOs;

namespace API.Application.Interfaces;

public interface IMeetingTypeService
{
    Task<ServiceResult<List<MeetingTypeDto>>> GetAllAsync();
    Task<ServiceResult<MeetingTypeDto>> GetByIdAsync(int id);
    Task<ServiceResult<MeetingTypeDto>> CreateAsync(UpsertMeetingTypeDto payload);
    Task<ServiceResult<MeetingTypeDto>> UpdateAsync(int id, UpsertMeetingTypeDto payload);
    Task<ServiceResult<string>> DeleteAsync(int id);
}
