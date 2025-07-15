namespace PCParts.Application.Command;

public record CreatePendingUserCommand(string Email, string Password);