namespace PCParts.Application.Model.Command;

public record CreateSpecificationValueCommand(Guid SpecificationId, string Value);