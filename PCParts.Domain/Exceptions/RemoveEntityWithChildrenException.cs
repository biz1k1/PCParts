using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCParts.Domain.Exceptions
{
    public class RemoveEntityWithChildrenException(string parent, string child) :
        DomainException(DomainErrorCode.Conflict, $"The parent {parent} cannot be deleted because he has a child {child}");
}
