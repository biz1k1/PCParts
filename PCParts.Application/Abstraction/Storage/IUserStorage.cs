using PCParts.Domain.Entities;
using System.Linq.Expressions;

namespace PCParts.Application.Abstraction.Storage
{
    public interface IUserStorage
    {
        Task<User> GetUser(Expression<Func<User, bool>> predicate,
            CancellationToken cancellationToken);
        Task<User> CreateUser(User user, CancellationToken cancellationToken);
    }
}
