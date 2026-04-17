namespace API.Domain.Entities;

public class MeetingObserver : BaseEntity
{
    // Name
    public string Name { get; set; } = null!; // e.g. "John Doe"
    public string Email { get; set; } = null!; // e.g. " Johne@gmail.com"
    public string PhoneNumber { get; set; } = null!; // e.g. "+1-555-123-4567"
    
    public int MeetingId { get; set; }
    public Meeting Meeting { get; set; } = null!;
    
    public int? OrganizationId { get; set; }
    public Organization? Organization { get; set; }
    
}