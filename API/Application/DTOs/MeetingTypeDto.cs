namespace API.Application.DTOs;

public class MeetingTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class UpsertMeetingTypeDto
{
    public string Name { get; set; } = null!;
}
