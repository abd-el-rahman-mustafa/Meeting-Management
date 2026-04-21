namespace API.Application.DTOs;

public class AgendaItemTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}

public class UpsertAgendaItemTypeDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}