namespace API.Domain.Entities;

public class AppUser 
{

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required Gender Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;



}

public enum Gender
{
    NotSpecified,
    Male,
    Female
}
