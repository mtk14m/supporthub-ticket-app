namespace SupportHub.Application.Repositories;

using SupportHub.Domain.Tickets;

public interface ITicketRepository
{
    Task AddAsync(Ticket ticket);
    Task<Ticket?>GetByIdAsync(Guid id);
    Task<IEnumerable<Ticket>> GetAllAsync();
    Task UpdateAsync(Ticket ticket);
}