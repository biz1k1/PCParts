using System.Linq.Expressions;

namespace PCParts.Domain.Specification.Base;

public abstract class Specification<T> : ISpecification<T>
{
    private readonly List<Expression<Func<T, object>>> _includes = new();
    private readonly List<(LambdaExpression Include, LambdaExpression Then)> _thenIncludes = new();

    public IReadOnlyList<Expression<Func<T, object>>> Includes => _includes.AsReadOnly();
    public IReadOnlyList<(LambdaExpression Include, LambdaExpression Then)> ThenIncludes => _thenIncludes.AsReadOnly();

    public abstract Expression<Func<T, bool>> ToExpression();

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        _includes.Add(includeExpression);
    }

    protected void AddThenInclude<TPrevious, TProperty>(
        Expression<Func<T, IEnumerable<TPrevious>>> include,
        Expression<Func<TPrevious, TProperty>> thenInclude)
    {
        _thenIncludes.Add((include, thenInclude));
    }
}