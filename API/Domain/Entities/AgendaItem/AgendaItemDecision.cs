namespace API.Domain.Entities;

public class AgendaItemDecision : BaseEntity
{
    public string Code { get; set; } = null!; // e.g. "ITEM_DECISION_001"
    public int AgendaItemId { get; set; }
    public AgendaItem AgendaItem { get; set; } = null!;
    public string DecisionText { get; set; } = null!; // e.g. "Approve the budget for Q3 marketing campaign."
    public string? Notes { get; set; } // e.g. "The decision was made after a thorough discussion on the potential ROI and risks associated with the campaign."

    // TODO: Beneficiary could be organization, project team, or external partner in addition to an individual user. Consider adding a BeneficiaryType property to distinguish between different types of beneficiaries or just use a string property to store the beneficiary information in a flexible way.
    public AppUser Beneficiary { get; set; } = null!; // e.g. "John Doe" (the person or entity that benefits from the decision, such as a department, project team, or external partner)
    public AppUser ImplementationResponsible { get; set; } = null!; // e.g. "Jane Smith" (the person responsible for implementing the decision, such as a project manager or team lead)

    public DateTime? DueDate { get; set; } // e.g. "2024-12-31" (the deadline for implementing the decision, if applicable)

    public DecisionStatus Status { get; set; } // e.g. "OPEN", "COMPLETED", "PENDING", "LATE", "POSTPONED", "CANCELLED"
} 

public enum DecisionStatus
{
    COMPLETED,
    OPEN,
    PENDING,
    LATE,
    POSTPONED,
    CANCELLED
}