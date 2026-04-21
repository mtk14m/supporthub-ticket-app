namespace SupportHub.Domain.Tickets;

public class Ticket
{

    private readonly List<TicketMessage> _messages = new();
    
    public Guid Id { get; private set; }
    public string Reference { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public TicketStatus Status { get; private set; }
    public TicketPriority Priority { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid? AssignedAgentId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public DateTimeOffset? ResolvedAt { get; private set; }
    public DateTimeOffset? ClosedAt { get; private set; }

    public IReadOnlyCollection<TicketMessage> Messages => _messages.AsReadOnly();


    private Ticket() { }

    public Ticket(
        Guid id,
        string reference,
        string title,
        string description,
        TicketPriority priority,
        Guid customerId,
        DateTimeOffset createdAt
    )
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        if (string.IsNullOrWhiteSpace(reference))
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
        Status = TicketStatus.Open;
        Priority = priority;
        CustomerId = customerId;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }


    public void AssignTo(Guid agentId, DateTimeOffset changedAt)
    {
        if (Status == TicketStatus.Closed)
            throw new InvalidOperationException("Cannot assign a closed ticket.");

        if (agentId == Guid.Empty)
            throw new ArgumentException("AgentId cannot be empty", nameof(agentId));

        AssignedAgentId = agentId;
        UpdatedAt = changedAt;
    }

    public void StartProgress(DateTimeOffset changedAt)
    {
        if (Status == TicketStatus.Closed)
            throw new InvalidOperationException("Cannot start progress on a closed ticket.");

        Status = TicketStatus.InProgress;
        UpdatedAt = changedAt;
    }

    public void UpdatePriority(TicketPriority newPriority, DateTimeOffset changedAt)
    {
        if (Status == TicketStatus.Closed)
            throw new InvalidOperationException("Cannot update priority of a closed ticket.");

        Priority = newPriority;
        UpdatedAt = changedAt;
    }

    public void MarkAsResolved(DateTimeOffset resolvedAt)
    {
        if (Status == TicketStatus.Closed)
            throw new InvalidOperationException("Cannot resolve a closed ticket.");

        if (_messages.Count == 0)
            throw new InvalidOperationException("Cannot resolve a ticket without any message.");
        Status = TicketStatus.Resolved;
        ResolvedAt = resolvedAt;
        UpdatedAt = resolvedAt;
    }

    public void Close(DateTimeOffset closedAt)
    {

        if (Status != TicketStatus.Resolved)
            throw new InvalidOperationException("Only a resolved ticket can be closed.");
        Status = TicketStatus.Closed;
        ClosedAt = closedAt;
        UpdatedAt = closedAt;
    }

    public void Reopen(DateTimeOffset reopenedAt)
    {
        if (Status != TicketStatus.Resolved && Status != TicketStatus.Closed)
            throw new InvalidOperationException("Only a resolved or closed ticket can be reopened.");
        Status = TicketStatus.Open;
        ResolvedAt = null;
        ClosedAt = null;
        UpdatedAt = reopenedAt;
    }


    public void AddMessage(
        Guid messageId,
        Guid authorId, 
        string body,
        bool isInternalNote,
        DateTimeOffset createdAt)
    {
        if (Status == TicketStatus.Closed)
            throw new InvalidOperationException("Cannot add a message to a closed ticket.");

        var message = new TicketMessage(
            messageId, 
            Id, 
            authorId,
            body, 
            isInternalNote, 
            createdAt
        );

        _messages.Add(message);
        UpdatedAt = createdAt;
    }
}