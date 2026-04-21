using SupportHub.Domain.Tickets;

namespace SupportHub.Tests.Domain.Tickets;

public class TicketTests
{
    [Fact]
    public void Constructor_Should_Create_Open_Ticket()
    {
        var now = new DateTimeOffset(2026, 4, 21, 01, 0, 0, TimeSpan.Zero);
        var customerId = Guid.NewGuid();

        var ticket = new Ticket(
            Guid.NewGuid(),
            "TKT-00000001", 
            "Cannot login", 
            "I cannot loginto my account.",
            TicketPriority.Normal, 
            customerId, 
            now
        );

        Assert.Equal(TicketStatus.Open, ticket.Status);
        Assert.Equal(customerId, ticket.CustomerId);
        Assert.Equal(now, ticket.CreatedAt);
        Assert.Equal(now, ticket.UpdatedAt);
        Assert.Empty(ticket.Messages);
        Assert.Null(ticket.AssignedAgentId);
        Assert.Null(ticket.ResolvedAt);
        Assert.Null(ticket.ClosedAt);
    }

    [Fact]
    public void AddMessage_Should_Throw_When_Ticket_Is_Closed()
    {
        var now = new DateTimeOffset(2026, 4, 21, 10, 0, 0, TimeSpan.Zero);

        var ticket = new Ticket(
            Guid.NewGuid(),
            "TKT-000002",
            "Cannot reset password",
            "The reset password link does not work.",
            TicketPriority.High,
            Guid.NewGuid(),
            now
        );

        ticket.AddMessage(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "We are checking this issue.",
            false,
            now.AddMinutes(5)
        );

        ticket.MarkAsResolved(now.AddMinutes(10));
        ticket.Close(now.AddMinutes(15));

        var exception = Assert.Throws<InvalidOperationException>(() =>
            ticket.AddMessage(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "New message after closure.",
                false,
                now.AddMinutes(20)
            )
        );

        Assert.Equal("Cannot add a message to a closed ticket.", exception.Message);
    }

    [Fact]
    public void MarkAsResolved_Should_Throw_When_Ticket_Has_No_Message()
    {
        var now = new DateTimeOffset(2026, 4, 21, 10, 0, 0, TimeSpan.Zero);

        var ticket = new Ticket(
            Guid.NewGuid(),
            "TKT-000003",
            "Cannot access dashboard",
            "The dashboard returns a blank page.",
            TicketPriority.Normal,
            Guid.NewGuid(),
            now
        );

        var exception = Assert.Throws<InvalidOperationException>(() =>
            ticket.MarkAsResolved(now.AddMinutes(10))
        );

        Assert.Equal("Cannot resolve a ticket without any message.", exception.Message);
    }

    [Fact]
    public void AssignTo_Should_Set_AssignedAgentId_And_UpdateAt()
    {
        var now = new DateTimeOffset(2026, 4, 21, 10, 0, 0, TimeSpan.Zero);
        var assignedAt = now.AddMinutes(3);
        var agentId = Guid.NewGuid();

        var ticket = new Ticket(
            Guid.NewGuid(),
            "TKT-000004", 
            "cannot upload file", 
            "Upload fails with a timeout.",
            TicketPriority.High, 
            Guid.NewGuid(),
            now
        );

        ticket.AssignTo(agentId, assignedAt);

        Assert.Equal(agentId, ticket.AssignedAgentId);
        Assert.Equal(assignedAt, ticket.UpdatedAt);
    }

}

