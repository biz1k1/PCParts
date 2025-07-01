using PCParts.Application.Model.Enum;

namespace PCParts.Application.Command;

public record UpdateSpecificationCommand(Guid Id, string? Name, SpecificationDataType? Type);