using PCParts.Domain.Specification.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PCParts.Domain.Specification.Specification
{
    public class SpecificationWithSpecificationValueSpec: Specification<Entities.Specification>
    {
        public SpecificationWithSpecificationValueSpec()
        {
            AddInclude(c => c.SpecificationValues);
        }

        public override Expression<Func<Entities.Specification, bool>> ToExpression()
        {
            return c => true;
        }
    }
}
