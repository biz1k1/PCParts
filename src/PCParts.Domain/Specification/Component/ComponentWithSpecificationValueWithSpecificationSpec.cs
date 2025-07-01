using System.Linq.Expressions;
using PCParts.Domain.Specification.Base;

namespace PCParts.Domain.Specification.Component;

public class ComponentWithSpecificationValueWithSpecificationSpec : Specification<Entities.Component>
{
    public ComponentWithSpecificationValueWithSpecificationSpec()
    {
        AddInclude(c => c.SpecificationValues);
        AddThenInclude(x => x.SpecificationValues, x => x.Specification);
    }

    public override Expression<Func<Entities.Component, bool>> ToExpression()
    {
        return c => true;
    }
}