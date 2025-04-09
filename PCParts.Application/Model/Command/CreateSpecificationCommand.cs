using PCParts.Domain.Enum;

namespace PCParts.Application.Model.Command;

public record CreateSpecificationCommand(Guid CategoryId, string? Name, SpecificationDataType DataType);