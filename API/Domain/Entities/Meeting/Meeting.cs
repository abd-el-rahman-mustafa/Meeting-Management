namespace API.Domain.Entities;

public class Meeting : BaseEntity
{
    // main data
    public string Code { get; set; } = null!; // e.g. generated automatically from the system  "MTG-2024-0001"
    public string Title { get; set; } = null!; // e.g. "Project Kickoff"
    public string Description { get; set; } = null!; // e.g. "Initial meeting to discuss project scope and deliverables."
    public DateTimeOffset StartTime { get; set; } // e.g. "2024-07-01T10:00:00Z" dynamically set upon starting the meeting
    public DateTimeOffset EndTime { get; set; } // e.g. "2024-07-01T11:00:00Z" dynamically set upon ending the meeting
    public LocationType LocationType { get; set; } // e.g. LocationType.Online or LocationType.Hybrid
    public string? Location { get; set; } // e.g. "https://zoom.us/j/123456789" for online or "Conference Room A" for hybrid or in-person

    // Session Occurrence
    public SessionOccurrence SessionOccurrence { get; set; } // e.g. SessionOccurrence.FirstOccurrence, SessionOccurrence.SecondOccurrence, etc.

    // meeting type 
    public int MeetingTypeId { get; set; } // e.g. 1 for "Team Meeting", 2 for "Client Meeting", etc.
    public MeetingType MeetingType { get; set; } = null!;

    public string MeetingTypeName { get; set; } = null!; // e.g. "Team Meeting", "Client Meeting", "Project Update"
    public string MeetingTypeDescription { get; set; } = null!; // e.g. "Regular team meetings to discuss project progress and blockers."

    // Meeting Category
    public int MeetingCategoryId { get; set; } // e.g. 1 for "Strategic", 2 for "Operational", etc.
    public MeetingCategory MeetingCategory { get; set; } = null!;
    
    public string MeetingCategoryName { get; set; } = null!; // e.g. "Strategic Meeting", "Operational Meeting", "Project Meeting"
    public string MeetingCategoryDescription { get; set; } = null!; // e.g


    // Attendees & Observers

    // Members Quorum
    public int RequiredMembersQuorum { get; set; } // e.g. 5 (minimum number of attendees required for the meeting to proceed) will be filled by the system based on the session occurrence and the total number of attendees invited to the meeting. For example, if there are 10 attendees invited to a meeting and it's the first occurrence, the quorum might be set to 5. If it's the second occurrence, the quorum might be set to 3, and so on (admin config).
    public int ActualMembersQuorum { get; set; } // e.g. 6 (actual number of attendees who joined the meeting) will be updated by the system in real-time as attendees join the meeting. This can be used to determine if the meeting can proceed based on the required quorum.


    //Management Quorum
    public int RequiredManagementQuorum { get; set; } // e.g. 2 (minimum number of management attendees required for the meeting to proceed) will be filled by the system based on the session occurrence and the total number of management attendees invited to the meeting. For example, if there are 4 management attendees invited to a meeting and it's the first occurrence, the quorum might be set to 2. If it's the second occurrence, the quorum might be set to 1, and so on (admin config).
    public int ActualManagementQuorum { get; set; } // e.g. 3 (actual number of management attendees who joined the meeting) will be updated by the system in real-time as management attendees join the meeting. This can be used to determine if the meeting can proceed based on the required management quorum.


    public string OrganizerId { get; set; } = null!;
    public AppUser Organizer { get; set; } = null!;
    public ICollection<MeetingAttendee> Attendees { get; set; } = new List<MeetingAttendee>();
    public ICollection<MeetingObserver> Observers { get; set; } = new List<MeetingObserver>();
    public ICollection<AgendaItem> AgendaItems { get; set; } = new List<AgendaItem>();
}

public enum LocationType
{
    Online = 1,
    Hybrid = 2,
    InPerson = 3
}

public enum SessionOccurrence
{
    FirstOccurrence = 1,
    SecondOccurrence = 2,
    ThirdOccurrence = 3,
    FourthOccurrence = 4,
    FifthOccurrence = 5
}
