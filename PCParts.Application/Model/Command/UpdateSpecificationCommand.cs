using PCParts.Application.Model.Enum;

namespace PCParts.Application.Model.Command;

public record UpdateSpecificationCommand(Guid Id, string? Name, SpecificationDataType? Type);