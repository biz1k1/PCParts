namespace PCParts.Application.Model.Command;

public record UpdateComponentCommand(Guid Id, string? Name, Guid? CategoryId);