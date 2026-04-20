namespace API.Application.DTOs;

public class MeetingCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}

public class UpsertMeetingCategoryDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}
