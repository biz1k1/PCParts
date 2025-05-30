using Microsoft.EntityFrameworkCore;
using PCParts.Domain.Specification.Base;

namespace PCParts.Storage.Extensions;

public static class SpecificationEvaluator
{
    public static IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, ISpecification<T> spec)
        where T : class
    {
        if (spec is null)
        {
            return query;
        }

        query = query.Where(spec.ToExpression());

        foreach (var include in spec.Includes)
        {
            query = query.Include(include);
        }

        foreach (var (includeExpr, thenIncludeExpr) in spec.ThenIncludes)
        {
            var includable = EntityFrameworkQueryableExtensions.Include(query, (dynamic)includeExpr);
            query = EntityFrameworkQueryableExtensions.ThenInclude((dynamic)includable, (dynamic)thenIncludeExpr);
        }

        return query;
    }
}