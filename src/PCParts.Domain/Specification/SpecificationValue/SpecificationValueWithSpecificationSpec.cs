using System.Linq.Expressions;
using PCParts.Domain.Specification.Base;

namespace PCParts.Domain.Specification.SpecificationValue;

public class SpecificationValueWithSpecificationSpec : Specification<Entities.SpecificationValue>
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