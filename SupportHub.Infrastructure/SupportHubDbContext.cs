using Microsoft.EntityFrameworkCore;
using SupportHub.Domain.Tickets;
using SupportHub.Domain.Users;

namespace SupportHub.Infrastructure;

public class SupportHubDbContext : DbContext
{
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketMessage> TicketMessages { get; set; }
    public DbSet<User> Users { get; set; }

    public SupportHubDbContext(DbContextOptions<SupportHubDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure Ticket
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Reference).HasMaxLength(50).IsRequired();
            entity.Property(t => t.Title).HasMaxLength(200).IsRequired();
            entity.Property(t => t.Description).IsRequired();
            entity.Property(t => t.Status).HasConversion<int>();
            entity.Property(t => t.Priority).HasConversion<int>();
        });

        // Configure TicketMessage
        modelBuilder.Entity<TicketMessage>(entity =>
        {
            entity.HasKey(tm => tm.Id);
            entity.Property(tm => tm.Body).IsRequired();
            entity.Property(tm => tm.IsInternalNote).HasDefaultValue(false);
        });

        // Configure User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).HasMaxLength(255).IsRequired();
            entity.Property(u => u.DisplayName).HasMaxLength(100).IsRequired();
            entity.Property(u => u.Role).HasConversion<int>();
            entity.Property(u => u.IsActive).HasDefaultValue(true);
        });
    }
}