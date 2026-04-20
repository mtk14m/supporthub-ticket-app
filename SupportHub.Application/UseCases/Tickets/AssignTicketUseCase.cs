namespace SupportHub.Application.UseCases.Tickets;

public class AssignTicketRequest
{
    public Guid TicketId { get; set; }
    public Guid AgentId { get; set; }
}

public interface IAssignTicketUseCase
{
    Task ExecuteAsync(AssignTicketRequest request);
}

public class AssignTicketUseCase : IAssignTicketUseCase
{
    public async Task ExecuteAsync(AssignTicketRequest request)
    {
        await Task.CompletedTask;
    }
}