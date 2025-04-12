using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PCParts.Storage.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyInclude<T>(this IQueryable<T> query, IEnumerable<string> includes)
        {
            foreach (var include in includes)
            {
                var properties = include.Split('.');
                var parameter = Expression.Parameter(typeof(T), "x");
                Expression body = parameter;
                var currentType = typeof(T);

                for (int i = 0; i < properties.Length; i++)
                {
                    var property = currentType.GetProperty(properties[i]);
                    if (property == null)
                    {
                        throw new ArgumentException(
                            $"Property '{properties[i]}' not found on type '{currentType.Name}'.");
                    }

                    body = Expression.PropertyOrField(body, properties[i]);
                    currentType = body.Type;

                    // Если это первый уровень, используем Include
                    var methodName = i == 0 ? "Include" : "ThenInclude";

                    // Для вложенных коллекций, после первого уровня, используем ThenInclude
                    if (i > 0 && typeof(IEnumerable).IsAssignableFrom(currentType))
                    {
                        methodName = "ThenInclude";
                    }

                    var method = typeof(EntityFrameworkQueryableExtensions)
                        .GetMethods()
                        .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                        .MakeGenericMethod(typeof(T), currentType);

                    // Применяем Include или ThenInclude
                    query = (IQueryable<T>)method.Invoke(null,
                        new object[] { query, Expression.Lambda(body, parameter) });
                }
            }

            return query;
        }
    }
}
