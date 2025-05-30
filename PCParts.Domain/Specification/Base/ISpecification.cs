using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PCParts.Domain.Specification.Base
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
        IReadOnlyList<Expression<Func<T, object>>> Includes { get; }
        IReadOnlyList<(LambdaExpression Include, LambdaExpression Then)> ThenIncludes { get; }
    }
}
