
namespace API.Domain.Entities;
public abstract class BaseEntity : SimpleBaseEntity
{
    public BaseEntity()
    {
        UpdatedAt = DateTime.Now;
    }
    public int? CreatedById { get; set; }
    public AppUser? CreatedBy { get; set; }
    public int? UpdatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
