namespace API.Domain.Entities;

public class AgendaItem : BaseEntity
{
    public string Title { get; set; } = null!; // e.g. "Project Update", "Budget Review", "Team Building Activity"
    public string Description { get; set; } = null!; // e.g. "Discussion on the current status of the project, including progress, challenges, and next steps."
    public int AgendaItemTypeId { get; set; }
    public AgendaItemType AgendaItemType { get; set; } = null!;
    public string AgendaItemTypeName { get; set; } = null!; // e.g. "Discussion", "Decision", "Information"

    public int Order { get; set; } // e.g. 1 (the order of the agenda item in the meeting agenda)
    public int DurationInMinutes { get; set; } // e.g. 30 (duration of the agenda item in minutes)

    public string DiscussionSummary { get; set; } = null!; // e.g. "The project is on track, but we are facing some challenges with the new API integration. We need to allocate more resources to address these issues."
    public int MeetingId { get; set; }
    public Meeting Meeting { get; set; } = null!;

    public ICollection<AgendaItemDecision> Decisions { get; set; } = new List<AgendaItemDecision>();
}

