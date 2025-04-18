using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCParts.Domain.Exceptions
{
    public class CollectionEntitiesNotFoundException(string entitiesName, IEnumerable<Guid> entitiesId) :
        DomainException(DomainErrorCode.NotFound, $"{entitiesName} with ids {entitiesId} was not found!");
}
