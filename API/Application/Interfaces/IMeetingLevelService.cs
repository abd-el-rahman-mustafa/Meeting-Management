using API.Application.Common;
using API.Application.DTOs;

namespace API.Application.Interfaces;

public interface IMeetingLevelService
{
    Task<ServiceResult<List<MeetingLevelDto>>> GetAllAsync();
    Task<ServiceResult<MeetingLevelDto>> GetByIdAsync(int id);
    Task<ServiceResult<MeetingLevelDto>> CreateAsync(UpsertMeetingLevelDto payload);
    Task<ServiceResult<MeetingLevelDto>> UpdateAsync(int id, UpsertMeetingLevelDto payload);
    Task<ServiceResult<string>> DeleteAsync(int id);
}
