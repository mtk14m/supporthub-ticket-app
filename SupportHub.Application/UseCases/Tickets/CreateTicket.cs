using System.Data.Common;

namespace SupportHub.Application.Tickets;

public class CreateTicketRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int Priority { get; set; }
    public Guid CustomerId { get; set; }
}

public class CreateTicketResponse
{
    public Guid Id { get; set; }
    public string Reference { get; set; }
}

public interface ICreateTicketUseCase
{
    Task<CreateTicketResponse> ExecuteAsync(CreateTicketRequest request);
}

public class CreateTicketUseCase : ICreateTicketUseCase
{
    public async Task<CreateTicketResponse> ExecuteAsync(CreateTicketRequest request)
    {
        var ticketId = Guid.NewGuid();
        var reference = $"TKT-{DateTime.UtcNow.Ticks}";

        return await Task.FromResult(new CreateTicketResponse
        {
            Id = ticketId,
            Reference = reference
        });
    }
}