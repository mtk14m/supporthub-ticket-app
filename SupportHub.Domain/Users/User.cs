namespace SupportHub.Domain.Users;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string DisplayName { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private User()
    {
    }

    public User(
        Guid id,
        string email,
        string displayName,
        UserRole role,
        DateTimeOffset createdAt)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or whitespace.", nameof(email));

        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name cannot be null or whitespace.", nameof(displayName));

        if (createdAt > DateTimeOffset.UtcNow)
            throw new ArgumentException("CreatedAt cannot be in the future.", nameof(createdAt));

        Id = id;
        Email = email;
        DisplayName = displayName;
        Role = role;
        CreatedAt = createdAt;
        IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;
    }

    public void ChangeDisplayName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name cannot be null or whitespace.", nameof(displayName));

        DisplayName = displayName;
    }
}