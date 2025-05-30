namespace PCParts.Application.Command;

public record CreateComponentCommand(
    string Name,
    Guid CategoryId,
    ICollection<CreateSpecificationValueCommand> SpecificationValues);