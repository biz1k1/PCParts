using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCParts.Domain.Exceptions
{
    public class SpecificationsNotFoundException(IEnumerable<Guid> specificationId) :
        DomainException(DomainErrorCode.NotFound, $"Specification with id {specificationId} was not found!");
}
