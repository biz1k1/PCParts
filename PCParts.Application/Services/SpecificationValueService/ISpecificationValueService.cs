using PCParts.Application.Model.Command;
using PCParts.Application.Model.Models;

namespace PCParts.Application.Services.SpecificationValueService;

public interface ISpecificationValueService
{
    Task<SpecificationValue> UpdateSpecificationValue(UpdateSpecificationValueCommand command,
        CancellationToken cancellationToken);

    Task<SpecificationValue> CreateSpecificationsValues(Guid componentId, 
        ICollection<CreateSpecificationValueCommand> commands, CancellationToken cancellationToken);
}