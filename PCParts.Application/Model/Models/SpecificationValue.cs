using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCParts.Application.Model.Models
{
    public record SpecificationValue
    {
        public Guid Id { get; init; }
        public string SpecificationName { get; init; }
        public object Value { get; init; }
    }
}
