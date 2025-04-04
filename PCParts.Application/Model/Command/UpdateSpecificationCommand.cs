using PCParts.Domain.Enum;

namespace PCParts.Application.Model.Command;

public record UpdateSpecificationCommand(Guid Id, string? Name, string? Value, SpecificationDataType? Type);