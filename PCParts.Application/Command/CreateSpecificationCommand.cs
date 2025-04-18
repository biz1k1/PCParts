using PCParts.Application.Model.Enum;

namespace PCParts.Application.Command;

public record CreateSpecificationCommand(Guid CategoryId, string Name, SpecificationDataType Type);