using PCParts.Application.Model.Enums;

namespace PCParts.Application.Command;

public record CreateSpecificationCommand(Guid CategoryId, string Name, SpecificationDataType Type);