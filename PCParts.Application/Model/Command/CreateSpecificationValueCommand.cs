namespace PCParts.Application.Model.Command;

public record CreateSpecificationValueCommand(Guid componentId, Guid specificationId, string value);