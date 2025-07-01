using System.Linq.Expressions;
using PCParts.Domain.Specification.Base;

namespace PCParts.Domain.Specification.Specification;

public class SpecificationWithSpecificationValueSpec : Specification<Entities.Specification>
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