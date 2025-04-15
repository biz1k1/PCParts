using PCParts.Application.Model.Models;

namespace PCParts.Application.Model.Command;

public record CreateComponentCommand(string Name, Guid CategoryId, ICollection<CreateSpecificationValueCommand> SpecificationValues);