namespace SupportHub.Domain.Tickets;

public class Ticket
{
    public Guid Id { get; private set; }
    public string Reference { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public TicketStatus Status { get; private set; }
    public TicketPriority Priority { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid? AssignendAgentId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }
    public DateTimeOffset? ClosedAt { get; private set; }

    private Ticket(){}

    private Ticket(
        Guid id, 
        string reference,
        string title,
        string description,
        TicketStatus status,
        TicketPriority priority,
        Guid customerId,
        DateTimeOffset createdAt
    )
    {
        if (id==Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        if(string.IsNullOrWhiteSpace(reference))
            throw new ArgumentException("Refrence cannot be empty", nameof(reference));
         if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or whitespace", nameof(title));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Title cannot be null or whitespace", nameof(description));
        if (customerId == Guid.Empty)
            throw new ArgumentException("CustomerId cannot be empty", nameof(customerId));
        if (createdAt > DateTimeOffset.UtcNow)
            throw new ArgumentException("CreatedAt cannot be in the futur", nameof(createdAt));

        
        Id = id;
        Reference = reference;
        Title = title;
        Description = description;
        Status = status;
        Priority = priority;
        CustomerId = customerId;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }


     public void AssignAgent(Guid agentId)
    {
        if (agentId == Guid.Empty)
            throw new ArgumentException("AgnetId cannot be empty", nameof(agentId));
        
        AssignendAgentId = agentId;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void ChangeStatus(TicketStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTimeOffset.UtcNow;

        if (newStatus == TicketStatus.Resolved)
            ResolvedAt = DateTimeOffset.UtcNow;
        else if (newStatus == TicketStatus.Closed)
            ClosedAt = DateTimeOffset.UtcNow;
    }

    public void UpdatePriority(TicketPriority newPriority)
    {
        Priority = newPriority;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

}