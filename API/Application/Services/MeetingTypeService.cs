using API.Application.Common;
using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Services;

public class MeetingTypeService : IMeetingTypeService
{
    private readonly DataContext _context;
    private readonly string _language;

    public MeetingTypeService(DataContext context, IRequestContext requestContext)
    {
        _context = context;
        _language = requestContext.Language;
    }

    public async Task<ServiceResult<List<MeetingTypeDto>>> GetAllAsync()
    {
        var types = await _context.MeetingTypes
            .AsNoTracking()
             .OrderByDescending(x => x.CreatedAt)
            .Select(x => MapToDto(x))
            .ToListAsync();

        return ServiceResult<List<MeetingTypeDto>>.Success(
            data: types,
            title: _language == "ar" ? "تم جلب أنواع الاجتماعات" : "Meeting types loaded",
            detail: _language == "ar" ? "تم جلب أنواع الاجتماعات بنجاح." : "Meeting types retrieved successfully."
        );
    }

    public async Task<ServiceResult<MeetingTypeDto>> GetByIdAsync(int id)
    {
        var types = await _context.MeetingTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (types is null)
        {
            return ServiceResult<MeetingTypeDto>.Failure(
                title: _language == "ar" ? "غير موجود" : "Not Found",
                detail: _language == "ar" ? "نوع الاجتماع غير موجود." : "Meeting type was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        return ServiceResult<MeetingTypeDto>.Success(
            data: MapToDto(types),
            title: _language == "ar" ? "تم جلب نوع الاجتماع" : "Meeting type loaded",
            detail: _language == "ar" ? "تم جلب نوع الاجتماع بنجاح." : "Meeting type retrieved successfully."
        );
    }

    public async Task<ServiceResult<MeetingTypeDto>> CreateAsync(UpsertMeetingTypeDto payload)
    {
        var duplicate = await EnsureNameUniqueAsync(payload.Name);
        if (duplicate is not null)
        {
            return duplicate;
        }

        var types = new MeetingType
        {
            Name = payload.Name.Trim(),
        };

        _context.MeetingTypes.Add(types);
        await _context.SaveChangesAsync();

        return ServiceResult<MeetingTypeDto>.Success(
            data: MapToDto(types),
            title: _language == "ar" ? "تم الإنشاء بنجاح" : "Created successfully",
                detail: _language == "ar" ? "تم إنشاء نوع الاجتماع بنجاح." : "Meeting type created successfully.",
            statusCode: StatusCodes.Status201Created
        );
    }

    public async Task<ServiceResult<MeetingTypeDto>> UpdateAsync(int id, UpsertMeetingTypeDto payload)
    {
        var types = await _context.MeetingTypes.FirstOrDefaultAsync(x => x.Id == id);
        if (types is null)
        {
            return ServiceResult<MeetingTypeDto>.Failure(
                title: _language == "ar" ? "غير موجود" : "Not Found",
                detail: _language == "ar" ? "نوع الاجتماع غير موجود." : "Meeting type was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        var duplicate = await EnsureNameUniqueAsync(payload.Name, id);
        if (duplicate is not null)
        {
            return duplicate;
        }

        types.Name = payload.Name.Trim();
        types.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return ServiceResult<MeetingTypeDto>.Success(
            data: MapToDto(types),
            title: _language == "ar" ? "تم التحديث بنجاح" : "Updated successfully",
            detail: _language == "ar" ? "تم تحديث نوع الاجتماع بنجاح." : "Meeting type updated successfully."
        );
    }

    public async Task<ServiceResult<string>> DeleteAsync(int id)
    {
        var types = await _context.MeetingTypes.FirstOrDefaultAsync(x => x.Id == id);
        if (types is null)
        {
            return ServiceResult<string>.Failure(
                title: _language == "ar" ? "غير موجود" : "Not Found",
                detail: _language == "ar" ? "نوع الاجتماع غير موجود." : "Meeting type was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        _context.MeetingTypes.Remove(types);
        await _context.SaveChangesAsync();

        return ServiceResult<string>.Success(
            data: "deleted",
            title: _language == "ar" ? "تم الحذف بنجاح" : "Deleted successfully",
            detail: _language == "ar" ? "تم حذف نوع الاجتماع بنجاح." : "Meeting type deleted successfully."
        );
    }

    private async Task<ServiceResult<MeetingTypeDto>?> EnsureNameUniqueAsync(string name, int? excludeId = null)
    {
        var normalizedCode = name.Trim().ToUpperInvariant();

        var exists = await _context.MeetingTypes.AnyAsync(x =>
            x.Name.ToUpper() == normalizedCode &&
            (!excludeId.HasValue || x.Id != excludeId.Value));

        if (exists)
        {
            return ServiceResult<MeetingTypeDto>.Failure(
                title: _language == "ar" ? "تعارض في البيانات" : "Conflict",
                detail: _language == "ar"
                    ? " نوع الاجتماع مستخدم بالفعل."
                    : "Meeting type name already exists.",
                statusCode: StatusCodes.Status409Conflict
            );
        }

        return null;
    }

    private static MeetingTypeDto MapToDto(MeetingType entity)
    {
        return new MeetingTypeDto
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }
}
