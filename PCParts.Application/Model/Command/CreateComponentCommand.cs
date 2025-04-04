namespace PCParts.Application.Model.Command;

public record CreateComponentCommand(string Name, Guid CategoryId);