using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCParts.Application.Model.Command
{
    public record CreateSpecificationValueCommand(Guid componentId, Guid specificationId, string value);
}
