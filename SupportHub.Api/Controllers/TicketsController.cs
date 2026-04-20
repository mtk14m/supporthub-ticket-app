

using Microsoft.AspNetCore.Mvc;
using SupportHub.Application.Tickets;
using SupportHub.Application.UseCases.Tickets;

namespace SupportHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ICreateTicketUseCase _createTicketUseCase;
    private readonly IGetTicketUseCase _getTicketUseCase;
    private readonly IAssignTicketUseCase _assignTicketUseCase;

    public TicketsController(
        ICreateTicketUseCase createTicketUseCase,
        IGetTicketUseCase getTicketUseCase,
        IAssignTicketUseCase assignTicketUseCase
    )
    {
        _createTicketUseCase = createTicketUseCase;
        _getTicketUseCase = getTicketUseCase;
        _assignTicketUseCase = assignTicketUseCase;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicket(Guid id)
    {
        var response = await _getTicketUseCase.ExecuteAsync(id);
        if (response == null)
            return NotFound();
        return Ok(response);
    }

    [HttpPut("{id}/assign")]
    public async Task<IActionResult> AssignTicket(Guid id, [FromBody] AssignTicketRequest request)
    {
        request.TicketId = id;
        await _assignTicketUseCase.ExecuteAsync(request);
        return NoContent();
    }

}