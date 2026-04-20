namespace SupportHub.Domain.Tickets;

public class TicketMessage
{
    public Guid Id {get; private set;}
    public Guid TicketId {get; private set;}
    public Guid AuthorId {get; private set;}
    public string Body {get; private set;}
    public bool IsInternalNote {get; private set;}
    public DateTimeOffset CreatedAt {get; private set;}

    private TicketMessage()
    {
    }

    public TicketMessage(
        Guid id, 
        Guid ticketId, 
        Guid authorId, 
        string body, 
        bool isInternalNote, 
        DateTimeOffset createdAt
    )
    {

        if (id == Guid.Empty)
            throw new ArgumentException("id cannot be empty", nameof(id));

        if (ticketId == Guid.Empty)
            throw new ArgumentException("ticketId cannot be empty", nameof(ticketId));

        if (authorId == Guid.Empty)
            throw new ArgumentException("authorId cannot be empty", nameof(authorId));

        if (string.IsNullOrWhiteSpace(body))
            throw new ArgumentException("body cannot be null or whitespace", nameof(body));

        if (createdAt > DateTimeOffset.UtcNow)
            throw new ArgumentException("CreatedAt cannot be in the future.", nameof(createdAt));
        
        Id= id;
        TicketId = ticketId;
        AuthorId = authorId;
        Body = body;
        IsInternalNote = isInternalNote;
        CreatedAt = createdAt;  
    }


}