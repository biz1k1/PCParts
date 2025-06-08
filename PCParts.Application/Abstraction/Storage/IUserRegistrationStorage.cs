using PCParts.Domain.Entities;
using System.Linq.Expressions;

namespace PCParts.Application.Abstraction.Storage;

public interface IPendingUserStorage:IStorage
{
    public Task<PendingUser> GetPendingUser(Expression<Func<PendingUser, bool>> predicate, 
        CancellationToken cancellationToken);
    public Task<PendingUser> CreatePendingUser(string phone,
        string passwordHash, CancellationToken cancellationToken);
}