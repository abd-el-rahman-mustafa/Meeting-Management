using API.Application.Common;
using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Services;

public class MeetingSettingsService : IMeetingSettingsService
{
    private readonly DataContext _context;
    private readonly string _language;

    public MeetingSettingsService(DataContext context, IRequestContext requestContext)
    {
        _context = context;
        _language = requestContext.Language;
    }

    public async Task<ServiceResult<MeetingSettingsDto>> GetAsync()
    {
        var settings = await GetOrCreateSettingsAsync();

        return ServiceResult<MeetingSettingsDto>.Success(
            data: MapToDto(settings),
            title: _language == "ar" ? "تم جلب الإعدادات" : "Meeting settings loaded",
            detail: _language == "ar"
                ? "تم جلب إعدادات الاجتماع بنجاح."
                : "Meeting settings retrieved successfully."
        );
    }

    public async Task<ServiceResult<MeetingSettingsDto>> UpsertAsync(UpsertMeetingSettingsDto payload)
    {
        if (!IsValid(payload))
        {
            return ServiceResult<MeetingSettingsDto>.BadRequest(
                _language == "ar"
                    ? "القيم يجب أن تكون أكبر من أو تساوي 1."
                    : "All required counts must be greater than or equal to 1.");
        }

        var settings = await GetOrCreateSettingsAsync();

        settings.FirstSessionOccurrenceRequiredManagementMembersCount = payload.FirstSessionOccurrenceRequiredManagementMembersCount;
        settings.SecondSessionOccurrenceRequiredManagementMembersCount = payload.SecondSessionOccurrenceRequiredManagementMembersCount;
        settings.ThirdSessionOccurrenceRequiredManagementMembersCount = payload.ThirdSessionOccurrenceRequiredManagementMembersCount;
        settings.FirstSessionOccurrenceRequiredMembersCount = payload.FirstSessionOccurrenceRequiredMembersCount;
        settings.SecondSessionOccurrenceRequiredMembersCount = payload.SecondSessionOccurrenceRequiredMembersCount;
        settings.ThirdSessionOccurrenceRequiredMembersCount = payload.ThirdSessionOccurrenceRequiredMembersCount;

        await _context.SaveChangesAsync();

        return ServiceResult<MeetingSettingsDto>.Success(
            data: MapToDto(settings),
            title: _language == "ar" ? "تم حفظ الإعدادات" : "Meeting settings saved",
            detail: _language == "ar"
                ? "تم حفظ إعدادات الاجتماع بنجاح."
                : "Meeting settings saved successfully."
        );
    }

    private async Task<MeetingSettings> GetOrCreateSettingsAsync()
    {
        var settings = await _context.MeetingSettings.FirstOrDefaultAsync();
        if (settings is not null)
        {
            return settings;
        }

        settings = new MeetingSettings();
        _context.MeetingSettings.Add(settings);
        await _context.SaveChangesAsync();
        return settings;
    }

    private static bool IsValid(UpsertMeetingSettingsDto payload)
    {
        return payload.FirstSessionOccurrenceRequiredManagementMembersCount >= 1
               && payload.SecondSessionOccurrenceRequiredManagementMembersCount >= 1
               && payload.ThirdSessionOccurrenceRequiredManagementMembersCount >= 1
               && payload.FirstSessionOccurrenceRequiredMembersCount >= 1
               && payload.SecondSessionOccurrenceRequiredMembersCount >= 1
               && payload.ThirdSessionOccurrenceRequiredMembersCount >= 1;
    }

    private static MeetingSettingsDto MapToDto(MeetingSettings settings)
    {
        return new MeetingSettingsDto
        {
            FirstSessionOccurrenceRequiredManagementMembersCount = settings.FirstSessionOccurrenceRequiredManagementMembersCount,
            SecondSessionOccurrenceRequiredManagementMembersCount = settings.SecondSessionOccurrenceRequiredManagementMembersCount,
            ThirdSessionOccurrenceRequiredManagementMembersCount = settings.ThirdSessionOccurrenceRequiredManagementMembersCount,
            FirstSessionOccurrenceRequiredMembersCount = settings.FirstSessionOccurrenceRequiredMembersCount,
            SecondSessionOccurrenceRequiredMembersCount = settings.SecondSessionOccurrenceRequiredMembersCount,
            ThirdSessionOccurrenceRequiredMembersCount = settings.ThirdSessionOccurrenceRequiredMembersCount
        };
    }
}
