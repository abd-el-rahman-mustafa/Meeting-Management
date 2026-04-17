namespace API.Domain.Entities;

public class MeetingAttendee : BaseEntity
{
    
   public string Name { get; set; } = null!; // e.g. "John Doe"
    public string Email { get; set; } = null!; // e.g. " Johne@gmail.com"
    public string PhoneNumber { get; set; } = null!; // e.g. "+1-555-123-4567"


     public AttendeeType AttendeeType { get; set; } // e.g. "REQUIRED", "OPTIONAL", "INFORMATION_ONLY" 
       
    public int MeetingId { get; set; }
    public Meeting Meeting { get; set; } = null!;

    public int? OrganizationId { get; set; }
    public Organization? Organization { get; set; }

}

public enum AttendeeType
{
    MEMBER,
    MANAGEMENT_MEMBER,
    OBSERVER,
    ATTENDEE
}