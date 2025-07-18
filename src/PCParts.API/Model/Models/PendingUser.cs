namespace PCParts.API.Model.Models;

public record PendingUser
{
    public Guid Id { get; init; }
    public string Email { get; init; }
}