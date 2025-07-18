namespace PCParts.API.Model.Models;

public record CreatePendingUser
{
    public string Email { get; init; }
    public string Password { get; init; }
}