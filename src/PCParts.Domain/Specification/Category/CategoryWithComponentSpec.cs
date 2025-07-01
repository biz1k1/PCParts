using System.Linq.Expressions;
using PCParts.Domain.Specification.Base;

namespace PCParts.Domain.Specification.Category;

public class CategoryWithComponentSpec : Specification<Entities.Category>
{
    public CategoryWithComponentSpec()
    {
        AddInclude(c => c.Components);
    }

    public override Expression<Func<Entities.Category, bool>> ToExpression()
    {
        return c => true;
    }
}