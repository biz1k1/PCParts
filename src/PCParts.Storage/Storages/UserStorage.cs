using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PCParts.Application.Abstraction.Storage;
using PCParts.Domain.Entities;

namespace PCParts.Storage.Storages;

public class UserStorage : IUserStorage
{
    private readonly PgContext _pgContext;

    public UserStorage(
        PgContext pgContext)
    {
        _pgContext = pgContext;
    }

    public async Task<User?> GetUser(Expression<Func<User, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return await _pgContext.Users
            .Where(predicate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> CreateUser(User user, CancellationToken cancellationToken)
    {
        user.Id = new Guid();

        await _pgContext.AddAsync(user, cancellationToken);
        return await _pgContext.Users.FindAsync([user.Id], cancellationToken);
    }
}
