namespace PCParts.Application.Command;

public record CreateSpecificationValueCommand(Guid SpecificationId, string Value);
