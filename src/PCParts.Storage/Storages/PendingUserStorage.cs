using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PCParts.Application.Abstraction.Storage;
using PCParts.Domain.Entities;

namespace PCParts.Storage.Storages;

public class PendingUserStorage : IPendingUserStorage
{
    private readonly PgContext _pgContext;

    public PendingUserStorage(
        PgContext pgContext)
    {
        _pgContext = pgContext;
    }

    public async Task<PendingUser?> GetPendingUser(Expression<Func<PendingUser, bool>> predicate,
        CancellationToken cancellationToken)
    {
        return await _pgContext.PendingUsers.Where(predicate).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PendingUser?> CreatePendingUser(string phone,
        string passwordHash, CancellationToken cancellationToken)
    {
        var user = new PendingUser
        {
            Id = Guid.NewGuid(),
            Phone = phone,
            PasswordHash = passwordHash,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _pgContext.PendingUsers.Add(user);
        await _pgContext.SaveChangesAsync(cancellationToken);

        return await _pgContext.PendingUsers
            .Where(x => x.Id == user.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}