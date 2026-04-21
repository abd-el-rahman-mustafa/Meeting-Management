using API.Application.Common;
using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Services;

public class MeetingLevelService : IMeetingLevelService
{
    private readonly DataContext _context;
    private readonly string _language;

    public MeetingLevelService(DataContext context, IRequestContext requestContext)
    {
        _context = context;
        _language = requestContext.Language;
    }

    public async Task<ServiceResult<List<MeetingLevelDto>>> GetAllAsync()
    {
        var levels = await _context.MeetingLevels
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => MapToDto(x))
            .ToListAsync();

        return ServiceResult<List<MeetingLevelDto>>.Success(
            data: levels,
            title: _language == "ar" ? "تم جلب مستويات الاجتماعات" : "Meeting levels loaded",
            detail: _language == "ar" ? "تم جلب مستويات الاجتماعات بنجاح." : "Meeting levels retrieved successfully."
        );
    }

    public async Task<ServiceResult<MeetingLevelDto>> GetByIdAsync(int id)
    {
        var level = await _context.MeetingLevels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (level is null)
        {
            return ServiceResult<MeetingLevelDto>.Failure(
                title: _language == "ar" ? "غير موجود" : "Not Found",
                detail: _language == "ar" ? "مستوى الاجتماع غير موجود." : "Meeting level was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        return ServiceResult<MeetingLevelDto>.Success(
            data: MapToDto(level),
            title: _language == "ar" ? "تم جلب مستوى الاجتماع" : "Meeting level loaded",
            detail: _language == "ar" ? "تم جلب مستوى الاجتماع بنجاح." : "Meeting level retrieved successfully."
        );
    }

    public async Task<ServiceResult<MeetingLevelDto>> CreateAsync(UpsertMeetingLevelDto payload)
    {
        var duplicate = await EnsureNameUniqueAsync(payload.Name);
        if (duplicate is not null)
        {
            return duplicate;
        }

        var level = new MeetingLevel
        {
            Name = payload.Name.Trim(),
            Description = payload.Description.Trim(),
        };

        _context.MeetingLevels.Add(level);
        await _context.SaveChangesAsync();

        return ServiceResult<MeetingLevelDto>.Success(
            data: MapToDto(level),
            title: _language == "ar" ? "تم الإنشاء بنجاح" : "Created successfully",
            detail: _language == "ar" ? "تم إنشاء مستوى الاجتماع بنجاح." : "Meeting level created successfully.",
            statusCode: StatusCodes.Status201Created
        );
    }

    public async Task<ServiceResult<MeetingLevelDto>> UpdateAsync(int id, UpsertMeetingLevelDto payload)
    {
        var level = await _context.MeetingLevels.FirstOrDefaultAsync(x => x.Id == id);
        if (level is null)
        {
            return ServiceResult<MeetingLevelDto>.Failure(
                title: _language == "ar" ? "غير موجود" : "Not Found",
                detail: _language == "ar" ? "مستوى الاجتماع غير موجود." : "Meeting level was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        var duplicate = await EnsureNameUniqueAsync(payload.Name, id);
        if (duplicate is not null)
        {
            return duplicate;
        }

        level.Name = payload.Name.Trim();
        level.Description = payload.Description.Trim();
        level.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return ServiceResult<MeetingLevelDto>.Success(
            data: MapToDto(level),
            title: _language == "ar" ? "تم التحديث بنجاح" : "Updated successfully",
            detail: _language == "ar" ? "تم تحديث مستوى الاجتماع بنجاح." : "Meeting level updated successfully."
        );
    }

    public async Task<ServiceResult<string>> DeleteAsync(int id)
    {
        var level = await _context.MeetingLevels.FirstOrDefaultAsync(x => x.Id == id);
        if (level is null)
        {
            return ServiceResult<string>.Failure(
                title: _language == "ar" ? "غير موجود" : "Not Found",
                detail: _language == "ar" ? "مستوى الاجتماع غير موجود." : "Meeting level was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        _context.MeetingLevels.Remove(level);
        await _context.SaveChangesAsync();

        return ServiceResult<string>.Success(
            data: "deleted",
            title: _language == "ar" ? "تم الحذف بنجاح" : "Deleted successfully",
            detail: _language == "ar" ? "تم حذف مستوى الاجتماع بنجاح." : "Meeting level deleted successfully."
        );
    }

    private async Task<ServiceResult<MeetingLevelDto>?> EnsureNameUniqueAsync(string name, int? excludeId = null)
    {
        var normalizedCode = name.Trim().ToUpperInvariant();

        var exists = await _context.MeetingLevels.AnyAsync(x =>
            x.Name.ToUpper() == normalizedCode &&
            (!excludeId.HasValue || x.Id != excludeId.Value));

        if (exists)
        {
            return ServiceResult<MeetingLevelDto>.Failure(
                title: _language == "ar" ? "تعارض في البيانات" : "Conflict",
                detail: _language == "ar"
                    ? " مستوى الاجتماع مستخدم بالفعل."
                    : "Meeting level name already exists.",
                statusCode: StatusCodes.Status409Conflict
            );
        }

        return null;
    }

    private static MeetingLevelDto MapToDto(MeetingLevel entity)
    {
        return new MeetingLevelDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
        };
    }
}
