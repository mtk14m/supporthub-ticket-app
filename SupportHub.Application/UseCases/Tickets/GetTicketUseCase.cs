namespace SupportHub.Application.UseCases.Tickets;

public class GetTicketResponse
{
    public Guid id {get; set;}
    public string Reference {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    public int Status {get; set;}
    public int Priority {get; set;}
    public Guid CustomerId {get; set;}
    public Guid? AssignedAgentId {get; set;}
}

public interface IGetTicketUseCase
{
    Task<GetTicketResponse> ExecuteAsync(Guid tickedId);
}

public class GetTicketUseCase : IGetTicketUseCase
{
    public async Task<GetTicketResponse> ExecuteAsync(Guid tickedId)
    {
        return await Task.FromResult(new GetTicketResponse());
    }
}