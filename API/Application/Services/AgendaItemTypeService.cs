using API.Application.Common;
using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Services;

public class AgendaItemTypeService : IAgendaItemTypeService
{
    private readonly DataContext _context;
    private readonly string _language;

    public AgendaItemTypeService(DataContext context, IRequestContext requestContext)
    {
        _context = context;
        _language = requestContext.Language;
    }

    public async Task<ServiceResult<List<AgendaItemTypeDto>>> GetAllAsync()
    {
        var types = await _context.AgendaItemTypes
            .AsNoTracking()
             .OrderByDescending(x => x.CreatedAt)
            .Select(x => MapToDto(x))
            .ToListAsync();

        return ServiceResult<List<AgendaItemTypeDto>>.Success(
            data: types,
            title: _language == "ar" ? "تم جلب أنواع عناصر الأجندة" : "Agenda item types loaded",
            detail: _language == "ar" ? "تم جلب أنواع عناصر الأجندة بنجاح." : "Agenda item types retrieved successfully."
        );
    }

    public async Task<ServiceResult<AgendaItemTypeDto>> GetByIdAsync(int id)
    {
        var types = await _context.AgendaItemTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (types is null)
        {
            return ServiceResult<AgendaItemTypeDto>.Failure(
                title: _language == "ar" ? "غير موجود" : "Not Found",
                detail: _language == "ar" ? "نوع عنصر الأجندة غير موجود." : "Agenda item type was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        return ServiceResult<AgendaItemTypeDto>.Success(
            data: MapToDto(types),
            title: _language == "ar" ? "تم جلب نوع عنصر الأجندة" : "Agenda item type loaded",
            detail: _language == "ar" ? "تم جلب نوع عنصر الأجندة بنجاح." : "Agenda item type retrieved successfully."
        );
    }

    public async Task<ServiceResult<AgendaItemTypeDto>> CreateAsync(UpsertAgendaItemTypeDto payload)
    {
        var duplicate = await EnsureNameUniqueAsync(payload.Name);
        if (duplicate is not null)
        {
            return duplicate;
        }

        var types = new AgendaItemType
        {
            Name = payload.Name.Trim(),
            Description = payload.Description.Trim(),
        };

        _context.AgendaItemTypes.Add(types);
        await _context.SaveChangesAsync();

        return ServiceResult<AgendaItemTypeDto>.Success(
            data: MapToDto(types),
            title: _language == "ar" ? "تم الإنشاء بنجاح" : "Created successfully",
                detail: _language == "ar" ? "تم إنشاء نوع عنصر الأجندة بنجاح." : "Agenda item type created successfully.",
            statusCode: StatusCodes.Status201Created
        );
    }

    public async Task<ServiceResult<AgendaItemTypeDto>> UpdateAsync(int id, UpsertAgendaItemTypeDto payload)
    {
        var types = await _context.AgendaItemTypes.FirstOrDefaultAsync(x => x.Id == id);
        if (types is null)
        {
            return ServiceResult<AgendaItemTypeDto>.Failure(
                title: _language == "ar" ? "غير موجود" : "Not Found",
                detail: _language == "ar" ? "نوع عنصر الأجندة غير موجود." : "Agenda item type was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        var duplicate = await EnsureNameUniqueAsync(payload.Name, id);
        if (duplicate is not null)
        {
            return duplicate;
        }

        types.Name = payload.Name.Trim();
        types.Description = payload.Description.Trim();
        types.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return ServiceResult<AgendaItemTypeDto>.Success(
            data: MapToDto(types),
            title: _language == "ar" ? "تم التحديث بنجاح" : "Updated successfully",
            detail: _language == "ar" ? "تم تحديث نوع عنصر الأجندة بنجاح." : "Agenda item type updated successfully."
        );
    }

    public async Task<ServiceResult<string>> DeleteAsync(int id)
    {
        var types = await _context.AgendaItemTypes.FirstOrDefaultAsync(x => x.Id == id);
        if (types is null)
        {
            return ServiceResult<string>.Failure(
                title: _language == "ar" ? "غير موجود" : "Not Found",
                detail: _language == "ar" ? "نوع عنصر الأجندة غير موجود." : "Agenda item type was not found.",
                statusCode: StatusCodes.Status404NotFound
            );
        }

        _context.AgendaItemTypes.Remove(types);
        await _context.SaveChangesAsync();

        return ServiceResult<string>.Success(
            data: "deleted",
            title: _language == "ar" ? "تم الحذف بنجاح" : "Deleted successfully",
            detail: _language == "ar" ? "تم حذف نوع عنصر الأجندة بنجاح." : "Agenda item type deleted successfully."
        );
    }

    private async Task<ServiceResult<AgendaItemTypeDto>?> EnsureNameUniqueAsync(string name, int? excludeId = null)
    {
        var normalizedCode = name.Trim().ToUpperInvariant();

        var exists = await _context.AgendaItemTypes.AnyAsync(x =>
            x.Name.ToUpper() == normalizedCode &&
            (!excludeId.HasValue || x.Id != excludeId.Value));

        if (exists)
        {
            return ServiceResult<AgendaItemTypeDto>.Failure(
                title: _language == "ar" ? "تعارض في البيانات" : "Conflict",
                detail: _language == "ar"
                    ? " نوع عنصر الأجندة مستخدم بالفعل."
                    : "Agenda item type name already exists.",
                statusCode: StatusCodes.Status409Conflict
            );
        }

        return null;
    }

    private static AgendaItemTypeDto MapToDto(AgendaItemType entity)
    {
        return new AgendaItemTypeDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
        };
    }
}