using Microsoft.EntityFrameworkCore;
using SupportHub.Application.Repositories;
using SupportHub.Domain.Tickets;

namespace SupportHub.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly SupportHubDbContext _context;

    public TicketRepository(SupportHubDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Ticket ticket)
    {
        await _context.Tickets.AddAsync(ticket);
        await _context.SaveChangesAsync();
    }

    public async Task<Ticket?> GetByIdAsync(Guid id)
    {
        return await _context.Tickets.FindAsync(id);
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _context.Tickets.ToListAsync();
    }

    public async Task UpdateAsync(Ticket ticket)
    {
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
    }
}