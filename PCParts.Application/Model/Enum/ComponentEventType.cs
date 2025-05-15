using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCParts.Application.Model.Enum
{
    public enum ComponentEventType
    {
        ComponentCreated = 100,
        ComponentUpdated = 101,
        ComponentDeleted = 102,

        SpecificationValueCreated = 200,
        SpecificationValueUpdated = 201,
        SpecificationValueDeleted = 202,
    }
}
