namespace API.Application.DTOs;

public class MeetingCategoryDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string NameAr { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string DescriptionAr { get; set; } = null!;
}

public class UpsertMeetingCategoryDto
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string NameAr { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string DescriptionAr { get; set; } = null!;
}
