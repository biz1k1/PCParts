using PCParts.Domain.Specification.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PCParts.Domain.Specification.SpecificationValue
{
    public class SpecificationValueWithSpecificationSpec: Specification<Entities.SpecificationValue>
    {
        public SpecificationValueWithSpecificationSpec()
        {
            AddInclude(c => c.Specification);
        }

        public override Expression<Func<Entities.SpecificationValue, bool>> ToExpression()
        {
            return c => true;
        }
    }
}
