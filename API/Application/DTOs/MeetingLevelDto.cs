namespace API.Application.DTOs;

public class MeetingLevelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class UpsertMeetingLevelDto
{
    public string Name { get; set; } = null!;
}
