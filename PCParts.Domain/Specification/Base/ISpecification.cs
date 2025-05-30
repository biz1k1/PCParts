using System.Linq.Expressions;

namespace PCParts.Domain.Specification.Base;

public interface ISpecification<T>
{
    IReadOnlyList<Expression<Func<T, object>>> Includes { get; }
    IReadOnlyList<(LambdaExpression Include, LambdaExpression Then)> ThenIncludes { get; }
    Expression<Func<T, bool>> ToExpression();
}