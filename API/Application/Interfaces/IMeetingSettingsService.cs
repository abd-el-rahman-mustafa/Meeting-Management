using API.Application.Common;
using API.Application.DTOs;

namespace API.Application.Interfaces;

public interface IMeetingSettingsService
{
    Task<ServiceResult<MeetingSettingsDto>> GetAsync();
    Task<ServiceResult<MeetingSettingsDto>> UpsertAsync(UpsertMeetingSettingsDto payload);
}
