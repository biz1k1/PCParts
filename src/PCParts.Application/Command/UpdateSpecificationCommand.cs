using PCParts.Application.Model.Enums;

namespace PCParts.Application.Command;

public record UpdateSpecificationCommand(Guid Id, string? Name, SpecificationDataType? Type);