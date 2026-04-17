namespace API.Domain.Entities;

public class Organization : BaseEntity
{
    public string Name { get; set; } = null!; // e.g. "Acme Corporation"
    public string NameAr { get; set; } = null!; // e.g. "شركة أكمي"
    public string ContactEmail { get; set; } = null!; // e.g. "Acme@gmail.com"
    public string ContactPhone { get; set; } = null!; // e.g. "+1-555-123-4567"
}
