namespace API.Domain.Entities;

public class MeetingAttendee : BaseEntity
{
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;

    public AttendeeType AttendeeType { get; set; } // e.g. MEMBER or MANAGEMENT_MEMBER    
    public int MeetingId { get; set; }
    public Meeting Meeting { get; set; } = null!;

    // user static data after the meeting is created, to avoid issues with user data changes after the meeting is created
    public string Name { get; set; } = null!; // e.g. "John Doe"
    public string Email { get; set; } = null!; // e.g. " Johne@gmail.com"
    public string PhoneNumber { get; set; } = null!; // e.g. "+1-555-123-4567"
}

public enum AttendeeType
{
    MEMBER,
    MANAGEMENT_MEMBER,
}