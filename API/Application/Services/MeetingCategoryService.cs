using API.Application.Common;
using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Services;

public class MeetingCategoryService : IMeetingCategoryService
{
    private readonly DataContext _context;
    private readonly string _language;

    public MeetingCategoryService(DataContext context, IRequestContext requestContext)
    {
        _context = context;
        _language = requestContext.Language;
    }

    public async Task<ServiceResult<List<MeetingCategoryDto>>> GetAllAsync()
    {
        var categories = await _context.MeetingCategories
            .AsNoTracking()
            .Select(x => MapToDto(x))
            .ToListAsync();

        return ServiceResult<List<MeetingCategoryDto>>.Success(
            data: categories,
            title: _language == "ar" ? "تم جلب التصنيفات" : "Meeting categories loaded",
            detail: _language == "ar" ? "تم جلب تصنيفات الاجتماعات بنجاح." : "Meeting categories retrieved successfully."
        );
    }

    public async Task<ServiceResult<MeetingCategoryDto>> GetByIdAsync(int id)
    {
        var category = await _context.MeetingCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (category is null)
        {
            return ServiceResult<MeetingCategoryDto>.Failure(
                title: _language == "ar" ? "غير موجود" : "Not Found",
                detail: _language == "ar" ? "تصنيف الاجتماع غير موجود." : "Meeting category was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        return ServiceResult<MeetingCategoryDto>.Success(
            data: MapToDto(category),
            title: _language == "ar" ? "تم جلب التصنيف" : "Meeting category loaded",
            detail: _language == "ar" ? "تم جلب تصنيف الاجتماع بنجاح." : "Meeting category retrieved successfully."
        );
    }

    public async Task<ServiceResult<MeetingCategoryDto>> CreateAsync(UpsertMeetingCategoryDto payload)
    {
        var duplicate = await EnsureCodeUniqueAsync(payload.Code);
        if (duplicate is not null)
        {
            return duplicate;
        }

        var category = new MeetingCategory
        {
            Code = payload.Code.Trim(),
            Name = payload.Name.Trim(),
            NameAr = payload.NameAr.Trim(),
            Description = payload.Description.Trim(),
            DescriptionAr = payload.DescriptionAr.Trim(),
        };

        _context.MeetingCategories.Add(category);
        await _context.SaveChangesAsync();

        return ServiceResult<MeetingCategoryDto>.Success(
            data: MapToDto(category),
            title: _language == "ar" ? "تم الإنشاء بنجاح" : "Created successfully",
            detail: _language == "ar" ? "تم إنشاء تصنيف الاجتماع بنجاح." : "Meeting category created successfully.",
            statusCode: StatusCodes.Status201Created
        );
    }

    public async Task<ServiceResult<MeetingCategoryDto>> UpdateAsync(int id, UpsertMeetingCategoryDto payload)
    {
        var category = await _context.MeetingCategories.FirstOrDefaultAsync(x => x.Id == id);
        if (category is null)
        {
            return ServiceResult<MeetingCategoryDto>.Failure(
                title: _language == "ar" ? "غير موجود" : "Not Found",
                detail: _language == "ar" ? "تصنيف الاجتماع غير موجود." : "Meeting category was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        var duplicate = await EnsureCodeUniqueAsync(payload.Code, id);
        if (duplicate is not null)
        {
            return duplicate;
        }

        category.Code = payload.Code.Trim();
        category.Name = payload.Name.Trim();
        category.NameAr = payload.NameAr.Trim();
        category.Description = payload.Description.Trim();
        category.DescriptionAr = payload.DescriptionAr.Trim();
        category.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        return ServiceResult<MeetingCategoryDto>.Success(
            data: MapToDto(category),
            title: _language == "ar" ? "تم التحديث بنجاح" : "Updated successfully",
            detail: _language == "ar" ? "تم تحديث تصنيف الاجتماع بنجاح." : "Meeting category updated successfully."
        );
    }

    public async Task<ServiceResult<string>> DeleteAsync(int id)
    {
        var category = await _context.MeetingCategories.FirstOrDefaultAsync(x => x.Id == id);
        if (category is null)
        {
            return ServiceResult<string>.Failure(
                title: _language == "ar" ? "غير موجود" : "Not Found",
                detail: _language == "ar" ? "تصنيف الاجتماع غير موجود." : "Meeting category was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        _context.MeetingCategories.Remove(category);
        await _context.SaveChangesAsync();

        return ServiceResult<string>.Success(
            data: "deleted",
            title: _language == "ar" ? "تم الحذف بنجاح" : "Deleted successfully",
            detail: _language == "ar" ? "تم حذف تصنيف الاجتماع بنجاح." : "Meeting category deleted successfully."
        );
    }

    private async Task<ServiceResult<MeetingCategoryDto>?> EnsureCodeUniqueAsync(string code, int? excludeId = null)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();

        var exists = await _context.MeetingCategories.AnyAsync(x =>
            x.Code.ToUpper() == normalizedCode &&
            (!excludeId.HasValue || x.Id != excludeId.Value));

        if (exists)
        {
            return ServiceResult<MeetingCategoryDto>.Failure(
                title: _language == "ar" ? "تعارض في البيانات" : "Conflict",
                detail: _language == "ar"
                    ? "كود تصنيف الاجتماع مستخدم بالفعل."
                    : "Meeting category code already exists.",
                statusCode: StatusCodes.Status409Conflict
            );
        }

        return null;
    }

    private static MeetingCategoryDto MapToDto(MeetingCategory entity)
    {
        return new MeetingCategoryDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            NameAr = entity.NameAr,
            Description = entity.Description,
            DescriptionAr = entity.DescriptionAr,
        };
    }
}
