using PCParts.Domain.Enum;

namespace PCParts.Application.Model.Command;

public record CreateSpecificationCommand(Guid ComponentId, string? Name, string? Value, SpecificationDataType DataType);