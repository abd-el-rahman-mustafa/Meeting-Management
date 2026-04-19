using API.Application.Common;
using API.Application.DTOs;

namespace API.Application.Interfaces;

public interface IMeetingCategoryService
{
    Task<ServiceResult<List<MeetingCategoryDto>>> GetAllAsync();
    Task<ServiceResult<MeetingCategoryDto>> GetByIdAsync(int id);
    Task<ServiceResult<MeetingCategoryDto>> CreateAsync(UpsertMeetingCategoryDto payload);
    Task<ServiceResult<MeetingCategoryDto>> UpdateAsync(int id, UpsertMeetingCategoryDto payload);
    Task<ServiceResult<string>> DeleteAsync(int id);
}
